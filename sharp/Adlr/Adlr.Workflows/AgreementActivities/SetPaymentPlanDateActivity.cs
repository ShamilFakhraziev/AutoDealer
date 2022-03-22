using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;

namespace Adlr.Workflows.AgreementActivities
{
    public class SetPaymentPlanDateActivity : CodeActivity
    {
        protected override void Execute(CodeActivityContext context)
        {
            var wfContext = context.GetExtension<IWorkflowContext>();
            var traceService = context.GetExtension<ITracingService>();
            var serviceFactory = context.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(null);

            try
            {
                var agreement = service.Retrieve("adlr_agreement", wfContext.PrimaryEntityId, new ColumnSet("adlr_paymentplandate"));

                agreement["adlr_paymentplandate"] = DateTime.UtcNow.AddDays(1);

                service.Update(agreement);
            }
            catch(Exception ex)
            {
                traceService.Trace($"Возникла ошибка {ex.InnerException}, {ex.StackTrace}, {ex.TargetSite}");
                throw new InvalidWorkflowException(ex.Message);
            }
        }
    }
}
