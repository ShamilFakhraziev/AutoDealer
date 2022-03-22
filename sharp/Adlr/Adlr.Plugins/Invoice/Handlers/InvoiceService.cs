using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adlr.Plugins.Invoice.Handlers
{
    public sealed class InvoiceService
    {
        private IOrganizationService service;
        public const int Manually = 222950000;
        public const int Automatically = 222950001;
        public InvoiceService(IOrganizationService service)
        {
            this.service = service;
        }

       
        public void SetInvoiceType(Entity targetInvoice)
        {
            if(targetInvoice == null) throw new ArgumentNullException("Target invoice was null");
            var type = targetInvoice.GetAttributeValue<OptionSetValue>("adlr_type");

            if (type == null)
            {
                targetInvoice["adlr_type"] = new OptionSetValue(Manually);
            }
        }
        public void RecalculateAmountPaid(Entity targetInvoice)
        {
            if (targetInvoice == null) throw new ArgumentNullException("Target invoice was null");

            //получаю ссылку на договор
            var agreementRef = targetInvoice.GetAttributeValue<EntityReference>("adlr_dogovorid");

            if (agreementRef != null)
            {
                //получаю обьект договора
                var agreement = service.Retrieve(agreementRef.LogicalName, agreementRef.Id, 
                    new ColumnSet("adlr_factsumma","adlr_fact","adlr_summa"));


                //получаю оплаченную сумму договора и сумму оплаты
                var invoiceAmount = targetInvoice.GetAttributeValue<Money>("adlr_amount") == null ? 0 : targetInvoice.GetAttributeValue<Money>("adlr_amount").Value;
                var agrFactSumma = agreement.GetAttributeValue<Money>("adlr_factsumma") == null ? 0 : agreement.GetAttributeValue<Money>("adlr_factsumma").Value;

                
                //получаю полную сумму счета для дальнейшего сравнения
                agrFactSumma += invoiceAmount;

                var agreementSumma = agreement.GetAttributeValue<Money>("adlr_summa") == null ? 0 : agreement.GetAttributeValue<Money>("adlr_summa").Value;
                //сравниваю, если оплаченная сумма договора больше просто суммы то выбрасываю ошибку,
                //если равна то изменяю оплаченную сумму и меняю значение поля факт оплаты
                //если меньше то просто меняю оплаченную сумму 
                if (agrFactSumma > agreementSumma)
                {
                    throw new Exception("Сумма оплаченных счетов превышает сумму договора");

                }
                else if (agrFactSumma == agreementSumma)
                {
                    agreement["adlr_factsumma"] = new Money(agrFactSumma);
                    agreement["adlr_fact"] = true;
                }
                else
                {
                    agreement["adlr_factsumma"] = new Money(agrFactSumma);
                }

                targetInvoice["adlr_paydate"] = DateTime.Now;

                //сохраняю изменения для договора
                service.Update(agreement);


            }
        }
    }
}
