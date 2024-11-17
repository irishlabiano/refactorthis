using RefactorThis.Domain.Constants;
using RefactorThis.Domain.Factories;
using RefactorThis.Domain.Models;

namespace DomainTest
{
    [TestFixture]
    public class CommercialInvoiceFactoryTests
    {
        [Test]
        public void HandlePayment_WhenPaymentEqualsAmount()
        {
            var invoice = new Invoice { Id = 1, Amount = 100, AmountPaid = 0, Payments = new List<Payment>(), TaxAmount = 0 };
            var payment = new Payment { InvoiceId = 1, Amount = 100 };

            var handler = new CommercialInvoiceFactory();
            var message = handler.CalculatePayment(invoice, payment);

            Assert.That(invoice.AmountPaid, Is.EqualTo(100));
            Assert.That(invoice.TaxAmount, Is.EqualTo(14)); // 14% tax
            Assert.That(invoice.Payments.Count, Is.EqualTo(1));
            Assert.That(message, Is.EqualTo(Message.INVOICE_FULLY_PAID));
        }

        [Test]
        public void HandlePayment_WhenPaymentIsLessThanAmount()
        {
            var invoice = new Invoice { Id = 1, Amount = 100, AmountPaid = 0, Payments = new List<Payment>(), TaxAmount = 0 };
            var payment = new Payment { InvoiceId = 1, Amount = 50 };

            var handler = new CommercialInvoiceFactory();
            var message = handler.CalculatePayment(invoice, payment);

            Assert.That(invoice.AmountPaid, Is.EqualTo(50));
            Assert.That(invoice.TaxAmount, Is.EqualTo(7)); // 14% tax on 50
            Assert.That(invoice.Payments.Count, Is.EqualTo(1));
            Assert.That(message, Is.EqualTo(Message.INVOICE_PARTIALY_PAID));
        }

        [Test]
        public void ProcessPayment_Should_ReturnPartiallyPaidMessage_When_PartialPaymentExistsAndAmountPaidIsLessThanAmountDue()
        {
            var invoice = new Invoice { Id = 1, Amount = 100, AmountPaid = 0, 
                Payments = new List<Payment>{
                    new Payment { Amount = 30, InvoiceId = 1}
                }, 
                TaxAmount = 0 };

            var payment = new Payment { InvoiceId = 1, Amount = 20 };

            var handler = new CommercialInvoiceFactory();
            var message = handler.CalculatePayment(invoice, payment);

            Assert.That(message, Is.EqualTo(Message.PAYMENT_RECEIVED_NOT_FULLY_PAID));
        }
    }
}
