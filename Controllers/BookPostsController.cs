using BookSwap.Data;
using BookSwap.DTO;
using BookSwap.Models;
using BookSwap.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookSwap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class BookPostsController : ControllerBase
    {
        private BookPostRepository bookrepo;
        private BorrowRequestRepository borrowrequestrepo;
        private  UserManager<ApplicationUser> usermanager;    
        public BookPostsController( UserManager<ApplicationUser> usermanager, BookPostRepository bookrepo,
            BorrowRequestRepository borrowrequestrepo)
        {
           
            this.usermanager = usermanager;
            this.bookrepo = bookrepo;
            this.borrowrequestrepo = borrowrequestrepo;

         

        }

        //get book by id
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var post=bookrepo.GetById(id);
            if (post == null) return NotFound();
            return Ok(post);

        }

        [HttpGet("AllPosts")]
        public IActionResult GetAll()
        {
            var listAll = bookrepo.GetAll()
                                  .Where(b => b.PostStatus == "Accepted")
                                  .ToList();

            return Ok(listAll);
        }





        // Add New BookPost
        [Authorize(Roles ="Book Owner")]
       
        [HttpPost("Add")]
       
        public async Task<IActionResult> AddBook([FromForm] BookPostDTO dto)
        {
            
            
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var user =await  usermanager.FindByIdAsync(userId);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Cannot identify user.");
            if (user.AccountStatus!= "Accepted")
                return Forbid("Your account is not approved to add books.");

            if (dto.CoverImage == null || dto.CoverImage.Length == 0)
                return BadRequest("Cover image is required.");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.CoverImage.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                dto.CoverImage.CopyTo(stream);
            }

            var coverImageUrl = Path.Combine("uploads", uniqueFileName).Replace("\\", "/");

            var book = new BookPost
            {
                Title = dto.Title,
                language = dto.language,
                genere = dto.genere,
                ISBN = dto.ISBN,
                BorrowPrice = dto.BorrowPrice,
                StartAvailability = dto.StartAvailability,
                EndAvailability = dto.EndAvailability,
                CoverImageUrl = coverImageUrl,
                BorrowStatus= "Available",
               PostStatus="Pending",
                UserId = userId ,
                bookownername=dto.bookownername
            };
            bookrepo.Add(book);
            bookrepo.Save();
         

            return Ok();
        }





        [Authorize(Roles = "Book Owner")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> EditBookPost(int id, [FromForm] BookPostDTO updatedbook)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Cannot identify user.");

            var bookfromdb = bookrepo.GetById(id);
            if (bookfromdb == null)
                return BadRequest("BookPost does not exist.");

            if (bookfromdb.UserId != userId)
                return Forbid("You are not allowed to edit this book.");

            bookfromdb.Title = updatedbook.Title;
            bookfromdb.ISBN = updatedbook.ISBN;
            bookfromdb.language = updatedbook.language;
            bookfromdb.genere = updatedbook.genere;
            bookfromdb.BorrowPrice = updatedbook.BorrowPrice;
           
            bookfromdb.StartAvailability = updatedbook.StartAvailability;
            bookfromdb.EndAvailability = updatedbook.EndAvailability;

       
            if (updatedbook.CoverImage != null && updatedbook.CoverImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(updatedbook.CoverImage.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await updatedbook.CoverImage.CopyToAsync(stream);
                }

                var coverImageUrl = Path.Combine("uploads", uniqueFileName).Replace("\\", "/");
                bookfromdb.CoverImageUrl = coverImageUrl;
            }

            bookrepo.Update(bookfromdb);
            bookrepo.Save();

            return NoContent();
        }



        [HttpDelete("{id:int}")]
        public IActionResult DeleteBookPost(int id)
        {
            var bookfromdb = bookrepo.GetById(id);  
            if (bookfromdb == null) return NotFound();
          
            borrowrequestrepo.DeleteByBookId(id);
            borrowrequestrepo.Save();

            bookrepo.Delete(id);
           
            bookrepo.Save();
            return NoContent();
        }

        [HttpGet("search-by-name/{title}")]
        public IActionResult SearchBooks(string title)
        {
             var result=bookrepo.Searchbytitle(title);

            return Ok(result);   
        }
        [Authorize(Roles ="Book Owner")]
        [HttpGet("MyPosts")]
        public IActionResult GetMyPosts()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not identified.");

            var myPosts = bookrepo.GetAll()
                .Where(b => b.UserId == userId).Select(b=>new{
                 
                 b.BorrowPrice,
                 b.Id,
                 b.Title,
                 b.bookownername,
                 b.ISBN,
                 b.genere,
                 b.StartAvailability, b.EndAvailability,b.CoverImageUrl,b.BorrowStatus


            });
                

            return Ok(myPosts);
        }




        [HttpGet("search-by-price/{price}")]
        public IActionResult SearchBooksByPrice(decimal price)
        {
            var result = bookrepo.Searchbyprice(price);

            return Ok(result);
        }


        [HttpPut("BookAvailability")]
        public IActionResult BookAvailability(int id)
        {
            var borrowRequest = borrowrequestrepo.GetById(id); 
            if (borrowRequest == null) return NotFound();

            var bookPostId = borrowRequest.BookPostId;  
            var bookpost = bookrepo.GetById(bookPostId);

            if (borrowRequest.status == "Accepted" && borrowRequest.EndDate > DateTime.Now)
                bookpost.BorrowStatus = "Borrowed";
            else
                bookpost.BorrowStatus = "Available";

            bookrepo.Save();
            return Ok(new
            {
                Message = "Book status updated successfully",
                BookId = bookpost.Id,
                NewStatus = bookpost.BorrowStatus
            });
        }







    }
}
