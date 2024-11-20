using RefactorThis.Domain.Enums;
using RefactorThis.Domain.Models;
using RefactorThis.Domain;
using RefactorThis.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Castle.Core.Logging;
using RefactorThis.Domain.Constants;

namespace DomainTest
{
    [TestFixture]
    public class InvoiceServiceTests
    {
        private Mock<IInvoiceRepository> _mockRepository;
        private InvoiceService _service;

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new Mock<IInvoiceRepository>();
            _service = new InvoiceService(_mockRepository.Object);
        }

        [Test]
        public void ProcessPayment_Should_ThrowException_When_NoInoiceFoundForPaymentReference()
        {
            var payment = new Payment { InvoiceId = 1 };

            _mockRepository.Setup(repo => repo.GetInvoiceById(payment.InvoiceId)).ReturnsAsync((Invoice)null);

            var result = Assert.ThrowsAsync<InvalidOperationException>(() => _service.ProcessPayment(payment));


            Assert.That(result.Message, Is.EqualTo(Message.NO_INVOICE_FOUND_FOR_PAYMENT));
        }
        [Test]
        public void ProcessPayment_Should_ReturnFailureMessage_When_NoPaymentNeeded()
        {
            var payment = new Payment();
            var invoice = new Invoice
            {
                Id = 1,
                Amount = 0,
                AmountPaid = 50,
                Type = InvoiceType.Standard,
            };
            _mockRepository.Setup(repo => repo.AddInvoice(invoice));

            _mockRepository.Setup(repo => repo.GetInvoiceById(payment.InvoiceId)).ReturnsAsync(invoice);

            var result = _service.ProcessPayment(payment).Result;

            Assert.That(result, Is.EqualTo(Message.NO_PAYMENT_NEEDED));
        }
        [Test]
        public void ProcessPayment_Should_ReturnFailureMessage_When_PartialPaymentExistsAndAmountPaidExceedsAmountDue()
        {
            var payment = new Payment { Amount = 100, InvoiceId = 1 };

            var invoice = new Invoice
            {
                Id = 1,
                Amount = 100,
                AmountPaid = 50,
                Payments = new List<Payment> { payment },
                Type = InvoiceType.Standard
            };
            _mockRepository.Setup(repo => repo.AddInvoice(invoice));

            _mockRepository.Setup(repo => repo.GetInvoiceById(payment.InvoiceId)).ReturnsAsync(invoice);

            var result = _service.ProcessPayment(payment).Result;

            Assert.That(result, Is.EqualTo(Message.PAYMENT_GREATER_AMOUNT_REMAINING));
        }

        [Test]
        public void ProcessPayment_Should_ReturnFailureMessage_When_NoPartialPaymentExistsAndAmountPaidExceedsInvoiceAmount()
        {
            var payment = new Payment { Amount = 200, InvoiceId = 1 };

            var invoice = new Invoice
            {
                Id = 1,
                Amount = 100,
                AmountPaid = 0,
                Payments = new List<Payment> { payment },
                Type = InvoiceType.Standard
            };
            _mockRepository.Setup(repo => repo.AddInvoice(invoice));

            _mockRepository.Setup(repo => repo.GetInvoiceById(payment.InvoiceId)).ReturnsAsync(invoice);

            var result = _service.ProcessPayment(payment).Result;

            Assert.That(result, Is.EqualTo(Message.PAYMENT_GREATER_INVOICE_AMOUNT));
        }


        [Test]
        public void ProcessPayment_Should_ReturnFailureMessage_When_InvoiceAlreadyFullyPaid()
        {
            var payment = new Payment { Amount = 50, InvoiceId = 1 };

            var invoice = new Invoice
            {
                Id = 1,
                Amount = 100,
                AmountPaid = 100,
                Payments = new List<Payment> { payment },
                Type = InvoiceType.Standard
            };
            _mockRepository.Setup(repo => repo.AddInvoice(invoice));

            _mockRepository.Setup(repo => repo.GetInvoiceById(payment.InvoiceId)).ReturnsAsync(invoice);

            var result = _service.ProcessPayment(payment).Result;

            Assert.That(result, Is.EqualTo(Message.INVOICE_ALREADY_FULLY_PAID));
        }


        [Test]
        public void ProcessPayment_WhenInvoiceIsPartiallyPaid()
        {
            var payment = new Payment { Amount = 50, InvoiceId = 1 };

            var invoice = new Invoice
            {
                Id = 1,
                Amount = 100,
                AmountPaid = 50,
                Payments = new List<Payment> { payment },
                Type = InvoiceType.Standard
            };
            _mockRepository.Setup(repo => repo.AddInvoice(invoice));

            _mockRepository.Setup(repo => repo.GetInvoiceById(payment.InvoiceId)).ReturnsAsync(invoice);

            var result = _service.ProcessPayment(payment).Result;

            Assert.That(result, Is.EqualTo(Message.INVOICE_PARTIALY_PAID));
            
        }
    }
}
