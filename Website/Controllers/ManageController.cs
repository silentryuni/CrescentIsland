using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using CrescentIsland.Website.Models;
using System.IO;
using System.Collections.Generic;

namespace CrescentIsland.Website.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }
        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Manage/Index
        public ActionResult Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.ChangeAvatarSuccess ? "Your avatar has been changed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.EmailSent ? "E-mail successfully sent"
                : "";

            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user == null) return RedirectToAction("Index", "Page");

            IndexViewModel model;

            model = new IndexViewModel
            {
                HasPassword = user.PasswordHash != null,
                HasVerified = user.EmailConfirmed,
                Email = user.Email,
                AvatarImage = user.AvatarImage,
                AvatarMimeType = user.AvatarMimeType
            };

            return View(model);
        }

        //
        // POST: /Manage/SendVerification
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendVerification(IndexViewModel model)
        {
            var userId = User.Identity.GetUserId();

            string code = await UserManager.GenerateEmailConfirmationTokenAsync(userId);
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = userId, code = code }, protocol: Request.Url.Scheme);
            try
            {
                await UserManager.SendEmailAsync(userId, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                ViewBag.StatusMessage = "E-mail successfully sent";
            }
            catch
            {
                ViewBag.StatusMessage = "An error has occurred.";
            }

            return View("Index", model);
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ChangeAvatar
        public ActionResult ChangeAvatar()
        {
            var model = new ChangeAvatarViewModel();

            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user == null) return RedirectToAction("Index", "Page");

            model.UserAvatarImage = user.AvatarImage;
            model.UserAvatarMimeType = user.AvatarMimeType;

            DirectoryInfo d = new DirectoryInfo(Server.MapPath("/Assets/Images/Avatars"));
            FileInfo[] files = d.GetFiles("*.png");

            model.Avatars = files.Select(f => new Avatar { ImageUrl = f.Name });

            return View(model);
        }

        //
        // POST: /Manage/ChangeAvatar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeAvatar(ChangeAvatarViewModel model)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null) return RedirectToAction("Index", "Page");

            var file = Server.MapPath(model.SelectedAvatar);

            if (!string.IsNullOrEmpty(file))
            {
                FileStream stream = System.IO.File.OpenRead(file);
                var fileBytes = new byte[stream.Length];

                stream.Read(fileBytes, 0, fileBytes.Length);
                stream.Close();

                var ext = Path.GetExtension(stream.Name).Substring(1);

                user.AvatarImage = fileBytes;
                user.AvatarMimeType = ext;
            }

            using (var db = HttpContext.GetOwinContext().Get<ApplicationDbContext>())
            {
                db.Users.Attach(user);
                db.Entry(user).Property(x => x.AvatarImage).IsModified = true;
                db.Entry(user).Property(x => x.AvatarMimeType).IsModified = true;
                var result = await db.SaveChangesAsync();
            }

            return RedirectToAction("Index", new { Message = ManageMessageId.ChangeAvatarSuccess });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            ChangeAvatarSuccess,
            EmailSent,
            Error
        }

#endregion
    }
}