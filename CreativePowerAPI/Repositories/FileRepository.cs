using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreativePowerAPI.Models;
using CreativePowerAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CreativePowerAPI.Repositories
{
    public class FileRepository: BaseRepository<File>, IFileRepository
    {
        public FileRepository(DBC databaseCtx) : base(databaseCtx)
        {
        }

        public void Delete(int element_id)
        {
            File file = GetElementById(element_id);
            _databaseCtx.Files.Remove(file);
        }

        public File GetElementById(int id)
        {
            return All().FirstOrDefault(f => f.Id == id);
        }

        public void Update(File element)
        {
            _databaseCtx.Files.Update(element);
            _databaseCtx.SaveChanges();
        }

        public IEnumerable<File> All()
        {
            return _databaseCtx.Files.ToList();
        }

        public void Add(File element)
        {
            _databaseCtx.Add(element);
            _databaseCtx.SaveChanges();
        }
    }
}
