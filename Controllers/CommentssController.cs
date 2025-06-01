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
    public class CommentssController : ControllerBase
    {

        private CommentRepository commrepo;
        public CommentssController(CommentRepository commrepo)
        {
            this.commrepo = commrepo;
        }

        // GET Comments for a specific BookPost
        [HttpGet("Bookpost/{Id}")]
        public IActionResult GetCommentsForPost(int Id)
        {
            var comments = commrepo.GetAll(Id);

            
            return Ok(comments);
        }
        // Add Comment
        [Authorize(Roles ="Book Owner , Reader")]
        [HttpPost]
        public  IActionResult AddComment(CommentDTO newcomment)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            Comment comment = new Comment()
            {
                CommentText=newcomment.CommentText,
                CreatedAt = DateTime.Now,
                UserId=userId,
                BookPostId=newcomment.BookPostId,


            };
         
            commrepo.Add(comment);
            commrepo.Save();

            return Ok(newcomment);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateComment(int id, CommentDTO updatedComment)
        {
           

            var existingComment = commrepo.GetById(id);
            if (existingComment == null)
                return NotFound();

            existingComment.CommentText = updatedComment.CommentText;
            commrepo.Update(existingComment);
            commrepo.Save();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteComment(int id)
        {
            var comment = commrepo.GetById(id);
            if (comment == null)
                return NotFound();
            commrepo.Delete(id);
            commrepo.Save();
            

            return NoContent();
        }
        [HttpPost("AddReplay")]
        public IActionResult AddReply([FromBody] AddRepleyDTO dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(dto.Text))
            {
                return BadRequest("Reply text is required.");
            }

            var comment = commrepo.GetById(dto.CommentId);
            if (comment == null)
            {
                return NotFound("Comment not found.");
            }

          
            if (!string.IsNullOrEmpty(comment.ReplyText))
            {
                return BadRequest("This comment already has a reply.");
            }

            comment.ReplyText = dto.Text;
            comment.ReplyUserId = userId;

            commrepo.Save();
           

            return Ok(comment);
        }






    }
}
