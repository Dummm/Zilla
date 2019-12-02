using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CompareAttribute = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace Zilla.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        //[Required]
        //[Display(Name = "Email")]
        //[EmailAddress]
        //public string Email { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class EditAccountViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Roles")]
        public IEnumerable<SelectListItem> RolesList { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "EmailConfirmed")]
        public bool EmailConfirmed { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "PasswordHash")]
        public string PasswordHash { get; set; }

        [Required]
        [Display(Name = "SecurityStamp")]
        public string SecurityStamp { get; set; }

        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Display(Name = "PhoneNumberConfirmed")]
        public bool PhoneNumberConfirmed { get; set; }

        [Display(Name = "TwoFactorEnabled")]
        public bool TwoFactorEnabled { get; set; }

        [Display(Name = "LockoutEndDateUtc")]
        public DateTime? LockoutEndDateUtc { get; set; }

        [Display(Name = "LockoutEnabled")]
        public bool LockoutEnabled { get; set; }

        [Display(Name = "AccessFailedCount")]
        public int AccessFailedCount { get; set; }

    }

    public class DetailsAccountViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Roles")]
        public IList<string> RolesList { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "EmailConfirmed")]
        public bool EmailConfirmed { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "PasswordHash")]
        public string PasswordHash { get; set; }

        [Required]
        [Display(Name = "SecurityStamp")]
        public string SecurityStamp { get; set; }

        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Display(Name = "PhoneNumberConfirmed")]
        public bool PhoneNumberConfirmed { get; set; }

        [Display(Name = "TwoFactorEnabled")]
        public bool TwoFactorEnabled { get; set; }

        [Display(Name = "LockoutEndDateUtc")]
        public DateTime? LockoutEndDateUtc { get; set; }

        [Display(Name = "LockoutEnabled")]
        public bool LockoutEnabled { get; set; }

        [Display(Name = "AccessFailedCount")]
        public int AccessFailedCount { get; set; }

    }

    public class IndexAccountViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Roles")]
        public IList<string> RolesList { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "EmailConfirmed")]
        public bool EmailConfirmed { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "PasswordHash")]
        public string PasswordHash { get; set; }

        [Required]
        [Display(Name = "SecurityStamp")]
        public string SecurityStamp { get; set; }

        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Display(Name = "PhoneNumberConfirmed")]
        public bool PhoneNumberConfirmed { get; set; }

        [Display(Name = "TwoFactorEnabled")]
        public bool TwoFactorEnabled { get; set; }

        [Display(Name = "LockoutEndDateUtc")]
        public DateTime? LockoutEndDateUtc { get; set; }

        [Display(Name = "LockoutEnabled")]
        public bool LockoutEnabled { get; set; }

        [Display(Name = "AccessFailedCount")]
        public int AccessFailedCount { get; set; }

    }
}
