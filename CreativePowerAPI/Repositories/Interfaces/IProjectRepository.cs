using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreativePowerAPI.Models;

namespace CreativePowerAPI.Repositories.Interfaces
{
    public interface IProjectRepository : IGenericHttpOperations<Project>
    {
       IEnumerable<Project> All();
    }
}
