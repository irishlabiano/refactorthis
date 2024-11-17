using RefactorThis.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorThis.Domain.Factories
{
    public class FactoryHandler
    {
        public static InvoiceTypeFactory CreateHandler(InvoiceType invoiceType)
        {
            switch (invoiceType)
            {
                case InvoiceType.Standard:
                    return new StandardInvoiceFactory();
                case InvoiceType.Commercial:
                    return new CommercialInvoiceFactory();
                default:
                    throw new ArgumentOutOfRangeException(nameof(invoiceType), "Unsupported invoice type");
            }
        }
    }
}
