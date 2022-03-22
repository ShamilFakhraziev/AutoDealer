using Adlr.Workflows.AgreementActivities.Handlers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;


namespace Adlr.Workflows.AgreementActivities
{
    public class LinkedManuallyTypeInvoiceActivity : CodeActivity
    {
        [Output("Is manually type invoice linked")]
        public OutArgument<bool> IsLinked { get; set; }

        protected override void Execute(CodeActivityContext context)
        {

            var wfContext = context.GetExtension<IWorkflowContext>();

            var traceService = context.GetExtension<ITracingService>(); 
            var serviceFactory = context.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(null);

            try
            { 
                AgreementActivitiesService activitiesService = new AgreementActivitiesService(service);
                var invoices = activitiesService.GetInvoicesByLinkedAgreement(wfContext.PrimaryEntityId, AgreementActivitiesService.Manually);

                IsLinked.Set(context, invoices.Entities.Count != 0);

            }
            catch (Exception ex)
            {
                traceService.Trace($"Возникла ошибка {ex.InnerException}, {ex.StackTrace}, {ex.TargetSite}");
                throw new InvalidWorkflowException(ex.Message);
            }
        }
    }
}
