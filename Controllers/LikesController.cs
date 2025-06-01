using BookSwap.Data;
using BookSwap.DTO;
using BookSwap.Models;
using BookSwap.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookSwap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private LikeRepository likerepo;
        public LikesController(LikeRepository likerepo)
        {
            this.likerepo = likerepo;
        }
        [Authorize(Roles ="Reader")]
        [HttpPost]
        public IActionResult AddLike([FromBody] LikeDTO likeDto)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
          

            if (userId == null)
                return Unauthorized();


            var like = new Like
            {
                UserId = userId,
                BookPostId = likeDto.BookPostId,
                ReactionType = likeDto.ReactionType  // 1 is like 0 is not like
            };

           likerepo.Add(like);
            likerepo.Save();

            return Ok(like);
        }
        [Authorize(Roles = "Reader")]
        [HttpDelete("unlike/{bookPostId}")]
        public IActionResult Unlike(int bookPostId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            

            if (userId == null)
                return Unauthorized("User is not authenticated.");

            var existingReaction = likerepo.GetById(bookPostId);

           

            likerepo.Delete(existingReaction.Id);
            likerepo.Save();

            return Ok();
        }



    }
}
