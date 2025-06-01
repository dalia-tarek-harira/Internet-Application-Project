using BookSwap.Data;
using BookSwap.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookSwap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ManageBookPostsController : ControllerBase
    {

        public AppDbContext Context;
        public ManageBookPostsController(AppDbContext context)
        {
            Context = context;
        }

        [HttpGet("AllPeding")]
        public IActionResult GetAllPending()
        {
            var Pending=Context.BookPosts.Where(b=>b.PostStatus=="Pending")
                .ToList();
            return Ok(Pending);
        }

        [HttpPost("Accept/{id}")]
        public IActionResult AcceptPost (int id)
        {
          var acceptedpost=Context.BookPosts.First(b=>b.Id==id);
            if (acceptedpost == null) return NotFound();
            acceptedpost.PostStatus = "Accepted";
            Context.SaveChanges();

            return Ok("Post is Accepted");


        }
        [HttpPost("Reject/{id}")]
        public IActionResult RejectedPost(int id)
        {
            var rejectedpost = Context.BookPosts.First(b => b.Id == id);
            if (rejectedpost == null) return NotFound();
            rejectedpost.PostStatus = "Rejected";
            Context.SaveChanges();

            return Ok("Post is Rejected");


        }




















    }
}
