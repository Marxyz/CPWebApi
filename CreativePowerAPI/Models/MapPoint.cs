using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreativePowerAPI.Models
{
    public class MapPoint
    {
        public string Name { get; set; }
        public TaskPosition Position { get; set; }  
        public MapPointType Type { get; set; }
        public string Color { get; set; }
    }
}
