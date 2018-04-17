using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreativePowerAPI.Models;
using CreativePowerAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CreativePowerAPI.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(DBC databaseCtx) : base(databaseCtx)
        {
        }

        public void Delete(int element_id)
        {
            var pr = _databaseCtx.Categories.FirstOrDefault(ent => ent.Id == element_id);
            _databaseCtx.Remove(pr);
            _databaseCtx.SaveChanges();
        }

        public Category GetElementById(int id)
        {
            return All().FirstOrDefault(cat => cat.Id == id);
        }

        public void Update(Category element)
        {
            _databaseCtx.Categories.Update(element);
            _databaseCtx.SaveChanges();
        }

        public IEnumerable<Category> All()
        {
            return _databaseCtx.Categories.Include(ent => ent.ListofPriceListElements);
        }

        public void Add(Category element)
        {
            _databaseCtx.Categories.Add(element);
            _databaseCtx.SaveChanges();
        }
    }
}
