using Microsoft.AspNetCore.SignalR;

namespace BookSwap.Hubs
{
    public class BookHub:Hub
    {
        public async Task RegisterReader(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }

    }
}
