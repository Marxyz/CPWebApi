using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreativePowerAPI.Models
{
    public class Discount
    {
            public int Id { get; set; }
            public Investor Investor { get; set; }
            public double Value { get; set; }

    }
}
