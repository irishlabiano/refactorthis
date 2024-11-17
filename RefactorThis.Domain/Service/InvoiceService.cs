using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RefactorThis.Domain.Constants;
using RefactorThis.Domain.Factories;
using RefactorThis.Domain.Models;
using Enums = RefactorThis.Domain.Enums;

namespace RefactorThis.Domain
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        private string ValidatePayment(Invoice invoice, Payment payment)
        {
            if (invoice.Amount == 0 && (invoice.Payments == null || !invoice.Payments.Any()))
            {
                return Message.NO_PAYMENT_NEEDED;
            }

            if (invoice.AmountPaid == invoice.Amount)
            {
                return Message.INVOICE_ALREADY_FULLY_PAID;
            }

            if (payment.Amount > invoice.Amount)
            {
                return Message.PAYMENT_GREATER_INVOICE_AMOUNT;
            }

            if (invoice.Payments.Sum(x => x.Amount) != 0 && invoice.Amount == invoice.Payments.Sum(x => x.Amount))
            {
                return Message.PAYMENT_GREATER_AMOUNT_REMAINING;
            }

            if ((invoice.Amount + invoice.AmountPaid) == payment.Amount)
            {
                return Message.FINAL_PAYMENT_INVOICE_PAID;
            }

            return string.Empty;
        }

        public async Task<string> ProcessPayment(Payment payment)
        {
            
            var invoice = await _invoiceRepository.GetInvoiceById(payment.InvoiceId);
            if (invoice == null)
            {
                throw new InvalidOperationException(Message.NO_INVOICE_FOUND_FOR_PAYMENT);
            }

            var response = ValidatePayment(invoice, payment);
            if (!string.IsNullOrEmpty(response))
            {
                return response;
            }

            InvoiceTypeFactory factory = null;

            switch (invoice.Type)
            {
                case Enums.InvoiceType.Standard:
                    factory = new StandardInvoiceFactory();
                    break;

                case Enums.InvoiceType.Commercial:
                    factory = new CommercialInvoiceFactory();
                    break;
            }

            var handler = FactoryHandler.CreateHandler(invoice.Type);
           
            var responseMessage = factory.CalculatePayment(invoice, payment);

            var result = await _invoiceRepository.SaveInvoice(invoice);

            return responseMessage;
        }


    }
}