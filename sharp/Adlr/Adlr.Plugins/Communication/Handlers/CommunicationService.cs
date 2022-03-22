using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adlr.Plugins.Communication.Handlers
{
    public class CommunicationService
    {
        private IOrganizationService service;

        public const int Phone = 222950001;
        public const int Email = 222950002;
        public CommunicationService(IOrganizationService service)
        {
            this.service = service;
            
        }
        public bool IsContainsCommunicationsWithSpecifiedType(EntityReference contactRef, int communicationTypeValue)
        {
            if (contactRef == null) throw new ArgumentNullException("Entity Reference to contact was null");
            QueryExpression query = new QueryExpression()
            {
                EntityName = "adlr_communication",
                ColumnSet = new ColumnSet(true),
                LinkEntities =
                {
                    new LinkEntity
                    {
                        JoinOperator = JoinOperator.Inner,
                        LinkFromAttributeName = "adlr_contactid",
                        LinkFromEntityName = "adlr_communication",
                        LinkToAttributeName = "contactid",
                        LinkToEntityName = contactRef.LogicalName,
                        LinkCriteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression("contactid", ConditionOperator.Equal, contactRef.Id)
                            }
                        }
                    }
                },
                Criteria =
                {
                    Filters =
                    {
                        new FilterExpression
                        {
                            FilterOperator = LogicalOperator.And,
                            Conditions =
                            {
                                new ConditionExpression("adlr_type",ConditionOperator.Equal, communicationTypeValue),
                                new ConditionExpression("adlr_main",ConditionOperator.Equal, true)
                            }
                        }
                    }
                }
            };
            var isHaving = service.RetrieveMultiple(query).Entities.Count.Equals(0);
            return isHaving;
        }
       

    }
}
