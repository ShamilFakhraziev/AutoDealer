using Adlr.Plugins.Invoice.Handlers;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adlr.Plugins.Invoice
{
    public class PreInvoiceUpdate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var traceService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            var pluginContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var targetInvoice = (Entity)pluginContext.InputParameters["Target"];

            var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = serviceFactory.CreateOrganizationService(Guid.Empty);

            InvoiceService invoiceService = new InvoiceService(service);

            var fact = targetInvoice.GetAttributeValue<bool>("adlr_fact");

            try
            {
                if (fact)
                {
                    invoiceService.RecalculateAmountPaid(targetInvoice);
                    traceService.Trace("Изменены поля договора");
                }
            }
            catch (Exception ex)
            {
                traceService.Trace($"Ошибка при создании счета {ex.InnerException}, {ex.StackTrace}, {ex.TargetSite}");
                throw new InvalidPluginExecutionException(ex.Message);
            }

        }
    }
}
