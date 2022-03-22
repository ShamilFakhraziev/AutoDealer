using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;

namespace Adlr.Workflows.AgreementActivities.Handlers
{
    public class AgreementActivitiesService
    {
        private IOrganizationService service;

        public const int Manually = 222950000;
        public const int Automatically = 222950001;
        public AgreementActivitiesService(IOrganizationService service)
        {
            this.service = service;
        }

        public EntityCollection GetInvoicesByLinkedAgreement(Guid agreementId)
        {
            if (agreementId == null) throw new Exception("Agreement Id was null!");
            QueryExpression query = new QueryExpression()
            {
                EntityName = "adlr_invoice",
                ColumnSet = new ColumnSet(true),
                LinkEntities =
                {
                    new LinkEntity
                    {
                        JoinOperator = JoinOperator.Inner,
                        LinkFromAttributeName = "adlr_dogovorid",
                        LinkFromEntityName = "adlr_invoice",
                        LinkToAttributeName = "adlr_agreementid",
                        LinkToEntityName = "adlr_agreement",
                        LinkCriteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression("adlr_agreementid", ConditionOperator.Equal, agreementId)
                            }
                        }
                    }
                }
            };
            var invoices = service.RetrieveMultiple(query);
            return invoices;
        }
        public EntityCollection GetInvoicesByLinkedAgreement(Guid agreementId,int invoiceTypeValue)
        {
            if (agreementId == null) throw new Exception("Agreement Id was null!");
            QueryExpression query = new QueryExpression()
            {
                EntityName = "adlr_invoice",
                ColumnSet = new ColumnSet(true),
                LinkEntities =
                {
                    new LinkEntity
                    {
                        JoinOperator = JoinOperator.Inner,
                        LinkFromAttributeName = "adlr_dogovorid",
                        LinkFromEntityName = "adlr_invoice",
                        LinkToAttributeName = "adlr_agreementid",
                        LinkToEntityName = "adlr_agreement",
                        LinkCriteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression("adlr_agreementid", ConditionOperator.Equal, agreementId)
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
                                new ConditionExpression("adlr_type",ConditionOperator.Equal, invoiceTypeValue)
                            }
                        }
                    }
                }
            };
            var invoices = service.RetrieveMultiple(query);
            return invoices;
        }
        public EntityCollection GetInvoicesByLinkedAgreement(Guid agreementId, bool isFact)
        {
            if (agreementId == null) throw new Exception("Agreement Id was null!");
            QueryExpression query = new QueryExpression()
            {
                EntityName = "adlr_invoice",
                ColumnSet = new ColumnSet(true),
                LinkEntities =
                {
                    new LinkEntity
                    {
                        JoinOperator = JoinOperator.Inner,
                        LinkFromAttributeName = "adlr_dogovorid",
                        LinkFromEntityName = "adlr_invoice",
                        LinkToAttributeName = "adlr_agreementid",
                        LinkToEntityName = "adlr_agreement",
                        LinkCriteria =
                        {
                            Conditions =
                            {
                                new ConditionExpression("adlr_agreementid", ConditionOperator.Equal, agreementId)
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
                                new ConditionExpression("adlr_fact",ConditionOperator.Equal, isFact)
                            }
                        }
                    }
                }
            };
            var invoices = service.RetrieveMultiple(query);
            return invoices;
        }
    }
}
