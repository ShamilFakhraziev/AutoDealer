using Adlr.Workflows.AgreementActivities.Handlers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;


namespace Adlr.Workflows.AgreementActivities
{
    public class DeleteLinkedInvoicesActivity : CodeActivity
    {
        protected override void Execute(CodeActivityContext context)
        {
            var wfContext = context.GetExtension<IWorkflowContext>();

            var traceService = context.GetExtension<ITracingService>();
            var serviceFactory = context.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(null);

            try
            {
                AgreementActivitiesService activitiesService = new AgreementActivitiesService(service);
                var invoices = activitiesService.GetInvoicesByLinkedAgreement(wfContext.PrimaryEntityId, AgreementActivitiesService.Automatically);
                foreach (var item in invoices.Entities)
                {
                    service.Delete(item.LogicalName, item.Id);
                }
            }
            catch(Exception ex)
            {
                traceService.Trace($"Возникла ошибка {ex.InnerException}, {ex.StackTrace}, {ex.TargetSite}");
                throw new InvalidWorkflowException(ex.Message);
            }
        }
    }
}
