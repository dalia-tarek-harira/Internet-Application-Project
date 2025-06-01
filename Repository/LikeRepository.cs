using BookSwap.Data;
using BookSwap.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BookSwap.Repository
{
    public class LikeRepository : IRepository<Like>
    {
        private AppDbContext context;
        private readonly IHttpContextAccessor _httpContextAccessor;

       
        public LikeRepository(AppDbContext context,IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this._httpContextAccessor = httpContextAccessor;
        }
        public void Add(Like like)
        {
           context.Add(like);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Like entity)
        {
            throw new NotImplementedException();
        }
        public void Delete(int id) 
        { 
         var like =context.Likes.First(l => l.Id == id);
            context.Likes.Remove(like);
        
        }

        public Like GetById(int bookPostId)
        {
            var userId = _httpContextAccessor.HttpContext?.User?
              .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var like=context.Likes
                .FirstOrDefault(l => l.UserId == userId && l.BookPostId == bookPostId);
            return like;
        }
    }
}
