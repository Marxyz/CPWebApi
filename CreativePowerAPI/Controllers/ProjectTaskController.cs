using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CreativePowerAPI.Migrations;
using CreativePowerAPI.Models;
using CreativePowerAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using OfficeOpenXml;
using ServiceStack;
using File = CreativePowerAPI.Migrations.File;
using PriceListElement = CreativePowerAPI.Models.PriceListElement;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CreativePowerAPI.Controllers
{
    public class ProjectTaskController : Controller
    {
        private IMarginalTaskRepository _marginalTaskRepository;
        private readonly IInvestorRepository _investorRepository;
        private readonly IPriceListRepository _priceListRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly UserManager<RegisterAccount> _userManager;

        private readonly IReportSetRepository _reportSetRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IFileRepository _fileRepository;
        private readonly IRegisterAccountRepository _registerAccountRepository;

        public ProjectTaskController(ITaskRepository taskRepository, UserManager<RegisterAccount> userManager, IProjectRepository projectRepository, IReportSetRepository reportSetRepository, IPriceListRepository priceListRepository, IHostingEnvironment hostingEnvironment, IInvestorRepository investorRepository, IFileRepository fileRepository, IRegisterAccountRepository registerAccountRepository, IMarginalTaskRepository marginalTaskRepository)
        {
            _taskRepository = taskRepository;
            _userManager = userManager;

            _projectRepository = projectRepository;
            _reportSetRepository = reportSetRepository;
            _priceListRepository = priceListRepository;
            _hostingEnvironment = hostingEnvironment;
            _investorRepository = investorRepository;
            _fileRepository = fileRepository;
            _registerAccountRepository = registerAccountRepository;
            _marginalTaskRepository = marginalTaskRepository;
        }

        [HttpGet("api/task/all")]
        [Authorize]
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                var userClaims = HttpContext.User.Claims;
                var user = await _userManager.FindByNameAsync(userClaims.ToList()[0].Value);
                if (user == null)
                {
                    throw new Exception("User not found.");
                }
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("User"))
                {

                    var tasks = _taskRepository.All();
                    var userTasks = tasks.ToList().FindAll(t => t.RegisterAccountId == user.Id);
                    return Ok(JsonConvert.SerializeObject(userTasks, Formatting.Indented));
                }
                if (roles.Contains("Admin"))
                {
                    var tasks = _taskRepository.All();
                    return Ok(JsonConvert.SerializeObject(tasks, Formatting.Indented));
                }
                return StatusCode(401, "User does not have specific roles.");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("api/task/{id}/AddFiles")]
        public IActionResult AddFiles(int id)
        {
            try
            {
                var task = _taskRepository.GetElementById(id);
                var files = HttpContext.Request.Form.Files;
                var listofPaths = new List<string>();
                foreach (var file in files)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var dirPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Docs");
                   
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }

                    var twoPartFilename = fileName.SplitOnLast('.');

                    var docFile = new Models.File();

                    _fileRepository.Add(docFile);

                    var filePath = Path.Combine(dirPath,
                        string.Format("{0}{1}{2}", twoPartFilename[0], docFile.Id, "." + twoPartFilename[1]));

                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(fs);
                        fs.Flush();

                        var rootPath = Path.Combine("creativepowercrm.azurewebsites.net", "Docs");
                        docFile.Url = Path.Combine(rootPath,
                            string.Format("{0}{1}{2}", twoPartFilename[0], docFile.Id, "." + twoPartFilename[1]));

                        _fileRepository.Update(docFile);

                        task.Files.Add(docFile);

                    }
                    listofPaths.Add(filePath);

                }
                _taskRepository.Update(task);
                return Ok(JsonConvert.SerializeObject(listofPaths, Formatting.Indented));

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [HttpPost("api/task/{id}/AddReport")]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] Report report, int id)
        {
            try
            {
                var claims = HttpContext.User.Claims.ToList()[0].Value;
                var user = await _userManager.FindByNameAsync(claims);

                var task = _taskRepository.GetElementById(id);
                if (task == null)
                {
                    throw new Exception("Task with this ID does not exist.");
                }




                PrepareReport(report, id, task, user);


                task.Status = task.Report.State == ReportState.Sent ? ProjectTaskStatus.WaitingForAcceptation : ProjectTaskStatus.Started;

                string contentRootPath = _hostingEnvironment.ContentRootPath;
                
                var dirPath = System.IO.Path.Combine(contentRootPath, "XLSFiles");
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }


                string sFileName = $"Report_{task.Name.Replace(" ","")}{task.Report.Id}.xlsx";
                FileInfo file = new FileInfo(Path.Combine(dirPath, sFileName));
                if (file.Exists)
                {
                    file.Delete();
                    file = new FileInfo(Path.Combine(dirPath, sFileName));
                }
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    var exc = new ExcelCreator(package);
                    var excelReport = task.Report;
                    exc.AddSheet($"Report{excelReport.ProjectTaskName}");
                    exc.AddReportData(1, 1, excelReport.PriceList);
                    exc.SaveWork();

                }
                var rootPath = Path.Combine("creativepowercrm.azurewebsites.net", "XLSFiles");
                task.Report.ReportCsvUrl = Path.Combine(rootPath, sFileName);
                // Być moze dodawac to do file repo.

                
                _taskRepository.Update(task);

                return Ok(JsonConvert.SerializeObject(task, Formatting.Indented));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        private void PrepareReport(Report report, int id, ProjectTask task, RegisterAccount user)
        {
            PriceList pricelist = null;
            if (report.PriceList.Id == 0)
            {
                var pr = _projectRepository.GetElementById(task.ProjectId);
                var inv = _investorRepository.GetElementById(pr.InvestorId);
                task.Report = new Report
                {
                    Files = new List<Models.File>(),
                    InvestorName = inv.Name,
                    ProjectName = pr.Name,
                    ProjectTaskName = task.Name,
                    Content = report.Content,
                    CreateDate = DateTime.Now
                };


                PriceList prclst = new PriceList();
                prclst.Categories = new List<Category>();
                prclst.Categories.AddRange(report.PriceList.Categories);
                prclst.Name = report.PriceList.Name;

                _priceListRepository.Add(prclst);
            }
            else
            {
                pricelist = _priceListRepository.GetElementById(report.PriceList.Id);
                pricelist.Categories
                    .SelectMany(pr => pr.ListofPriceListElements).ToList()
                    .ForEach(el => el.Quantity = report.PriceList.Categories
                        .SelectMany(category => category.ListofPriceListElements)
                        .FirstOrDefault(ele => ele.Id == el.Id)
                        .Quantity);
                _priceListRepository.Update(pricelist);
            }

            if (task.Report == null)
            {
                task.Report = new Report();
                task.Report.Files = new List<Models.File>();
                task.Report.CompanyName = user.CompanyName;
                task.Report.ProjectTaskName = task.Name;
                task.Report.PriceList = pricelist;
                task.Report.Content = report.Content;
                task.Report.CreateDate = DateTime.Now;
                if (report.Files != null)
                {
                    task.Report.Files.AddRange(report.Files);
                }
                task.Report.BoxNisNumber = report.BoxNisNumber;
                task.Report.ProjectTaskId = id;
                task.Report.State = report.State;
                task.Report.SwitcherNisNumber = report.SwitcherNisNumber;
                var reportTas = _projectRepository.GetElementById(task.ProjectId);
                var reportInv = _investorRepository.GetElementById(reportTas.InvestorId);
                task.Report.InvestorName = reportInv.Name;
                task.Report.ProjectName = reportTas.Name;
                _reportSetRepository.Add(task.Report);
            }
            else
            {
                task.Report.CompanyName = user.CompanyName;
                task.Report.InvestorName =
                    _investorRepository.GetElementById(_projectRepository.GetElementById(task.ProjectId).InvestorId).Name;
                task.Report.ProjectName = _projectRepository.GetElementById(task.ProjectId).Name;
                task.Report.ProjectTaskName = task.Name;
                task.Report.PriceList = pricelist;
                task.Report.Content = report.Content;
                task.Report.CreateDate = DateTime.Now;
                task.Report.BoxNisNumber = report.BoxNisNumber;
                task.Report.ProjectTaskId = id;
                task.Report.State = report.State;
                task.Report.SwitcherNisNumber = report.SwitcherNisNumber;
            }
        }

        [HttpPost("api/task/{id}/addstartingtask")]
        public IActionResult AddStartingTask(int id,[FromBody] MarginalLinePointTask marginalLinePointTask)
        {
            try
            {
                _marginalTaskRepository.Add(marginalLinePointTask);
                var task = _taskRepository.GetElementById(id);
                if (task == null)
                {
                    throw new Exception("Task with this id does not exist.");
                }
                task.StartingLineTask = marginalLinePointTask;
                _taskRepository.Update(task);
                return Ok(JsonConvert.SerializeObject(task, Formatting.Indented));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("api/task/{id}/addendingtask")]
        public IActionResult AddEndingTask(int id, [FromBody] MarginalLinePointTask marginalLinePointTask)
        {
            try
            {
                _marginalTaskRepository.Add(marginalLinePointTask);
                var task = _taskRepository.GetElementById(id);
                if (task == null)
                {
                    throw new Exception("Task with this id does not exist.");
                }
                task.EndingLineTask = marginalLinePointTask;
                _taskRepository.Update(task);
                return Ok(JsonConvert.SerializeObject(task, Formatting.Indented));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [Authorize("Admin")]
        [HttpPut("api/task/update")]
        public IActionResult UpdateTask([FromBody] ProjectTask task)
        {
            try
            {
                _taskRepository.Update(task);
                return Ok(JsonConvert.SerializeObject(task, Formatting.Indented));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        

        [HttpPost("api/task/{id}/ChangeUser")]
        public IActionResult ChangeUser(int id, [FromBody] RegisterAccount user)
        {
            try
            {
                var newId = user.Id;
                var newName = user.CompanyName;

                var task = _taskRepository.GetElementById(id);

                //usuwanie starego taska
                var previousTaskuser = _registerAccountRepository.All().FirstOrDefault(us => us.TaskList.Contains(task));
                if (previousTaskuser != null)
                {
                    previousTaskuser.TaskList.Remove(task);
                    _registerAccountRepository.Update(previousTaskuser);
                }


                //Zmiana
                task.RegisterAccountId = newId;
                task.RegisterAccountName = newName;
                _taskRepository.Update(task);
                var rA = _registerAccountRepository.GetElementById(newId);
                rA.TaskList.Add(task);
                _registerAccountRepository.Update(rA);

                return Ok(JsonConvert.SerializeObject(rA, Formatting.Indented));


            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("api/task/{id}/Reject")]
        public IActionResult RejectTask(int id)
        {
            try
            {
                var task = _taskRepository.GetElementById(id);
                if (task == null)
                {
                    throw new Exception("Task with this ID does not exist.");
                }
                task.RegisterAccountId = null;
                _taskRepository.Update(task);
                return Ok(JsonConvert.SerializeObject(task, Formatting.Indented));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("api/task/{id}/Accept")]
        public IActionResult AcceptTask(int id)
        {
            try
            {
                var task = _taskRepository.GetElementById(id);
                if (task == null)
                {
                    throw new Exception("Task with this ID does not exist.");
                }


                task.isTaskAccepted = "isAccepted";
                _taskRepository.Update(task);


                return Ok(JsonConvert.SerializeObject(task, Formatting.Indented));

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("api/task/ConfirmDate")]
        [Authorize]
        public IActionResult ConfirmDate([FromBody] ProjectTask task)
        {
            try
            {
                var realTask = _taskRepository.GetElementById(task.Id);
                realTask.ConfirmationDate= task.ConfirmationDate;
                _taskRepository.Update(realTask);
                return Ok(JsonConvert.SerializeObject(realTask, Formatting.Indented));

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("api/task/{id}/MarkAsDone")]
        public IActionResult MarkAsDone(int id)
        {
            try
            {
                var task = _taskRepository.GetElementById(id);
                if (task.isReportRequired== "not_required" && task.isTaskAccepted == "isAccepted")
                {
                    task.Status = ProjectTaskStatus.Completed; ;
                    _taskRepository.Update(task);
                }
                return Ok(JsonConvert.SerializeObject(task, Formatting.Indented));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("api/task/{id}")]
        public IActionResult GetTaskWithId(int id)
        {
            try
            {
                var task = _taskRepository.GetElementById(id);
                if (task == null)
                {
                    throw new Exception("Task with this ID does not exist.");
                }
                return Ok(JsonConvert.SerializeObject(task, Formatting.Indented));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [Authorize("Admin")]
        [HttpDelete("api/task/{id}/delete")]
        public IActionResult DeleteTaskWithId(int id)
        {
            try
            {
                var task = _taskRepository.GetElementById(id);
                if (task == null)
                {
                    throw new Exception("Task with this ID does not exist.");
                }
                _taskRepository.Delete(id);
                return Ok(JsonConvert.SerializeObject(task,Formatting.Indented));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

    }
}
