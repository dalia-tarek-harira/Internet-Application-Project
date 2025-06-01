using System.ComponentModel.DataAnnotations;

namespace BookSwap.DTO
{
    public class RegisterAdminDTO
    {
      
        public string Username { get; set; }
       
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
       
        public string Email { get; set; }
        public string Address {  get; set; }    

        public string Phone { get; set; }
        public int Age { get; set; }
      





    }
}
