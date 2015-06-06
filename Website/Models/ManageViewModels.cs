using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CrescentIsland.Website.Models
{
    public class ChangeInfoViewModel
    {
        public bool HasPassword { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Gender")]
        public UserGender UserGender { get; set; } = UserGender.Male;
        [Required]
        public int Year { get; set; }
        [Required]
        public int Month { get; set; }
        [Required]
        public int Day { get; set; }
        public string Country { get; set; }
        public bool HasVerified { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public IEnumerable<SelectListItem> YearList { get; set; }
        public IEnumerable<SelectListItem> MonthList { get; set; }
        public IEnumerable<SelectListItem> DayList { get; set; }
    }

    public class ChangeSettingsViewModel
    {
        public bool HasPassword { get; set; }

        [Required]
        [Display(Name = "Display Age")]
        public bool ShowAge { get; set; }
        [Required]
        [Display(Name = "Display Gender")]
        public bool ShowGender { get; set; }
        [Required]
        [Display(Name = "Display Money")]
        public bool ShowMoney { get; set; }
    }

    public class ChangeAvatarViewModel
    {
        public bool HasPassword { get; set; }

        public byte[] UserAvatarImage { get; set; }
        public string UserAvatarMimeType { get; set; }
        public IEnumerable<Avatar> Avatars { get; set; }
        public string SelectedAvatar { get; set; }
    }

    public class Avatar
    {
        public string ImageUrl { get; set; }
    }

    public class SetPasswordViewModel
    {
        public bool HasPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        public bool HasPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}