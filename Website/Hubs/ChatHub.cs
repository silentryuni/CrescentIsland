using Microsoft.AspNet.SignalR;

namespace CrescentIsland.Website.Hubs
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message, string timestamp)
        {
            // Call the broadcastMessage method to update clients.
            //Clients.All.broadcastMessage(name, message);

            Clients.All.addNewMessageToPage(name, message, timestamp);
        }
    }
}