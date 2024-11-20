using RefactorThis.Domain.Constants;
using RefactorThis.Domain.Models;
using System.Linq;

namespace RefactorThis.Domain.Factories
{
    public class StandardInvoiceFactory : InvoiceTypeFactory
    {
        public override string CalculatePayment(Invoice invoice, Payment payment)
        {
            AddPayment(invoice, payment);

            if (payment.Amount == invoice.Amount)
            {
                return Message.INVOICE_FULLY_PAID;
            }

            else if (invoice.Payments.Any() && invoice.Amount > (invoice.AmountPaid + payment.Amount))
            {
                return Message.PAYMENT_RECEIVED_NOT_FULLY_PAID;
            }
            else
            {
                return Message.INVOICE_PARTIALY_PAID;
            }
        }
    }
}
