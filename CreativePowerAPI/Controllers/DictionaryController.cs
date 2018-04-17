using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreativePowerAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CreativePowerAPI.Controllers
{
    public class DictionaryController : Controller
    {
        private readonly IInvestorRepository _investorRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IRegisterAccountRepository _registerAccountRepository;

        public DictionaryController(IInvestorRepository investorRepository,
            IProjectRepository projectRepository, IRegisterAccountRepository registerAccountRepository)
        {
            _investorRepository = investorRepository;
            _projectRepository = projectRepository;
            _registerAccountRepository = registerAccountRepository;
        }

        [HttpGet]
        [Route("api/dictionary/invsandpricelists")]
        public IActionResult GetInvsAndPricelists()
        {
            try
            {
                var result = _investorRepository.All().Select(i => new
                {
                    Id = i.Id,
                    Name = i.Name,
                    PriceLists = i.PriceLists.Select(pr => new { Id = pr.Id, Name = pr.Name })
                });
                string json = JsonConvert.SerializeObject(result, Formatting.Indented);
                return Ok(json);


            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);

            }
        }


        [HttpGet]
        [Route("api/dictionary/investors")]
        public IActionResult GetInvestorsInfo()
        {
            try
            {

                var tuple = _investorRepository.All().Select(t => new { Name = t.Name, Id = t.Id });
                string json = JsonConvert.SerializeObject(tuple, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { errorInfo = e.Message });
            }

        }
        [HttpGet("api/dictionary/projects")]
        public IActionResult GetProjectsInfo()
        {
            try
            {
                var tuple = _projectRepository.All().Select(t => new { Name = t.Name, Id = t.Id });
                string json = JsonConvert.SerializeObject(tuple, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("api/dictionary/invsandprojects")]
        public IActionResult GetInvsWithProject()
        {
            try
            {
                var res = _investorRepository.All().Select(i => new
                {
                    Name = i.Name,
                    Id = i.Id,
                    Projects = i.Projects.Select
                        (
                            p => new
                            {
                                Name = p.Name,
                                Id = p.Id
                            }
                        )
                }
                );
                return Ok(JsonConvert.SerializeObject(res, Formatting.Indented));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("api/dictionary/users")]
        public IActionResult GetUsersInfo()
        {
            try
            {
                var res = _registerAccountRepository.All().Select(i => new { CompanyName = i.CompanyName, Id = i.Id });
                return Ok(JsonConvert.SerializeObject(res, Formatting.Indented));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("api/dictionary/ProjectsWithTasks")]
        public IActionResult GetProjectsWithTask()
        {
            try
            {
                var res = _projectRepository.All().Select(p => new { Id = p.Id, Name = p.Name,p.PriceList, Tasks = p.Tasks });
                return Ok(JsonConvert.SerializeObject(res, Formatting.Indented));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
            
        }
    }

}
