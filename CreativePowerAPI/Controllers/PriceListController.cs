using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CreativePowerAPI.Models;
using CreativePowerAPI.Repositories;
using CreativePowerAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using OfficeOpenXml;
using ServiceStack;

namespace CreativePowerAPI.Controllers
{
    public class PriceListController:Controller
    {
        private readonly IPriceListRepository _priceListRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private IInvestorRepository _investorRepository;
        public PriceListController(IPriceListRepository priceListRepository, ITaskRepository taskRepository, IHostingEnvironment hostingEnvironment, IInvestorRepository investorRepository)
        {
            _priceListRepository = priceListRepository;
            _taskRepository = taskRepository;
            _hostingEnvironment = hostingEnvironment;
            _investorRepository = investorRepository;
        }

        [HttpGet("api/pricelist/{id}/get")]
        public IActionResult GetPricelist(int id)
        {
            try
            {
                var pr = _priceListRepository.GetElementById(id);
                if (pr == null)
                {
                    throw new Exception("Pricelist with this ID does not exist.");
                }
                
                return Ok(JsonConvert.SerializeObject(pr, Formatting.Indented));
           
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        


        [HttpPost("api/pricelist/{investorID}/addfromexcel")]
        public IActionResult GetExcelPricelist(int investorId)
        {
            PriceList dataToPricelist = new PriceList();
            try
            {
                var files = HttpContext.Request.Form.Files;
                foreach (var file in files)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    
                    var dirPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Temporary");
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }

                    var twoPartFilename = fileName.SplitOnLast('.');

                    var filePath = Path.Combine(dirPath,
                            string.Format("{0}{1}", twoPartFilename[0], "." + twoPartFilename[1]));

                    

                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }
                    FileInfo fileInfo = new FileInfo(filePath);
                    using (ExcelPackage package = new ExcelPackage(fileInfo))
                    {

                        var exc = new ExcelCreator(package)
                        {
                            CurrentWorksheet = package.Workbook.Worksheets.FirstOrDefault()
                        };
                        exc.ToDictionaryData();
                        dataToPricelist = exc.ExcelDataToPricelist();
                    }
                    var inv = _investorRepository.GetElementById(investorId);
                    
                        _priceListRepository.Add(dataToPricelist);
                        inv.PriceLists.Add(dataToPricelist);


                    dataToPricelist.Name = fileName;
                    _investorRepository.Update(inv);

                    fileInfo.Delete();
                }

                return Ok(JsonConvert.SerializeObject(dataToPricelist,Formatting.Indented));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("api/pricelist/addPriceList")]
        public IActionResult AddPriceList([FromBody] PriceList priceList)
        {
            try
            {
                                /*if (priceList.InvestorId == 0 || priceList.ProjectTaskId == 0 )
                {
                    throw new Exception("Pricelist should have InvestorId and ProjectID");
                }*/
                
                _priceListRepository.Add(priceList);
                return Ok(JsonConvert.SerializeObject(priceList,Formatting.Indented));
            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("api/pricelist/all")]
        public IActionResult GetAllPriceLists()
        {
            try
            {
                var pls = _priceListRepository.All();
                return Ok(JsonConvert.SerializeObject(pls, Formatting.Indented));
            }
            catch (Exception e)
            {
                return StatusCode(500,e.Message);
            }
        }

        [Authorize("Admin")]
        [HttpPost("api/pricelist/{id}/update")]
        public IActionResult UpdatePriceList([FromBody] PriceList pricelist, int id)
        {
            try
            {
                var pr = _priceListRepository.GetElementById(pricelist.Id);
                pr.Categories.Clear();
                foreach (var cat in pricelist.Categories)
                {
                        pr.Categories.Add(cat);
                }
                _priceListRepository.Update(pr);
                return Ok(JsonConvert.SerializeObject(pr, Formatting.Indented));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        [Authorize("Admin")]
        [HttpDelete("api/pricelist/{id}/delete")]
        public IActionResult DeletePriceList(int id)
        {
            try
            {
                var pl = _priceListRepository.GetElementById(id);
                if (pl == null)
                {
                    throw new Exception("Pricelist with this ID does not exist.");
                }
                _priceListRepository.Delete(id);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
