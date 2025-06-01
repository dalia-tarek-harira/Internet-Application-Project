using BookSwap.DTO;
using BookSwap.Interfaces;
using BookSwap.Models;
using BookSwap.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookSwap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public UserManager<ApplicationUser> Usermanager;
        public RoleManager<IdentityRole> rolemanager;
        public IConfiguration Config;
        public ITokenService tokenservice;
        public AccountController(UserManager<ApplicationUser> usermanager,
            IConfiguration config,ITokenService tokenservice,RoleManager<IdentityRole> roleManager)
        {

            Usermanager = usermanager;
            rolemanager = roleManager;
            Config = config;
            this.tokenservice = tokenservice;
          

        }

        // Admin Register
        [HttpPost("Register")]
       public async Task< IActionResult> RegisterAdmin(RegisterAdminDTO datafromreq,string Role)
        {
            if (datafromreq.Password != datafromreq.ConfirmPassword)
            {
                return BadRequest();
            }
            ApplicationUser appuser=new ApplicationUser();
            appuser.Email = datafromreq.Email;
            appuser.UserName = datafromreq.Username;
            appuser.PhoneNumber = EncryptionHelper.Encrypt(datafromreq.Phone);
            appuser.Address = EncryptionHelper.Encrypt(datafromreq.Address);
            appuser.age = datafromreq.Age;
            appuser.AccountStatus = "Pending";
            
           
            if(await rolemanager.RoleExistsAsync(Role))
            {
                IdentityResult result = await Usermanager.CreateAsync(appuser, datafromreq.Password);
                if (result.Succeeded)
                   
                {
                    // add role to user

                    await Usermanager.AddToRoleAsync(appuser, Role);
                    return Ok(appuser);
                }
            }

            return BadRequest(ModelState);




        }

        [HttpPost("Login")]
       
        public async Task<IActionResult> Login(LoginDTO datafromreq)
        {
            ApplicationUser userfromdb = await Usermanager.FindByNameAsync(datafromreq.Username);
            if (userfromdb != null)
            {
                bool found = await Usermanager.CheckPasswordAsync(userfromdb, datafromreq.Password);

                if (!found)
                {
                    return Unauthorized();
                }

                var accessToken = await tokenservice.GenerateAccessToken(userfromdb);
                var refreshToken = tokenservice.GenerateRefreshToken();

                userfromdb.RefreshToken = refreshToken;
                userfromdb.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(30); 
                await Usermanager.UpdateAsync(userfromdb);

                var roles = await Usermanager.GetRolesAsync(userfromdb);
                var role = roles.FirstOrDefault();

                return Ok(new
                {
                    accessToken,
                    refreshToken,
                    refreshTokenExpiry = userfromdb.RefreshTokenExpiryTime,
                    role,
                    userId = userfromdb.Id,
                      status = userfromdb.AccountStatus
                });

            }
            ModelState.AddModelError("Username", "Invalid username Or Password");
            return BadRequest(ModelState);
        }


        [HttpGet("getuser/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await Usermanager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await Usermanager.GetRolesAsync(user);
            return Ok(new
            {
                user.Id,
                user.UserName,
                user.Email,
                Role = roles.FirstOrDefault(),
                Status = user.AccountStatus // ← تأكدي إن عندك الخاصية دي
            });
        }















        [Authorize(Roles = "Admin")]
        [HttpGet("{username}")]
        public async Task<IActionResult> GetProfile(string username)
        {

            var user = await Usermanager.FindByNameAsync(username);


            var profile = new
            {
                user.UserName,
                Phone = EncryptionHelper.Decrypt(user.PhoneNumber),
                Address = EncryptionHelper.Decrypt(user.Address)
            };

            return Ok(profile);
        }





        /*    [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] MessageDto messageDto)
        {
            await _hubContext.Clients.Users(messageDto.UserId).SendAsync("ReceiveNotification", messageDto.Message);
            return Ok();
        }
        
         
         */








    }
}
