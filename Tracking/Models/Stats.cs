using System;
using System.ComponentModel.DataAnnotations;

namespace Tracking.Models
{
    public class Stats
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public decimal Count { get; set; }
    }
}
