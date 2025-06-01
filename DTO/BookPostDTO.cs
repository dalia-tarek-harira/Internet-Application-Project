namespace BookSwap.DTO
{
    public class BookPostDTO
    {
        public string Title { get; set; }
        public string language { get; set; }
        public string genere { get; set; }
        public string ISBN { get; set; }
      
        public int BorrowPrice { get; set; }
        public string bookownername {  get; set; }

        public DateTime StartAvailability { get; set; }
        public DateTime EndAvailability { get; set; }
        public IFormFile CoverImage { get; set; }
      






    }
}
