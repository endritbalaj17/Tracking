using System;
using System.ComponentModel.DataAnnotations;

namespace Tracking.Models
{
    public class Home
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Error { get; set; }
    }
}
