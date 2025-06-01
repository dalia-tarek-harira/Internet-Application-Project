using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookSwap.Models
{
    public class BookPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string language { get; set; }
        public string genere { get; set; }
        public string ISBN { get; set; }
        public string BorrowStatus { get; set; }
        public string bookownername {  get; set; }
        public int BorrowPrice { get; set; }
     public string PostStatus { get; set; } = "Pending";
     public DateTime PublicationDate { get; set; }= DateTime.Now;
     public DateTime StartAvailability { get; set;} 
     public DateTime EndAvailability { get; set; }
     public string CoverImageUrl {  get; set; }
        [ForeignKey("User")]
     public string UserId {  get; set; }  

      // Book owner id (user id)
     public ApplicationUser User { get; set; }

        [JsonIgnore]
        public List<BorrowRequest> BorrowRequests { get; set; }
     public List<Like> Likes { get; set; }







    }
}
