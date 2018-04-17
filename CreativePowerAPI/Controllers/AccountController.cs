using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using CreativePowerAPI.Models;
using CreativePowerAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CreativePowerAPI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<RegisterAccount> _userManager;
        private readonly SignInManager<RegisterAccount> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger _logger;
        private readonly IRegisterAccountRepository _registerAccountRepository;
        private readonly ITaskRepository _taskRepository;


        public AccountController(
            UserManager<RegisterAccount> userManager,
            SignInManager<RegisterAccount> signInManager,
            ILoggerFactory loggerFactory, RoleManager<IdentityRole> roleManager, IRegisterAccountRepository registerAccountRepository, ITaskRepository taskRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _roleManager = roleManager;
            _registerAccountRepository = registerAccountRepository;
            _taskRepository = taskRepository;
        }

        

        [HttpGet]
        [Route("api/account/me/tasks")]
        [Authorize]
        public async Task<IActionResult> GetMyTasks()
        {
            try
            {

                var userClaims = HttpContext.User.Claims;
                var user = await _userManager.FindByNameAsync(userClaims.ToList()[0].Value);
                var roles = await _userManager.GetRolesAsync(user);
                IEnumerable<ProjectTask> tasklist;
                tasklist = roles.Contains("User") ? _taskRepository.All().Where(t => t.RegisterAccountId == user.Id) : _taskRepository.All();
                
                return Ok(JsonConvert.SerializeObject(tasklist, Formatting.Indented));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
                
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/account/register")]
        public async Task<IActionResult> Register([FromBody] RegisterAccount acc)
        {
            try
            {
                acc.UserName = acc.Credentials.Username;
                var result = await _userManager.CreateAsync(acc, acc.Credentials.Password);

                if (result.Succeeded)
                {

                    var Role = new IdentityRole("User");
                    await _roleManager.CreateAsync(Role);
                    await _userManager.AddClaimAsync(acc, new Claim("Role", Role.Name));
                    await _userManager.AddToRoleAsync(acc, Role.Name);



                    _logger.LogInformation(3, "User created a new account with password.");
                    return Ok(acc);
                }
                return BadRequest(result.Errors.Select(p => p.Description).ToList());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("api/account/registerAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAccount acc)
        {
            try
            {
                acc.UserName = acc.Credentials.Username;
                var result = await _userManager.CreateAsync(acc, acc.Credentials.Password);
                if (result.Succeeded)
                {
                    var userRole = new IdentityRole("Admin");
                    await _roleManager.CreateAsync(userRole);
                    await _userManager.AddClaimAsync(acc, new Claim("Role", userRole.Name));
                    await _userManager.AddToRoleAsync(acc, userRole.Name);



                    _logger.LogInformation(3, "User created a new account with password.");
                    return Ok(acc);
                }
                return BadRequest(result.Errors.Select(p => p.Description).ToList());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }

        /*[HttpPost]
        [AllowAnonymous]
        [Route("api/account/loginContr")]
        public async Task<IActionResult> Login(LoginUser loginUser)
        {
            try
            {
                var model = loginUser;
                AuthenticationProperties options = new AuthenticationProperties();
                options.ExpiresUtc = new DateTimeOffset(DateTime.Now.AddMinutes(5));
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, isPersistent: false,
                    lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var t = await Task.FromResult(new ClaimsIdentity(new GenericIdentity(model.Username, "Token"), new Claim[] { }));
                    return Ok(t);
                }

                return BadRequest();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }*/



        [HttpGet("api/account/all")]
        public IActionResult GetAll()
        {
            try
            {

                var users = _registerAccountRepository.All().Select(a => new { a.CompanyName, a.TaskList, a.UserName, a.NIP, Role = _userManager.GetRolesAsync(a).Result, a.Contacts});
                return Ok(JsonConvert.SerializeObject(users, Formatting.Indented));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        

        [Authorize("User")]
        [HttpDelete("api/account/delete")]
        public async Task<IActionResult> DeleteThisAccount()
        {

            try
            {
                var userClaims = HttpContext.User.Claims;
                var user = await _userManager.FindByNameAsync(userClaims.ToList()[0].Value);
                var role = await _userManager.GetRolesAsync(user);
                await _userManager.DeleteAsync(user);
                return StatusCode(200, "User deleted.");

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }
        [Authorize("Admin")]
        [HttpDelete("api/account/{id}/delete")]
        public async Task<IActionResult> DeleteAccount(string id)
        {

            try
            {
                var user = _registerAccountRepository.GetElementById(id);
                var role = await _userManager.GetRolesAsync(user);
                await _userManager.DeleteAsync(user);
                return StatusCode(200, $"User {id} deleted.");

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }
        [HttpGet("api/account/me")]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            try
            {
                var userClaims = HttpContext.User.Claims;
                var user = await _userManager.FindByNameAsync(userClaims.ToList()[0].Value);
                var role = await _userManager.GetRolesAsync(user);
                var returnUser = new
                {
                    CompanyName = user.CompanyName,
                    CompanyNip = user.NIP,
                    CompanyAdress = user.CompanyAddress,
                    CompanyPostalCode = user.CompanyPostalCode,
                    Role = role,
                    Tasks = user.TaskList,

                    Notifications = user.Notifications
                };

                return Ok(returnUser);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }




    }
}