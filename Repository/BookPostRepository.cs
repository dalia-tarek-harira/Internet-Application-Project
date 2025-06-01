using BookSwap.Data;
using BookSwap.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookSwap.Repository
{
    public class BookPostRepository : IRepository<BookPost>
    {
        private AppDbContext context;
        public BookPostRepository(AppDbContext context)
        {
            this.context = context;
        }
        public void Add(BookPost book)
        {
           context.Add(book);
        }
        public void Update(BookPost book)
        {
            context.Update(book);
        }
        public void Save()
        {
            context.SaveChanges();
        }
        public void Delete(int id) 
        { 
            var book=context.BookPosts.First(p => p.Id == id);
            context.Remove(book);   
        }
        public List<BookPost> GetAll() 
        { 
          return context.BookPosts.ToList();
        }
        public BookPost GetById(int id)
        {
            var bookPost = context.BookPosts.FirstOrDefault(b => b.Id == id);
            return bookPost;

        }
        public List<BookPost> Searchbytitle(string title)
        {
            var result = context.BookPosts
              .Where(b => b.Title.ToLower().Contains(title.ToLower()))
              .ToList();
            return result;  


        }
        public List<BookPost> Searchbyprice(decimal price)
        {

            var result = context.BookPosts
                .Where(b => b.BorrowPrice == price)
                .ToList();
            return result;


        }
      


    }
}
