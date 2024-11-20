using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorThis.Domain.Models
{
    public class Payment
    {
        public int InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public string Reference { get; set; }
    }
}
