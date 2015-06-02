using CrescentIsland.Website.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR;
using System;
using System.Data.Entity;
using System.Web;

namespace CrescentIsland.Website.Hubs
{
    public class ChatHub : Hub
    {
        private ChatDbContext _chatDb;

        public ChatHub()
        {
        }
        public ChatHub(ChatDbContext chatDb)
        {
            ChatDb = chatDb;
        }

        public ChatDbContext ChatDb
        {
            get
            {
                return _chatDb ?? HttpContext.Current.GetOwinContext().GetUserManager<ChatDbContext>();
            }
            private set
            {
                _chatDb = value;
            }
        }

        public void Send(string name, string message)
        {
            var chatmsg = new ChatMessage()
            {
                UserName = HttpContext.Current.User.Identity.Name,
                Message = message,
                Role = HttpContext.Current.User.IsInRole("Administrator") ? "Admin" : "",
                Timestamp = DateTime.Now,
                ConnectionId = Context.ConnectionId
            };

            // Add chat message to database
            ChatDb.Entry(chatmsg).State = EntityState.Added;
            ChatDb.SaveChanges();
            ChatDb.Entry(chatmsg).GetDatabaseValues();
            
            Clients.All.addMessage(chatmsg.Id, chatmsg.UserName, message, chatmsg.Timestamp.ToString("HH:mm"), chatmsg.Role);
        }

        [Authorize(Roles = "Administrator")]
        public void Delete(int id)
        {
            var msg = ChatDb.ChatMessages.Find(id);
            if (msg == null) return;

            ChatDb.Entry(msg).State = EntityState.Deleted;
            ChatDb.SaveChanges();

            Clients.All.removeMessage(id);
        }

        [Authorize(Roles = "Administrator")]
        public void Ban(int id)
        {
            ChatMessage msg;

            msg = ChatDb.ChatMessages.Find(id);
            if (msg == null) return;

            User user;
            using (var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>())
            {
                user = userManager.FindByName(msg.UserName);

                if (user == null) return;

                userManager.SetLockoutEndDate(user.Id, DateTime.Now.AddDays(1));
            }

            Clients.Client(msg.ConnectionId).lockout();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _chatDb != null)
            {
                _chatDb.Dispose();
                _chatDb = null;
            }

            base.Dispose(disposing);
        }
    }
}