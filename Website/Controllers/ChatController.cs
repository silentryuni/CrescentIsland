using CrescentIsland.Website.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CrescentIsland.Website.Controllers
{
    public class ChatController : Controller
    {
        //
        // GET: /Chat/ChatBox
        public ActionResult ChatBox()
        {
            return PartialView();
        }

        //
        // POST: /Chat/GetMessages
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetMessages()
        {
            var amount = 20;

            var messages = new List<string>();
            List<ChatMessage> chatmessages;

            using (var db = HttpContext.GetOwinContext().Get<ChatDbContext>())
            {
                chatmessages = db.ChatMessages.AsNoTracking().OrderByDescending(c => c.Timestamp).Take(amount).ToList();
            }

            foreach (var msg in chatmessages)
            {
                var sb = new StringBuilder();

                sb.Append("<li id=\"");
                sb.Append(msg.Id);
                sb.Append("\"><span class=\"role");
                sb.Append(msg.Role);
                sb.Append("\">");
                sb.Append(msg.UserName);
                sb.Append(": </span><span class=\"chat-time\">");
                sb.Append(msg.Timestamp.ToString("HH:mm"));
                sb.Append("</span>");
                if (User.IsInRole("Administrator"))
                {
                    sb.Append("<span class=\"admin-buttons\"> <a href=\"#\" onclick=\"return Global.DeleteChatMessage(this);\">[D]</a> ");
                    sb.Append("<a href=\"#\" onclick=\"return Global.BanChatUser(this);\">[B]</a></span>");
                }
                sb.Append("<br /><span class=\"chat-message\">");
                sb.Append(HttpUtility.HtmlEncode(msg.Message));
                sb.Append("</span></li>");

                messages.Add(sb.ToString());
            }

            return Json(messages);
        }
    }
}