using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreativePowerAPI.Models;
using CreativePowerAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CreativePowerAPI.Repositories
{
    public class TaskRepository : BaseRepository<ProjectTask>, ITaskRepository
    {
        public TaskRepository(DBC databaseCtx) : base(databaseCtx)
        {
        }

        public void Delete(int elementId)
        {
            _databaseCtx.Tasks.Remove(GetElementById(elementId));
            _databaseCtx.SaveChanges();
        }

        public ProjectTask GetElementById(int id)
        {
            return All().FirstOrDefault(pr => pr.Id == id);
        }

        public void Update(ProjectTask element)
        {
            _databaseCtx.Tasks.Update(element);
            _databaseCtx.SaveChanges();
        }

        public IEnumerable<ProjectTask> All()
        {
            return
                _databaseCtx.Tasks
                    .Include(ent => ent.Files)
                    .Include(ent => ent.ListOfTaskPositions)
                    .Include(ent => ent.Report).ThenInclude(ent => ent.Files)
                    .Include(ent => ent.Report).ThenInclude(ent=>ent.PriceList).ThenInclude(ent=> ent.Categories).ThenInclude(ent=>ent.ListofPriceListElements);
        }

        public void Add(ProjectTask element)
        {
            _databaseCtx.AbstractTasks.Add(element);
            _databaseCtx.Tasks.Add(element);
            _databaseCtx.SaveChanges();
        }
    }
}
