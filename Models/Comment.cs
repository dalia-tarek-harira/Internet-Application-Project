using System.ComponentModel.DataAnnotations.Schema;

namespace BookSwap.Models
{
    public class Comment
    {
     public int Id { get; set; }    
     public string CommentText {  get; set; }   
     public DateTime CreatedAt { get; set; }
     public int BookPostId {  get; set; }
        // Reader is refrence (user id )
        [ForeignKey("User")]
        public string UserId {  get; set; }
        public string? ReplyText { get; set; }
        public string ?ReplyUserId { get; set; }
        public BookPost bookpost { get; set; }
    public ApplicationUser User { get; set; }





    }
}
