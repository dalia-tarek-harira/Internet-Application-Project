namespace BookSwap.DTO
{
    public class BorrowRequestDTO
    {
        public int BookPostId {  get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
    }
}
