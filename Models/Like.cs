using System.ComponentModel.DataAnnotations.Schema;

namespace BookSwap.Models
{
    public class Like
    {
     public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId {  get; set; }
     public int BookPostId {  get; set; }
     public bool ReactionType {  get; set; }
     public BookPost BookPost { get; set; }
        // User Object
     public ApplicationUser User { get; set; }






    }
}
