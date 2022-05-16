using System;
using System.ComponentModel.DataAnnotations;

namespace Tracking.Models
{
    public class Credit
    {
        public int ide { get; set; }
        public int TransactionID { get; set; }
        [Required]
        public int SenderID { get; set; }
        [Required]
        public int ReceiverID { get; set; }
        public string Reason { get; set; }
        [Required]
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Error { get; set; }
    }
}
