using RefactorThis.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorThis.Domain.Factories
{
    public abstract class InvoiceTypeFactory
    {
        public abstract string CalculatePayment(Invoice invoice, Payment payment);
        protected void AddPayment(Invoice invoice, Payment payment)
        {
            invoice.AmountPaid += payment.Amount;
            invoice.Payments.Add(payment);
        }

        protected void AddTax(Invoice invoice, decimal paymentAmount, decimal taxRate)
        {
            invoice.TaxAmount += paymentAmount * taxRate;
        }
    }
}
