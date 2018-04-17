using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreativePowerAPI.Models
{
    public class Polyline
    {
        public List<TaskPosition> ListOfPositions{ get; set; }
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Color { get; set; }
    }


}
