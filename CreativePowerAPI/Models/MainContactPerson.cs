using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreativePowerAPI.Models
{
    public class MainContactPerson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ContactPerson ContactPerson { get; set; }
        public List<ContactPerson> ListOfContacts { get; set; }

    }
}
