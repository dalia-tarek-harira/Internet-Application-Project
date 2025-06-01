using BookSwap.Data;
using BookSwap.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookSwap.Repository
{
    public class BorrowRequestRepository : IRepository<BorrowRequest>
    {
        private AppDbContext context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BorrowRequestRepository( AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            _httpContextAccessor = httpContextAccessor;

        }
        public void Add(BorrowRequest req)
        {
           context.Add(req);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(BorrowRequest req)
        {
           context.Update(req);
        }
        public List<BorrowRequest> GetAll()
        {
            var userId = _httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return context.BorrowRequests
     .Include(br => br.bookpost)
     .Where(br => br.bookpost.UserId == userId)
     .OrderByDescending(br => br.StartDate)
     .ToList();

        }


        public BorrowRequest GetById(int id)
        {
            var req = context.BorrowRequests.FirstOrDefault(r => r.Id == id);
            return req;
        }

        public void Delete(int id)
        {
            var req=context.BorrowRequests.FirstOrDefault(r=>r.Id == id);
            context.BorrowRequests.Remove(req);
        }
        public void DeleteByBookId (int id)
        {
            var requestsToDelete = context.BorrowRequests
                                 .Where(r => r.BookPostId == id)
                                 .ToList();

            context.BorrowRequests.RemoveRange(requestsToDelete);
          
        }

       
       
    }
}
