using RefactorThis.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorThis.Domain.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal TaxAmount { get; set; }
        public List<Payment> Payments { get; set; }

        public InvoiceType Type { get; set; } = new InvoiceType();
    }
}
