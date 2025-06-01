using BookSwap.Models;

namespace BookSwap.Interfaces
{
    public interface ITokenService
    {
        public  Task<string> GenerateAccessToken(ApplicationUser user);
        public string GenerateRefreshToken();



    }
}
