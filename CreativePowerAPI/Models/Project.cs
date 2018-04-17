using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreativePowerAPI.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ATask> Tasks { get; set; }
        public DateTime CreateDateTime { get; set; }
        public PriceList PriceList { get; set; }
        public int InvestorId { get; set; }
        public string InvestorName { get; set; }
    }
}
