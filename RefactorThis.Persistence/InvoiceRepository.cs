
using RefactorThis.Domain;
using RefactorThis.Domain.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RefactorThis.Persistence {
	public class InvoiceRepository : IInvoiceRepository
    {
        private readonly ICollection<Invoice> _invoices = new HashSet<Invoice>();


        public async Task<Invoice> GetInvoiceById(int id)
		{
            try
            {
                var result = _invoices.FirstOrDefault(invoice => invoice.Id == id);
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

		public async Task<int> SaveInvoice( Invoice invoice )
		{
			return 0;
		}

		public void AddInvoice( Invoice invoice )
		{
            _invoices.Add(invoice);

        }
	}
}