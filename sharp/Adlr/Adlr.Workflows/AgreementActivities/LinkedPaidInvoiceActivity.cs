using Adlr.Workflows.AgreementActivities.Handlers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;


namespace Adlr.Workflows.AgreementActivities
{
    public class LinkedPaidInvoiceActivity : CodeActivity
    {
        [Output("Is paid invoice linked")]
        public OutArgument<bool> IsLinked { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var wfContext = context.GetExtension<IWorkflowContext>();

            var traceService = context.GetExtension<ITracingService>();
            var serviceFactory = context.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(null);

            try
            {
                AgreementActivitiesService agreementActivities = new AgreementActivitiesService(service);
                var invoices = agreementActivities.GetInvoicesByLinkedAgreement(wfContext.PrimaryEntityId, true);

                IsLinked.Set(context, invoices.Entities.Count != 0);
                
            }
            catch(Exception ex)
            {
                traceService.Trace($"Возникла ошибка {ex.InnerException}, {ex.StackTrace}, {ex.TargetSite}");
                throw new InvalidWorkflowException(ex.Message);
            }

        }
    }
}
