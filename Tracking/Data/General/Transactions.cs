using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tracking.Data.General
{
    public class Transactions
    {
        [Key]
        public int TransactionID { get; set; }
        public int SenderID { get; set; }
        public int ReceiverID { get; set; }
        public decimal Price { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
