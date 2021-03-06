﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreativePowerAPI.Models;

namespace CreativePowerAPI.Repositories.Interfaces
{
    interface ICategoryRepository : IGenericHttpOperations<Category>
    {
        IEnumerable<Category> All();
    }
}
