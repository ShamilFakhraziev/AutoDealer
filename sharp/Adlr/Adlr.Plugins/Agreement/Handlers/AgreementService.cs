using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adlr.Plugins.Agreement.Handlers
{
    public sealed class AgreementService
    {
        private readonly IOrganizationService service;
        public AgreementService(IOrganizationService service)
        {
            this.service = service;
        }

        public EntityCollection GetAgreementsContainingTargetContact(EntityReference contactRef)
        {

            if (contactRef == null) throw new ArgumentNullException("Entity Reference to contact was null");
            QueryExpression query = new QueryExpression()
            {
                EntityName = "adlr_agreement",
                ColumnSet = new ColumnSet(true),
                LinkEntities =
                {
                    new LinkEntity
                    {
                        JoinOperator = JoinOperator.Inner,
                        LinkFromAttributeName = "adlr_contact",
                        LinkFromEntityName = "adlr_agreement",
                        LinkToAttributeName = "contactid",
                        LinkToEntityName = contactRef.LogicalName,
                        LinkCriteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression("contactid",ConditionOperator.Equal,contactRef.Id)
                            }
                        }
                    }
                }
            };
            var agreements = service.RetrieveMultiple(query);
            return agreements;
        }
        public void ChangeContactDate(Entity targetAgreement)
        {
            if(targetAgreement == null) throw new ArgumentNullException("Agreement was null");
            var contactReference = targetAgreement.GetAttributeValue<EntityReference>("adlr_contact");
            var agreements = GetAgreementsContainingTargetContact(contactReference);

            if (agreements.Entities.Count == 0)
            {
                var contact = service.Retrieve(contactReference.LogicalName, contactReference.Id, new ColumnSet("adlr_date"));
                contact["adlr_date"] = targetAgreement.GetAttributeValue<DateTime>("adlr_date");
                service.Update(contact);
            }
            
        }

    }
}
