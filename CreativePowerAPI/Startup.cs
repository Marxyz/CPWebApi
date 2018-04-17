using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using CreativePowerAPI.Models;
using CreativePowerAPI.Repositories.Interfaces;
using CreativePowerAPI.Repositories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Swashbuckle.AspNetCore.Swagger;

namespace CreativePowerAPI
{
    public partial class Startup
    {
        


        private static string _secret = null;
        public Startup(IHostingEnvironment env)
        {


            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)

                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables().AddUserSecrets<Startup>();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {

            _secret = Configuration["SecretKey"];
            services.AddDbContext<DBC>(o => o.UseSqlServer(Configuration.GetConnectionString("AzureConnection")));
            services.AddIdentity<RegisterAccount, IdentityRole>().AddEntityFrameworkStores<DBC>();
            services.AddCors(options =>
            {
                options.AddPolicy("Allow",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());
                
            });
            services.Configure<IdentityOptions>(options =>
            {
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("User", policy => policy.RequireClaim("Role", "User"));
                options.AddPolicy("Admin", policy => policy.RequireClaim("Role", "Admin"));
            });
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "CreativePowerCRM Web Api",
                    Version = "v1",
                    Description = "A simple Web Api for my team members.",
                    TermsOfService = "None"

                });
            });

            services.AddDirectoryBrowser();
            services.AddTransient<IInvestorRepository, InvestorRepository>();
            services.AddTransient<IProjectRepository, ProjectRepository>();
            services.AddTransient<IRegisterAccountRepository, RegisterAccountRepository>();
            services.AddTransient<ITaskRepository, TaskRepository>();
            services.AddTransient<IPriceListRepository, PriceListRepository>();
            services.AddTransient<IReportSetRepository, ReportSetRepository>();
            services.AddTransient<IFileRepository, FileRepository>();
            services.AddTransient<ISubTaskRepository, SubTaskRepository>();
            services.AddTransient<IMarginalTaskRepository, MarginalTaskRepository>();
            services.AddTransient<IAbstractTaskRepository, AbstractTaskRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors("Allow");
            ConfigureAuth(app);
            app.UseIdentity();
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CreativePowerCRM API v1");
            });
            app.UseStaticFiles();
            app.UseStaticFiles
            (
                new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory())),
                    RequestPath = new PathString(string.Empty)
                }
            );

            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory())),
                RequestPath = new PathString("/Browser")
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                name: "default",
                template: "{controller}/{action}/{id?}",
                defaults: new { controller = "Investor", action = "GetAllInvestors" });
            });


        }
    }
}

