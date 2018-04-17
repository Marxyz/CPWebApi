using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreativePowerAPI.Models
{
    public class PriceList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Category> Categories{ get; set; }
    }

    public class Category
    {
        public string Name{ get; set; }
        public int Id { get; set; }
        public List<PriceListElement> ListofPriceListElements { get; set; }
        public double Sum { get; set; }
    }

    public class PriceListElement
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public float Price { get; set; }
        public float Quantity { get; set; }
        public string Description { get; set; }
    }

        
}
 