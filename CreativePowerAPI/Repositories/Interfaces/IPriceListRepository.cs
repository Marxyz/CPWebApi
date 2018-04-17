using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreativePowerAPI.Models;

namespace CreativePowerAPI.Repositories.Interfaces
{
    public interface IPriceListRepository:IGenericHttpOperations<PriceList>
    {
        IEnumerable<PriceList> All();
    }
}
