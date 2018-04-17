using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreativePowerAPI.Models;
using CreativePowerAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CreativePowerAPI.Repositories
{
    public class ProjectRepository : BaseRepository<Project>, IProjectRepository
    {
        public ProjectRepository(DBC databaseCtx) : base(databaseCtx)
        {

        }

        public void Delete(int element_id)
        {
            var pr = _databaseCtx.ProjectSet.FirstOrDefault(p => p.Id == element_id);
            _databaseCtx.ProjectSet.Remove(pr);
            _databaseCtx.SaveChanges();
        }

        public Project GetElementById(int id)
        {
            return All().FirstOrDefault(pr => pr.Id == id);
        }

        public void Update(Project element)
        {
            _databaseCtx.ProjectSet.Update(element);
            _databaseCtx.SaveChanges();
        }

        public IEnumerable<ProjectTask> AllTasks()
        {
            List<ProjectTask> result = new List<ProjectTask>();
            foreach (var atask in _databaseCtx.AbstractTasks)
            {
                if (atask is ProjectTask)
                {
                    result.Add(atask as ProjectTask);
                }
            }
            return result;
        }

        public IEnumerable<Project> All()
        {

            return
                _databaseCtx.ProjectSet
                    .Include(ent => ent.PriceList).ThenInclude(ent => ent.Categories).ThenInclude(ent=>ent.ListofPriceListElements)
                    .Include(ent => ent.Tasks).ThenInclude(ent => ent.Report)
                    .Include(ent => ent.Tasks).ThenInclude(ent => ent.ListOfTaskPositions)
                    .Include(ent => ent.Tasks).ThenInclude(ent => ent.Report).ThenInclude(ent => ent.PriceList).ThenInclude(ent => ent.Categories).ThenInclude(ent=>ent.ListofPriceListElements)
                    .Include(ent => ent.Tasks).ThenInclude(ent => ent.Report).ThenInclude(ent => ent.Files).ToList();


            /*.Include(ent=> ent.Tasks).ThenInclude(ent=> ent.Files)
            .Include(ent=>ent.Tasks).ThenInclude(ent=>ent.ListOfTaskPoints)
            .Include(ent=>ent.Tasks).ThenInclude(ent=>ent.Report).ThenInclude(ent=>ent.Files)
            .Include(ent=> ent.Tasks).ThenInclude(ent=>ent.Report).ThenInclude(ent=>ent.PriceList).ThenInclude(ent=>ent.ListofPriceListElements)
            .ToList();*/
        }

        public void Add(Project element)
        {

            _databaseCtx.Add(element);
            _databaseCtx.SaveChanges();
        }
    }
}
