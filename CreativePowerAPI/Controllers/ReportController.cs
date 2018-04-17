using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CreativePowerAPI.Models;
using CreativePowerAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using ServiceStack;
using ServiceStack.Text;
using File = CreativePowerAPI.Models.File;

namespace CreativePowerAPI.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportSetRepository _reportSetRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IFileRepository _reportPhotoFileRepository;
        public ReportController(IReportSetRepository reportSetRepository, ITaskRepository taskRepository, IHostingEnvironment hostingEnvironment, IFileRepository reportPhotoFileRepository)
        {
            _reportSetRepository = reportSetRepository;
            _taskRepository = taskRepository;
            _hostingEnvironment = hostingEnvironment;
            _reportPhotoFileRepository = reportPhotoFileRepository;
        }

        [HttpGet("api/report/{reportID}/Get")]
        public IActionResult Get(int reportId)
        {
            try
            {
                var rep = _reportSetRepository.GetElementById(reportId);
                if (rep == null)
                {
                    throw new Exception("Report with this ID does not exist.");
                }
                return Ok(JsonConvert.SerializeObject(rep, Formatting.Indented));

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }
        [Authorize("Admin")]
        [HttpDelete("api/report/{reportID}/delete")]
        public IActionResult Delete(int reportId)
        {
            try
            {

                _reportSetRepository.Delete(reportId);
                return Ok(NoContent());
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPost("api/report/{id}/AddPhoto")]
        public IActionResult AddPhoto(int id)
        {
            try
            {
                var report = _reportSetRepository.GetElementById(id);
                var files = HttpContext.Request.Form.Files;
                foreach (var file in files)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var dirPath = Path.Combine(_hostingEnvironment.ContentRootPath, "RaportPhotos");
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }

                    var twoPartFilename = fileName.SplitOnLast('.');

                    var photoFile = new Models.File();

                    _reportPhotoFileRepository.Add(photoFile);

                    var filePath = Path.Combine(dirPath,
                            string.Format("{0}{1}{2}", twoPartFilename[0], photoFile.Id, "." + twoPartFilename[1]));

                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                        var rootPath = Path.Combine("creativepowercrm.azurewebsites.net", "RaportPhotos");
                        photoFile.Url = Path.Combine(rootPath,
                            string.Format("{0}{1}{2}", twoPartFilename[0], photoFile.Id, "." + twoPartFilename[1]));
                        _reportPhotoFileRepository.Update(photoFile);
                        report.Files.Add(photoFile);
                    }
                }

                _reportSetRepository.Update(report);
                return Ok(JsonConvert.SerializeObject(report.Files, Formatting.Indented));


            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        
        [HttpPost("api/report/{id}/Confirm")]
        public IActionResult ConfirmReport(int id)
        {
            try
            {
                var rep = _reportSetRepository.GetElementById(id);
                rep.State = ReportState.Confirmed;
                //task status completed
                var task = _taskRepository.GetElementById(rep.ProjectTaskId);
                task.Status = ProjectTaskStatus.Completed;
                _reportSetRepository.Update(rep);
                _taskRepository.Update(task);
                return Ok();

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }
        [HttpPost("api/report/{id}/Reject")]
        public IActionResult RejectReport(int id)
        {
            try
            {
                var rep = _reportSetRepository.GetElementById(id);
                
                rep.State = ReportState.Started;
                var task = _taskRepository.GetElementById(rep.ProjectTaskId);
                task.Status = ProjectTaskStatus.Rejected;
                //task status completed
                _reportSetRepository.Update(rep);
                _taskRepository.Update(task);
                return Ok();

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("api/report/GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var reports = _reportSetRepository.All().Where(pr => pr.State != ReportState.Started);
                return Ok(JsonConvert.SerializeObject(reports, Formatting.Indented));
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);
            }
        }

       
    }
}
