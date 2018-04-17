using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreativePowerAPI.Models
{
    public class MapCircle
    {
        public int ProjectId { get; set; }
        public int InvestorId { get; set; }
        public string InvestorName { get; set; }
        public string ProjectName { get; set; }
        public double Radius { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Color { get; set; }
    }
}
