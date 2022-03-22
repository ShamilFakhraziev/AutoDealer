using Adlr.Plugins.Agreement.Handlers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adlr.Plugins.Agreement
{
    public sealed class PreAgreementCreate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var traceService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            var pluginContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var targetAgreement = (Entity)pluginContext.InputParameters["Target"];

            var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = serviceFactory.CreateOrganizationService(Guid.Empty);

            try
            {
                AgreementService agreementService = new AgreementService(service);
                agreementService.ChangeContactDate(targetAgreement);
                traceService.Trace("Contact date was changed and agreement was created");
            }
            catch(Exception ex)
            {
                traceService.Trace($"Возникла ошибка {ex.InnerException}, {ex.StackTrace}, {ex.TargetSite}");
                throw new InvalidPluginExecutionException(ex.Message);
            }

        }
    }
}
