using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CreativePowerAPI.Models;
using CreativePowerAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using File = System.IO.File;

namespace CreativePowerAPI.Controllers
{
    
    public class ValuesController : Controller
    {
        
        private readonly UserManager<RegisterAccount> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        private IReportSetRepository _reportSetRepository;
        private IPriceListRepository _priceListRepository;

        public ValuesController(DBC databaseContext, UserManager<RegisterAccount> userManager, RoleManager<IdentityRole> roleManager, IHostingEnvironment hostingEnvironment, IReportSetRepository reportSetRepository, IPriceListRepository priceListRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _hostingEnvironment = hostingEnvironment;
            _reportSetRepository = reportSetRepository;
            _priceListRepository = priceListRepository;
        }
        // GET: api/values
        [HttpGet("api/values/excel")]
        public IActionResult Excel()
        {
            try
            {
                string sWebRootFolder = _hostingEnvironment.ContentRootPath;
                string sFileName = @"demo3.xlsx";
                FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                if (file.Exists)
                {
                    file.Delete();
                    file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                }
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    var priceList = _priceListRepository.GetElementById(3);
                    var exc = new ExcelCreator(package);
                    exc.AddSheet($"PriceList{priceList.Name}");
                    exc.AddReportData(1,1,priceList);
                    exc.ToDictionaryData();
                    exc.SaveWork();
                    
                }
                return Ok();
                
            }
                catch (Exception e)
            {
                return StatusCode(500, e.Message + "inner: \n" + e.InnerException.Message);
            }
        }
        [HttpGet("api/values")]
        public IActionResult Get()
        {
            try
            {

                var list = new List<string>();

                ColourGenerator colgen = new ColourGenerator();
                for (int i = 0; i < 800; i++)
                {
                    list.Add(string.Format("{0}: {1}", i, colgen.NextColour()));
                }
                return Ok(JsonConvert.SerializeObject(list, Formatting.Indented));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        // GET api/values/5
        [HttpGet("api/values/user")]
        [Authorize(Policy = "User")]
        public string GetUs()
        {
            return "Hello user";
        }
        [HttpGet("api/values/admin")]
        [Authorize(Policy = "Admin")]
        public string GetA()
        {
            return "Hello Admin";
        }
    }
}
