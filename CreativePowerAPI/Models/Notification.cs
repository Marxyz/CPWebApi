using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreativePowerAPI.Models
{
    public class Notification
    {
        public DateTime CreateDateTime { get; set; }
        public int Id { get; set; }
        public string Content { get; set; }
    }
}
