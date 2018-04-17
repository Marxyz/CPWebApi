using CreativePowerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace CreativePowerAPI.Repositories.Interfaces
{
    public interface IInvestorRepository : IGenericHttpOperations<Investor>
    {
        IEnumerable<Investor> All();
        IEnumerable<Investor> QuickInvestorsAndProjects();
    }
}
