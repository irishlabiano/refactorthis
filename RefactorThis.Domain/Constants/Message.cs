using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorThis.Domain.Constants
{
    public static class Message
    {
        public static string NO_INVOICE_FOUND_FOR_PAYMENT = "There is no invoice matching this payment";
        public static string NO_PAYMENT_NEEDED = "no payment needed";
        public static string INVOICE_ALREADY_FULLY_PAID = "invoice was already fully paid";
        public static string PAYMENT_GREATER_AMOUNT_REMAINING = "the payment is greater than the partial amount remaining";
        public static string PAYMENT_GREATER_INVOICE_AMOUNT = "the payment is greater than the invoice amount";
        public static string INVOICE_FULLY_PAID = "invoice is now fully paid";
        public static string INVOICE_PARTIALY_PAID = "invoice is now partially paid";
        public static string FINAL_PAYMENT_INVOICE_PAID = "final partial payment received, invoice is now fully paid";
        public static string PAYMENT_RECEIVED_NOT_FULLY_PAID = "another partial payment received, still not fully paid";
    }
    

}
