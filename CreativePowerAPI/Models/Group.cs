using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreativePowerAPI.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Project> Projects { get; set; } = new List<Project>();
        public List<Discount> Discounts { get; set; } = new List<Discount>();
        public List<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();


    }
}
