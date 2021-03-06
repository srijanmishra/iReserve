﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iReserve.Models
{
    public class PasswordChangeModel
    {
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[!@.#$%^&*()_+])[A-Za-z\d][A-Za-z\d!@.#$%^&*()_+]{6,}$", ErrorMessage = "Password must be at least 6 characters long and must contain at least 1 digit, 1 letter and 1 special character.")]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[!@.#$%^&*()_+])[A-Za-z\d][A-Za-z\d!@.#$%^&*()_+]{5,}$", ErrorMessage = "Password must be at least 6 characters long and must contain at least 1 digit, 1 letter and 1 special character.")]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "New password does not match Confirm password.")]
        public string ConfirmPassword { get; set; }
    }

    public class UserLoginModel
    {
        [Required]
        [Display(Name = "Employee ID")]
        [RegularExpression(@"([1-9][0-9]*)", ErrorMessage = "Employee ID must contain only digits.")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[!@.#$%^&*()_+])[A-Za-z\d][A-Za-z\d!@.#$%^&*()_+]{5,}$", ErrorMessage = "Password must be at least 6 characters long and must contain at least 1 digit, 1 letter and 1 special character.")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Log in Role")]
        public String Role { get; set; }

        //[Display(Name = "Remember me?")]
        //public bool RememberMe { get; set; }
    }

    public class UserRegisterModel
    {
        [Required]
        [Display(Name = "Name")]
        [RegularExpression(@"[A-Za-z\s]+", ErrorMessage="Name should contain only letters.")]
        [MaxLength(30, ErrorMessage = "Name can have only 30 characters.")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Date of Joining")]
        [DataType(DataType.Date)]
        public System.DateTime DateOfJoining { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Email is not valid.")]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [StringLength(10, ErrorMessage = "Phone number can have only 10 digits")]
        [MaxLength(10, ErrorMessage = "Phone number can have only 10 digits")]
        [RegularExpression(@"[1-9][0-9]*", ErrorMessage="Phone number should contain only numbers.")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Employee ID")]
        [RegularExpression(@"([1-9][0-9]*)", ErrorMessage = "Employee ID must contain only digits.")]
        [StringLength(5, ErrorMessage = "Employee ID can have only 5 digits")]
        [MaxLength(5, ErrorMessage = "Employee ID can have only 5 digits")]
        public string EmployeeID { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}