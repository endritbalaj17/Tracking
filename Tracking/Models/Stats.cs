using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Tracking.Models
{
    public class Stats
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public decimal CountSenderMoney { get; set; }
        public decimal CountReceiverMoney { get; set; }
        public decimal Count{ get; set; }
        public List<ListKey> CountList{ get; set; }

    }
    public class ListKey
    {

        public int key { get; set; }
        public decimal count{ get; set; }

    }



}
