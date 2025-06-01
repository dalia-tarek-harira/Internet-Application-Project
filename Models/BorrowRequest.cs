using System.ComponentModel.DataAnnotations.Schema;

namespace BookSwap.Models
{
    public class BorrowRequest
    {

    public int Id { get; set; } 
    public DateTime ReqDate { get; set; }= DateTime.Now;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; } 
    public string status {  get; set; }
     public int BookPostId {  get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
     public BookPost bookpost { get; set; }
     // Reader id refrence(user id)
     public ApplicationUser User { get; set; }


    }
}
