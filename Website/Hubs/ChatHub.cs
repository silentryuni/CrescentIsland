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
        public string clientId = "";

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
            using (var db = new ChatDbContext())
            {
                db.Entry(chatmsg).State = EntityState.Added;
                db.SaveChanges();
                db.Entry(chatmsg).GetDatabaseValues();
            }
            
            Clients.All.addMessage(chatmsg.Id, chatmsg.UserName, message, chatmsg.Timestamp.ToString("HH:mm"), chatmsg.Role);
        }

        public void Delete(int id)
        {
            if (HttpContext.Current.User.IsInRole("Administrator"))
            {
                using (var db = new ChatDbContext())
                {
                    var msg = db.ChatMessages.Find(id);
                    if (msg == null) return;

                    db.Entry(msg).State = EntityState.Deleted;
                    db.SaveChanges();
                }

                Clients.All.removeMessage(id);
            }
        }

        public void Ban(int id)
        {
            if (HttpContext.Current.User.IsInRole("Administrator"))
            {
                ChatMessage msg;

                using (var db = new ChatDbContext())
                {
                    msg = db.ChatMessages.Find(id);
                    if (msg == null) return;
                }

                var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = userManager.FindByName(msg.UserName);

                if (user == null) return;

                userManager.SetLockoutEndDate(user.Id, DateTime.Now.AddDays(1));

                Clients.Client(msg.ConnectionId).lockout();
            }
        }
    }
}