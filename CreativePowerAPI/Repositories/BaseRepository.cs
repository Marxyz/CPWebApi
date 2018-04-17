using CreativePowerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CreativePowerAPI.Repositories
{
    public abstract class BaseRepository<T> where T:class
    {
        protected DBC _databaseCtx;
        public BaseRepository(DBC databaseCtx)
        {
            _databaseCtx = databaseCtx;
        }

        
    }
}
