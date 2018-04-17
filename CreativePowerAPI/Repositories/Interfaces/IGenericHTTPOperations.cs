using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreativePowerAPI.Repositories.Interfaces
{
    public interface IGenericHttpOperations<T>
    {
        void Add(T element);
        void Delete(int element_id);
        T GetElementById(int id);
        void Update(T element);
    }
}
