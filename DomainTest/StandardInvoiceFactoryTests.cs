using RefactorThis.Domain.Constants;
using RefactorThis.Domain.Factories;
using RefactorThis.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainTest
{
    [TestFixture]
    public class StandardInvoiceFactoryTests
    {
        [Test]
        public void HandlePayment_WhenPaymentEqualsAmount()
        {
            var invoice = new Invoice { Amount = 100, AmountPaid = 0, Payments = new List<Payment>() };
            var payment = new Payment { Amount = 100 };

            var handler = new StandardInvoiceFactory();
            var message = handler.CalculatePayment(invoice, payment);

            Assert.That(invoice.AmountPaid, Is.EqualTo(100));
            Assert.That(invoice.Payments.Count, Is.EqualTo(1));
            Assert.That(message, Is.EqualTo(Message.INVOICE_FULLY_PAID));
        }

        [Test]
        public void HandlePayment_WhenPaymentIsLessThanAmount()
        {
            var invoice = new Invoice { Amount = 100, AmountPaid = 0, Payments = new List<Payment>() };
            var payment = new Payment { Amount = 50 };

            var handler = new StandardInvoiceFactory();
            var message = handler.CalculatePayment(invoice, payment);

            Assert.That(invoice.AmountPaid, Is.EqualTo(50));
            Assert.That(invoice.Payments.Count, Is.EqualTo(1));
            Assert.That(message, Is.EqualTo(Message.INVOICE_PARTIALY_PAID));
        }

        [Test]
        public void ProcessPayment_Should_ReturnPartiallyPaidMessage_When_PartialPaymentExistsAndAmountPaidIsLessThanAmountDue()
        {
            var invoice = new Invoice { Id = 1, Amount = 100, AmountPaid = 0, Payments = new List<Payment>(), TaxAmount = 0 };
            var payment = new Payment { InvoiceId = 1, Amount = 20 };

            var handler = new CommercialInvoiceFactory();
            var message = handler.CalculatePayment(invoice, payment);

            Assert.That(message, Is.EqualTo(Message.PAYMENT_RECEIVED_NOT_FULLY_PAID));
        }
    }
}
