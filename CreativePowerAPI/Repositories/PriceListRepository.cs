using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreativePowerAPI.Models;
using CreativePowerAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CreativePowerAPI.Repositories
{
    public class PriceListRepository:BaseRepository<PriceList>,IPriceListRepository
    {
        public PriceListRepository(DBC databaseCtx) : base(databaseCtx)
        {
        }

        public void Delete(int element_id)
        {
            var pl = GetElementById(element_id);
            _databaseCtx.PriceLists.Remove(pl);
            _databaseCtx.SaveChanges();
        }

        public PriceList GetElementById(int id)
        {
            return All().FirstOrDefault(p => p.Id == id);
        }

        public void Update(PriceList element)
        {
            _databaseCtx.PriceLists.Update(element);
            _databaseCtx.SaveChanges();
        }

        public IEnumerable<PriceList> All()
        {
            return _databaseCtx.PriceLists.Include(ent => ent.Categories).ThenInclude(ent=>ent.ListofPriceListElements).ToList();
        }

        public void Add(PriceList element)
        {

            _databaseCtx.Add(element);
            _databaseCtx.SaveChanges();
        }
    }
}
