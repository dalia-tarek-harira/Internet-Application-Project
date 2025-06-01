using Microsoft.AspNetCore.Identity;

namespace BookSwap.Models
{
    public class ApplicationUser: IdentityUser
    {

     public string ?Address {  get; set; }   
    
     public string? AccountStatus { get; set; } //pending,Accepted,Rejected  
     public int age {  get; set; }
     public string? RefreshToken { get; set; }
     public DateTime? RefreshTokenExpiryTime { get; set; }
        // for bookowner
     public List<BookPost> ?Posts { get; set; }
     /**for readers**/
    public List<BorrowRequest> ?BorrowRequests { get; set;}
  
     public List<Comment>? comments { get; set; }
     public List<Like>?Likes { get; set; }
    


    }
}
