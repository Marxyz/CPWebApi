using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreativePowerAPI.Models
{
    public class Investor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<PriceList> PriceLists { get; set; }
        public ContactPerson ContactPerson { get; set; }
        public List<Project> Projects { get; set; }
    }
}
