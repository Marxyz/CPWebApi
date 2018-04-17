using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreativePowerAPI.Models;
using CreativePowerAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CreativePowerAPI.Repositories
{
   public class ReportSetRepository : BaseRepository<Report>,IReportSetRepository
    {
        public ReportSetRepository(DBC databaseCtx) : base(databaseCtx)
        {
        }

        

        public void Delete(int element_id)
        {
            var rep = GetElementById(element_id);
            _databaseCtx.Remove(rep);
            _databaseCtx.SaveChanges();
        }

        public Report GetElementById(int id)
        {
            return All().FirstOrDefault(rep => rep.Id == id);
        }

        public void Update(Report element)
        {
            _databaseCtx.Update(element);
            _databaseCtx.SaveChanges();
        }

        public IEnumerable<Report> All()
        {
            return _databaseCtx.ReportSet
                .Include(ent => ent.Files)
                .Include(ent=>ent.PriceList).ThenInclude(ent=>ent.Categories).ThenInclude(ent=>ent.ListofPriceListElements)
                .Include(ent=> ent.Files);
        }

        public void Add(Report element)
        {

            _databaseCtx.Add(element);
            _databaseCtx.SaveChanges();
        }
    }
}
