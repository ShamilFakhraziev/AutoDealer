using Adlr.Plugins.Communication.Handlers;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adlr.Plugins.Communication
{
    public class PreCommunicationCreate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var traceService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            var pluginContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var targetCommunication = (Entity)pluginContext.InputParameters["Target"];

            var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = serviceFactory.CreateOrganizationService(Guid.Empty);

            CommunicationService communicationService = new CommunicationService(service);
            
            try
            {
                var type = targetCommunication.GetAttributeValue<OptionSetValue>("adlr_type");
                var isMain = targetCommunication.GetAttributeValue<bool>("adlr_main");
                var contactRef = targetCommunication.GetAttributeValue<EntityReference>("adlr_contactid");
                var isNotСontained = false;
                if (type.Value == CommunicationService.Email && isMain)
                {
                    isNotСontained = communicationService.IsContainsCommunicationsWithSpecifiedType(contactRef, CommunicationService.Email);
                    if (!isNotСontained) throw new Exception("Средство связи для контакта с Типом = Email и" +
                        " полем Основной = Да уже существует!");
                }
                else if(type.Value == CommunicationService.Phone && isMain)
                {
                    isNotСontained = communicationService.IsContainsCommunicationsWithSpecifiedType(contactRef, CommunicationService.Phone);
                    if (!isNotСontained) throw new Exception("Средство связи для контакта с Типом = Телефон и" +
                        " полем Основной = Да уже существует!");
                }
            }
            catch(Exception ex)
            {
                traceService.Trace($"Возникла ошибка {ex.InnerException}, {ex.StackTrace}, {ex.TargetSite}");
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
    }
}
