using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CreativePowerAPI.Models
{
    public class RegisterAccount : IdentityUser
    {
        public string Role { get; set; }
        public string CompanyName { get; set; }
        public string NIP { get; set; }
        public LoginUser Credentials { get; set; }
        public List<ContactPerson> Contacts { get; set; }
        public string CompanyPhoneNumber { get; set; }
        public List<Notification> Notifications { get; set; }
        public string CompanyPostalCode { get; set; }
        public string CompanyAddress { get; set; }
        public List<ATask> TaskList { get; set; }
        public float Discount { get; set; }
    }
}
