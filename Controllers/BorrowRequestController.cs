using BookSwap.Data;
using BookSwap.DTO;
using BookSwap.Hubs;
using BookSwap.Models;
using BookSwap.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace BookSwap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowRequestController : ControllerBase
    {

        private BorrowRequestRepository reqrepo;
        private BookPostRepository bookrepo;
        private UserManager<ApplicationUser> usermanager;
        private readonly IHubContext<BookHub> hubContext;
        public BorrowRequestController( UserManager<ApplicationUser> usermanager
            , BorrowRequestRepository reqrepo, BookPostRepository bookrepo,   IHubContext<BookHub> _hubContext)
        {
           
            this.usermanager = usermanager;
            this.reqrepo = reqrepo;
            this.bookrepo = bookrepo;
            hubContext = _hubContext;
        }
        [Authorize(Roles = "Reader")]
        [HttpPost]
        public IActionResult BorrowRequest([FromForm] BorrowRequestDTO borrowreq)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var book = bookrepo.GetById(borrowreq.BookPostId);
            if (book == null) return NotFound("Book not found");

            if (book.BorrowStatus == "Borrowed")
                return BadRequest("Book is already borrowed.");

            BorrowRequest borrowrequest = new BorrowRequest()
            {
                ReqDate = DateTime.Now,
                StartDate = borrowreq.StartDate,
                EndDate = borrowreq.EndDate,
                status = "Pending",
                UserId = userId,
                BookPostId = book.Id
            };

            reqrepo.Add(borrowrequest);
            reqrepo.Save();

           
            return Ok(new { BorrowRequestID = borrowrequest.Id });
        }

       
        [Authorize(Roles = "Book Owner")]
        [HttpGet("AllRequests")]
        public IActionResult GetOwnerRequests()
        {
          var requests = reqrepo.GetAll();

            return Ok(requests);
        }

        [Authorize(Roles = "Book Owner")]
        [HttpGet("GetRequest/{id}")]
        public IActionResult GetById(int id)
        {

            var req = reqrepo.GetById(id);
            return Ok(req);
        }

        [Authorize(Roles ="Book Owner")]
        [HttpPut("{id:int}/{newStatus:alpha}")]
        public async Task <IActionResult> UpdateRequestStatus(int id, string newStatus)
        {
            var request = reqrepo.GetById(id);
            if (request == null) return NotFound();

            var book = bookrepo.GetById(request.BookPostId);
            if (book == null) return NotFound("Book not found");

            request.status = newStatus;

            if (newStatus == "Accepted" && request.EndDate > DateTime.Now)
            {
                book.BorrowStatus = "Borrowed";
                request.status = "Accepted";

                Console.WriteLine("📤 Sending SignalR notification to: " + request.UserId);

                await hubContext.Clients.Group(request.UserId)
      .SendAsync("ReceiveNotification", "✅ Your borrow request has been accepted!");

            }
            else
            {
                request.status = "Rejected";
                await hubContext.Clients.Group(request.UserId)
    .SendAsync("ReceiveNotification", "❌ Your borrow request was rejected.");


            }
            reqrepo.Save();
            return NoContent();
        }
     
    }
}
