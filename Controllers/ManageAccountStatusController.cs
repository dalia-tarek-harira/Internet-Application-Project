using BookSwap.Data;
using BookSwap.DTO;
using BookSwap.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookSwap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ManageAccountStatusController : ControllerBase
    {
        public UserManager<ApplicationUser> Usermanager;
        public AppDbContext Context;
        public ManageAccountStatusController(UserManager<ApplicationUser> usermanager, AppDbContext context)
        {
            Usermanager = usermanager;
            Context = context;

        }
        [HttpGet("pending")]
      
        public IActionResult GetPendingBookOwners()
        {
          var Pending=Usermanager.Users
                .Where( U=>U.AccountStatus == "Pending" ).ToList();
            return Ok(Pending);

        }

        [HttpPost("accept/{id}")]
        public async Task<IActionResult> AcceptedAccount(string id)
        {
          var Acceptedowner= await Usermanager.FindByIdAsync(id);
            if (Acceptedowner != null)
            {
                Acceptedowner.AccountStatus = "Accepted";
                await Usermanager.UpdateAsync(Acceptedowner);
                return Ok("BookOwner Accepted");
          }

         return NotFound();

        }
        [HttpPost("reject/{id}")]
        public async Task<IActionResult> RejectedAccount(string id)
        {
            var Acceptedowner = await Usermanager.FindByIdAsync(id);
            if (Acceptedowner != null)
            {
                Acceptedowner.AccountStatus = "Rejected";
                await Usermanager.UpdateAsync(Acceptedowner);
                return Ok("BookOwner Rejected");
            }

            return NotFound();

        }



    }
}
