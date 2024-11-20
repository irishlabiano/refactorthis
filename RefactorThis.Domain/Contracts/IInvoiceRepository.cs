using RefactorThis.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorThis.Domain
{
    public interface IInvoiceRepository
    {
        Task<Invoice> GetInvoiceById(int id);
        Task<int> SaveInvoice(Invoice invoice);
        void AddInvoice(Invoice invoice);
    }
}
