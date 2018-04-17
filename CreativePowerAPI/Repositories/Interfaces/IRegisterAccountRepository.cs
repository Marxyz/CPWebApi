using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreativePowerAPI.Models;

namespace CreativePowerAPI.Repositories.Interfaces
{
    public interface IRegisterAccountRepository
    {
        IEnumerable<RegisterAccount> All();
        RegisterAccount GetElementById(string id);
        void Delete(string id);
        void Update(RegisterAccount element);
    }
}
