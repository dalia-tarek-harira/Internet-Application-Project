using BookSwap.Data;
using BookSwap.Models;

namespace BookSwap.Repository
{
    public class CommentRepository : IRepository<Comment>
    {
        private AppDbContext context;
        private BookPostRepository bookrepo;
        public CommentRepository(AppDbContext context)
        {
            this.context = context;
        }
        public void Add(Comment comment)
        {
           context.Add(comment);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Comment comment)
        {
            context.Update(comment);
        }
        public void Delete(int id) 
        { 
        var comment=context.Comments.FirstOrDefault(c => c.Id==id); 
            context.Comments.Remove(comment);

        
        }

        public Comment GetById(int id)
        {
          var com=context.Comments.First(c => c.Id==id);
            return com;
        }

        public List<Comment> GetAll(int id)   
        {
            var comments = context.Comments
                 .Where(c => c.BookPostId == id)
                 .ToList();
            return comments;    
        }
    }
}
