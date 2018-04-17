using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreativePowerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using CreativePowerAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace CreativePowerAPI.Controllers
{
    public class ProjectController : Controller
    {

        private readonly IProjectRepository _projectRepository;
        private readonly IPriceListRepository _priceListRepository;
        private ITaskRepository _taskRepository;
        private UserManager<RegisterAccount> _userManager;
        private IRegisterAccountRepository _registerAccountRepository;
        private ISubTaskRepository _subTaskRepository;

        public ProjectController(IProjectRepository projectRepository, IPriceListRepository priceListRepository, UserManager<RegisterAccount> userManager, IRegisterAccountRepository registerAccountRepository, ITaskRepository taskRepository, ISubTaskRepository subTaskRepository)
        {
            _projectRepository = projectRepository;
            _priceListRepository = priceListRepository;
            _userManager = userManager;
            _registerAccountRepository = registerAccountRepository;
            _taskRepository = taskRepository;
            _subTaskRepository = subTaskRepository;
        }



        [HttpGet("api/project/{projectId}/getPriceLists")]
        public IActionResult GetPriceLists(int projectId)
        {
            try
            {
                var project = _projectRepository.GetElementById(projectId);
                if (project == null)
                {
                    throw new Exception("Project with this ID does not exist.");
                }
                var pri = project.PriceList;
                if (pri == null)
                {
                    throw new Exception("Pricelist does not exist.");
                }
                return Ok(JsonConvert.SerializeObject(pri, Formatting.Indented));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        

        [HttpGet("api/project/getMapCircles")]
        public IActionResult GetMapCircles()
        {
            try
            {
                var projects = _projectRepository.All().ToList();
                var mapCircles = new List<MapCircle>();
                ColourGenerator colgen = new ColourGenerator();
                
                for (var index = 0; index < projects.Count; index++)
                {
                    var project = projects[index];
                    if (project.Tasks.ToList().Count == 0)
                    {
                        continue;
                    }
                    var distance = MapCircleHelper.MeasureProjectTasksRadiusDistance(project);
                    if (distance > 0.0)
                    {
                        MapCircle mapCircle = new MapCircle()
                        {
                            InvestorName = project.InvestorName,
                            ProjectId = project.Id,
                            ProjectName = project.Name,
                            InvestorId =  project.InvestorId,
                            Latitude = MapCircleHelper.MeanLatitude(project),
                            Longitude = MapCircleHelper.MeanLongitude(project),
                            Radius = distance,
                            Color = colgen.NextColour()
                           
                        };

                        mapCircles.Add(mapCircle);
                    }
                }

                return Ok(JsonConvert.SerializeObject(mapCircles, Formatting.Indented));

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [Authorize]
        [HttpGet("api/project/{projectId}")]
        public IActionResult GetProject(int projectId)
        {
            try
            {
                var userClaims = HttpContext.User.Claims;
                var user =  _userManager.FindByNameAsync(userClaims.ToList()[0].Value).Result;
                var role =  _userManager.GetRolesAsync(user).Result;
                var pr = _projectRepository.GetElementById(projectId);


                if (pr == null)
                {
                    throw new Exception("Project with this ID does not exist.");
                }
                ProjectWithMapConfig returnPro = new ProjectWithMapConfig() ;
                if (role.Contains("Admin"))
                {
                    returnPro.Tasks = pr.Tasks;
                    
                }
                else
                {
                    returnPro.Tasks = pr.Tasks.Where(t => t.RegisterAccountId == user.Id);
                }
                returnPro.PriceList = pr.PriceList;
                returnPro.CreateDateTime = pr.CreateDateTime;
                returnPro.Id = pr.Id;
                returnPro.InvestorId = pr.InvestorId;
                returnPro.InvestorName = pr.InvestorName;
                returnPro.Name = pr.Name;
                returnPro.MapConfig = MapConfiguration.Calculate(pr);
                return Ok(JsonConvert.SerializeObject(returnPro, Formatting.Indented));
                    //new {project = pr,config = MapConfiguration.Calculate(pr)}, Formatting.Indented));

            }
            catch (Exception e)
            {

                return StatusCode(500, e.Message);
            }
        }

        [Authorize]
        [HttpGet("api/project/{projectId}/GetTasks")]
        public async Task<IActionResult> GetProjectTasks(int projectId)
        {
            try
            {
                var userClaims = HttpContext.User.Claims;
                var user = await _userManager.FindByNameAsync(userClaims.ToList()[0].Value);
                var role = await _userManager.GetRolesAsync(user);
                var pr = _projectRepository.GetElementById(projectId);
                if (pr == null)
                {
                    throw new Exception("Project with this ID does not exist.");
                }
                if (role.Contains("User"))
                {
                    return Ok(JsonConvert.SerializeObject(pr.Tasks.Where(t=> t.RegisterAccountId == user.Id), Formatting.Indented));
                }
                if (role.Contains("Admin"))
                {
                    return Ok(JsonConvert.SerializeObject(pr.Tasks, Formatting.Indented));
                }
                return StatusCode(200, "User does not have suitable role to perform this task.");

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("api/project/{id}/ProjectPricelist")]
        public IActionResult GetInvestorPricelist(int projectId)
        {
            try
            {
                var pr = _projectRepository.GetElementById(projectId);
                if (pr == null)
                {
                    throw new Exception("Investor with this ID does not exist.");
                }

                return Ok(JsonConvert.SerializeObject(pr.PriceList, Formatting.Indented));

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("api/project/{projectId}/AddTask")]
        public IActionResult AddTaskToProject(int projectId, [FromBody]ProjectTask task)
        {
            try

            {
                task.ProjectId = projectId;
                _taskRepository.Add(task);
                
                var pr = _projectRepository.GetElementById(projectId);
                if (pr == null)
                {
                    throw new Exception("Project with this ID does not exist.");
                }
                task.CreateDate = DateTime.Now;
                var addHours = task.CreateDate.Value.AddHours(2);
                task.CreateDate = addHours;
                task.Status = ProjectTaskStatus.Started;
                task.ProjectName = pr.Name;
                task.InvestorName = pr.InvestorName;
                if (task.DeadlineDate == null)
                {
                    task.DeadlineDate = null;
                }
                if (task.RegisterAccountId == null)
                {
                    pr.Tasks.ToList().Add(task);  
                    _projectRepository.Update(pr);
                    return Ok(JsonConvert.SerializeObject(task, Formatting.Indented));
                }
                task.isTaskAccepted = "IsNotAccepted";
                var reg =  _registerAccountRepository.GetElementById(task.RegisterAccountId);
                task.RegisterAccountName = reg.CompanyName;
                reg.TaskList.Add(task);
                pr.Tasks.ToList().Add(task);
                _registerAccountRepository.Update(reg);
                _projectRepository.Update(pr);
                return Ok(JsonConvert.SerializeObject(task, Formatting.Indented));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [HttpPost("api/project/{projectId}/AddSubTask")]
        public IActionResult AddTaskToProject(int projectId, [FromBody]SubTask SubTask)
        {
            try

            {
                SubTask.ProjectId = projectId;
                SubTask.EndTask.ProjectId = projectId;
                _subTaskRepository.Add(SubTask);

                var pr = _projectRepository.GetElementById(projectId);
                if (pr == null)
                {
                    throw new Exception("Project with this ID does not exist.");
                }
                SubTask.CreateDate = DateTime.Now;
                var addHours = SubTask.CreateDate.Value.AddHours(2);
                SubTask.CreateDate = addHours;
                SubTask.Status = ProjectTaskStatus.Started;
                SubTask.ProjectName = pr.Name;
                SubTask.InvestorName = pr.InvestorName;
                if (SubTask.DeadlineDate == null)
                {
                    SubTask.DeadlineDate = null;
                }
                if (SubTask.RegisterAccountId == null)
                {
                    pr.Tasks.ToList().Add(SubTask);
                    _projectRepository.Update(pr);
                    return Ok(JsonConvert.SerializeObject(SubTask, Formatting.Indented));
                }

                var reg = _registerAccountRepository.GetElementById(SubTask.RegisterAccountId);
                SubTask.RegisterAccountName = reg.CompanyName;
                reg.TaskList.Add(SubTask);
                pr.Tasks.ToList().Add(SubTask);
                _registerAccountRepository.Update(reg);
                _projectRepository.Update(pr);
                return Ok(JsonConvert.SerializeObject(SubTask, Formatting.Indented));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }



        [HttpGet("api/project/all")]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(JsonConvert.SerializeObject(_projectRepository.All(), Formatting.Indented));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [Authorize("Admin")]
        [HttpDelete("api/project/{id}/delete")]
        public IActionResult DeleteProject(int id)
        {
            try
            {
                var pr = _projectRepository.GetElementById(id);
                if (pr == null)
                {
                    throw new Exception("Project with this ID does not exist.");
                }
                else
                {
                    _projectRepository.Delete(id);
                    return Ok();
                }

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("api/project/{id}/addPriceList/{priceListId}")]
        public IActionResult AddPriceList(int id, int priceListId)
        {
            try
            {
                var pr = _projectRepository.GetElementById(id);
                if (pr == null)
                {
                    throw new Exception("Project with this ID does not exist.");
                }
                var priceList = _priceListRepository.GetElementById(priceListId);
                if (priceList == null)
                {
                    throw new Exception("PriceList with this ID does not exist.");
                }
                else
                {
                    pr.PriceList = priceList;
                    _projectRepository.Update(pr);
                    return Ok(JsonConvert.SerializeObject(pr, Formatting.Indented));
                }

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }

    
}
