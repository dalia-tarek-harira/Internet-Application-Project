using System.ComponentModel.DataAnnotations.Schema;

namespace BookSwap.DTO
{
    public class CommentDTO
    {
   
        public string CommentText { get; set; }
     
        public int BookPostId { get; set; }
       
       
    }
}
