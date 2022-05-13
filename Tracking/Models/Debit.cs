using System;
using System.ComponentModel.DataAnnotations;

namespace Tracking.Models
{
    public class Debit
    {
        public int ide { get; set; }
        public int TransactionID { get; set; }
        public int SenderID { get; set; }
        public int ReceiverID { get; set; }
        public decimal Price { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Error { get; set; }
    }
}
