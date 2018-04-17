using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace CreativePowerAPI.Models
{

    public class DBC : IdentityDbContext<RegisterAccount>
    {
        public DBC(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
        
        public DbSet<Project> ProjectSet { get; set; }
        public DbSet<Investor> Investors { get; set; }
        public DbSet<RegisterAccount> RegisterAccounts { get; set; }
        public DbSet<ProjectTask> Tasks { get; set; }
        public DbSet<PriceList> PriceLists { get; set; }
        public DbSet<Report> ReportSet { get; set; }
        public DbSet<CreativePowerAPI.Models.File> Files { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubTask> SubTasks { get; set; }
        public DbSet<MarginalLinePointTask> MarginalLinePointTasks { get; set; }
        public DbSet<ATask> AbstractTasks { get; set; }

    }

 
}
    