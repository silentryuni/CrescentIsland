using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using CrescentIsland.Website.Models;
using System.IO;
using System;

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
        // GET: /Manage/ChangeInfo
        public ActionResult ChangeInfo(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangeInfoSuccess ? "Your information has been changed."
                : message == ManageMessageId.EmailSent ? "E-mail successfully sent"
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";

            var model = new ChangeInfoViewModel();

            model.HasPassword = false;
            model.UserGender = UserGender.Male;
            model.YearList = Enumerable.Range(DateTime.Now.Year - 100, 100).Select(i => new SelectListItem { Value = i.ToString(), Text = i.ToString() }).Reverse();
            model.MonthList = Enumerable.Range(1, 12).Select(i => new SelectListItem { Value = i.ToString(), Text = i.ToString() });
            model.DayList = Enumerable.Range(1, 31).Select(i => new SelectListItem { Value = i.ToString(), Text = i.ToString() });

            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                model.HasPassword = user.PasswordHash != null;
                model.Email = user.Email;
                model.HasVerified = user.EmailConfirmed;
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.UserGender = user.UserGender;
                model.Year = user.Birthday.Year;
                model.Month = user.Birthday.Month;
                model.Day = user.Birthday.Day;
                model.Country = user.Country;
            }

            return View(model);
        }

        //
        // POST: /Manage/ChangeInfo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeInfo(ChangeInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user == null) return RedirectToAction("ChangeInfo", new { message = ManageMessageId.Error });

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Country = model.Country;
                user.UserGender = model.UserGender;
                user.Birthday = new DateTime(model.Year, model.Month, model.Day);
                if (user.Email != model.Email)
                {
                    user.Email = model.Email;
                    user.EmailConfirmed = false;
                }

                var result = await UserManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ChangeInfo", new { message = ManageMessageId.ChangeInfoSuccess });
                }
                else
                {
                    return RedirectToAction("ChangeInfo", new { message = ManageMessageId.Error });
                }
            }

            model.YearList = Enumerable.Range(DateTime.Now.Year - 100, 100).Select(i => new SelectListItem { Value = i.ToString(), Text = i.ToString() }).Reverse();
            model.MonthList = Enumerable.Range(1, 12).Select(i => new SelectListItem { Value = i.ToString(), Text = i.ToString() });
            model.DayList = Enumerable.Range(1, 31).Select(i => new SelectListItem { Value = i.ToString(), Text = i.ToString() });

            return View(model);
        }

        //
        // GET: /Manage/ChangeSettings
        public ActionResult ChangeSettings(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangeSettingsSuccess ? "Your settings has been changed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";

            var model = new ChangeSettingsViewModel() {
                HasPassword = false,
                ShowAge = false,
                ShowGender = false,
                ShowMoney = false
            };

            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                model.HasPassword = user.PasswordHash != null;
                model.ShowAge = user.ShowAge;
                model.ShowGender = user.ShowGender;
                model.ShowMoney = user.ShowMoney;
            }

            return View(model);
        }

        //
        // POST: /Manage/ChangeSettings
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeSettings(ChangeSettingsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user == null) return RedirectToAction("ChangeSettings", new { message = ManageMessageId.Error });

                user.ShowAge = model.ShowAge;
                user.ShowGender = model.ShowGender;
                user.ShowMoney = model.ShowMoney;

                var result = await UserManager.UpdateAsync(user);

                if (result.Succeeded)
                    return RedirectToAction("ChangeSettings", new { message = ManageMessageId.ChangeSettingsSuccess });
                else
                    return RedirectToAction("ChangeSettings", new { message = ManageMessageId.Error });
            }

            return View(model);
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";

            var model = new ChangePasswordViewModel()
            {
                HasPassword = false
            };

            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                model.HasPassword = user.PasswordHash != null;
            }

            return View(model);
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
                return RedirectToAction("ChangePassword", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            var model = new SetPasswordViewModel()
            {
                HasPassword = false
            };

            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                model.HasPassword = user.PasswordHash != null;
            }

            return View(model);
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
                    return RedirectToAction("ChangePassword", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ChangeAvatar
        public ActionResult ChangeAvatar(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangeAvatarSuccess ? "Your avatar has been changed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";

            DirectoryInfo d = new DirectoryInfo(Server.MapPath("/Assets/Images/Avatars"));
            FileInfo[] files = d.GetFiles("*.png");

            var model = new ChangeAvatarViewModel()
            {
                HasPassword = false,
                Avatars = files.Select(f => new Avatar { ImageUrl = f.Name }),
                UserAvatarImage = new byte[0],
                UserAvatarMimeType = ""
            };

            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                model.HasPassword = user.PasswordHash != null;
                model.UserAvatarImage = user.AvatarImage;
                model.UserAvatarMimeType = user.AvatarMimeType;
            }
            
            return View(model);
        }

        //
        // POST: /Manage/ChangeAvatar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeAvatar(ChangeAvatarViewModel model)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null) return RedirectToAction("ChangeAvatar", new { message = ManageMessageId.Error });

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

            var result = await UserManager.UpdateAsync(user);

            if (result.Succeeded)
                return RedirectToAction("ChangeAvatar", new { message = ManageMessageId.ChangeAvatarSuccess });
            else
                return RedirectToAction("ChangeAvatar", new { message = ManageMessageId.Error });
        }

        //
        // POST: /Manage/SendVerification
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendVerification()
        {
            var userId = User.Identity.GetUserId();

            string code = await UserManager.GenerateEmailConfirmationTokenAsync(userId);
            var callbackUrl = Url.Action("ConfirmEmail", "User", new { userId = userId, code = code }, protocol: Request.Url.Scheme);
            try
            {
                await UserManager.SendEmailAsync(userId, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                return RedirectToAction("ChangeInfo", new { message = ManageMessageId.EmailSent });
            }
            catch
            {
                return RedirectToAction("ChangeInfo", new { message = ManageMessageId.Error });
            }
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
            ChangeInfoSuccess,
            ChangeSettingsSuccess,
            ChangePasswordSuccess,
            SetPasswordSuccess,
            ChangeAvatarSuccess,
            EmailSent,
            Error
        }

#endregion
    }
}