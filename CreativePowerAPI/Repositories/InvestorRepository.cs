using CreativePowerAPI.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreativePowerAPI.Models;
using CreativePowerAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CreativePowerAPI.Repositories
{
    public class InvestorRepository : BaseRepository<Investor>,IInvestorRepository
    {
        public InvestorRepository(DBC databaseCtx) : base(databaseCtx)
        {
        }

        
        public void Delete(int id)
        {
            var investor = _databaseCtx.Investors.FirstOrDefault(inv => inv.Id == id);
            _databaseCtx.Investors.Remove(investor);
            _databaseCtx.SaveChanges();
        }

        public Investor GetElementById(int id)
        {
            return All().FirstOrDefault(i => i.Id == id);
        }

     

        public void Update(Investor element)
        {
            _databaseCtx.Investors.Update(element);
            _databaseCtx.SaveChanges();
        }


        public IEnumerable<Investor> QuickInvestorsAndProjects()
        {
            return _databaseCtx.Investors.Include(ent => ent.Projects);
        }

        public IEnumerable<Investor> All()
        {

            return _databaseCtx
                .Investors
                .Include(entity => entity.PriceLists).ThenInclude(ent => ent.Categories).ThenInclude(ent=>ent.ListofPriceListElements)
                .Include(ent => ent.Projects).ThenInclude(entity => entity.Tasks).ThenInclude(ent=>ent.ListOfTaskPositions)
                .Include(ent=>ent.Projects).ThenInclude(ent=>ent.Tasks).ThenInclude(ent=>ent.Report)
                .Include(ent => ent.Projects).ThenInclude(ent => ent.Tasks).ThenInclude(ent => ent.Report).ThenInclude(ent => ent.Files)
                .Include(ent => ent.Projects).ThenInclude(ent => ent.Tasks).ThenInclude(ent => ent.Report).ThenInclude(ent=>ent.PriceList).
                ThenInclude(ent=>ent.Categories).ThenInclude(ent=>ent.ListofPriceListElements);
                }

        public void Add(Investor element)
        {

            _databaseCtx.Add(element);
            _databaseCtx.SaveChanges();
        }
    }
}
