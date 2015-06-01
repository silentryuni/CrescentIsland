using CrescentIsland.Website.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace CrescentIsland.Website.Controllers
{
    public class ChatController : Controller
    {
        // GET: Chat
        public ActionResult ChatBox()
        {
            return PartialView();
        }

        public JsonResult GetMessages()
        {
            var amount = 10;

            var messages = new List<string>();
            List<ChatMessage> chatmessages;

            using (var db = new ChatDbContext())
            {
                chatmessages = db.ChatMessages.OrderByDescending(c => c.Timestamp).Take(amount).ToList();
            }

            foreach (var msg in chatmessages)
            {
                var sb = new StringBuilder();

                sb.Append("<li><strong>");
                sb.Append(msg.UserName);
                sb.Append("</strong>: ");
                sb.Append(msg.Timestamp.ToString("HH:mm"));
                sb.Append("<br />");
                sb.Append(msg.Message);
                sb.Append("</li>");

                messages.Add(sb.ToString());
            }

            return Json(messages, JsonRequestBehavior.DenyGet);
        }

        public JsonResult SaveMessage(string username, string message)
        {
            var timestamp = DateTime.Now;

            var chatmsg = new ChatMessage()
            {
                UserName = username,
                Message = message,
                Timestamp = timestamp
            };

            using (var db = new ChatDbContext())
            {
                //Add newStudent entity into DbEntityEntry and mark EntityState to Added
                db.Entry(chatmsg).State = EntityState.Added;

                // call SaveChanges method to save new Student into database
                db.SaveChanges();
            }

            return Json(timestamp.ToString("HH:mm"), JsonRequestBehavior.DenyGet);
        }
    }
}