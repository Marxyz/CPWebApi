using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreativePowerAPI.Models;
using CreativePowerAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CreativePowerAPI.Repositories
{
    public class RegisterAccountRepository : BaseRepository<RegisterAccount>,IRegisterAccountRepository
    {
        public RegisterAccountRepository(DBC databaseCtx) : base(databaseCtx)
        {
        }

        public IEnumerable<RegisterAccount> All()
        {
            return
                _databaseCtx.RegisterAccounts.Include(ent => ent.Contacts)
                    .Include(ent => ent.Notifications)
                    .Include(ent => ent.TaskList).ToList();
        }

        public RegisterAccount GetElementById(string id)
        {
            return All().FirstOrDefault(ent => ent.Id == id);
        }

        public void Delete(string id)
        {
            var us = All().FirstOrDefault(ent => ent.Id == id);
            _databaseCtx.RegisterAccounts.Remove(us);
            _databaseCtx.SaveChanges();
        }

        public void Update(RegisterAccount element)
        {
            _databaseCtx.RegisterAccounts.Update(element);
            _databaseCtx.SaveChanges();
        }
    }
}
