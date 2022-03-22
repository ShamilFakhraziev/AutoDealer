using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;

namespace Adlr.Workflows.AgreementActivities
{
    public class PaymentScheduleActivity : CodeActivity
    {
        protected override void Execute(CodeActivityContext context)
        {
            var wfContext = context.GetExtension<IWorkflowContext>();

            var traceService = context.GetExtension<ITracingService>();
            var serviceFactory = context.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(null);

            try
            {
                var agreement = service.Retrieve("adlr_agreement", wfContext.PrimaryEntityId,
                    new ColumnSet("adlr_creditperiod", "adlr_creditamount", "adlr_name"));

                var creditPeriod = agreement.GetAttributeValue<int>("adlr_creditperiod");

                var creditAmount = agreement.GetAttributeValue<Money>("adlr_creditamount");

                if (creditAmount != null)
                {
                    var month = creditPeriod * 12;
                    decimal invoiceAmount = creditAmount.Value / month;
                    for (int i = 1; i <= month; i++)
                    {
                        Entity invoice = new Entity("adlr_invoice");
                        invoice["adlr_name"] = $"Договор {agreement.GetAttributeValue<string>("adlr_name")}, {i} месяц";
                        invoice["adlr_date"] = DateTime.UtcNow;
                        invoice["adlr_paydate"] = DateTime.UtcNow.AddMonths(i);
                        invoice["adlr_dogovorid"] = agreement.ToEntityReference();
                        invoice["adlr_type"] = new OptionSetValue(222950001);
                        invoice["adlr_amount"] = new Money(invoiceAmount);
                        service.Create(invoice);
                    }

                }
                else
                {
                    throw new Exception("Credit period and credit amount was null");
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
