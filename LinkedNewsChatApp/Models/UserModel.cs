﻿using System.ComponentModel.DataAnnotations;

namespace LinkedNewsChatApp.Models
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be 3-20 characters long")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$",
            ErrorMessage = "Please enter valid email adress")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^((?=^.{8,}$)(?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$",
            ErrorMessage = "The password must be at least 8 characters long contain at least one uppercase " +
            "and one lowercase character, number and special character!")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
        public string? ConfirmPassword { get; set; }

        [Range(1, 10, ErrorMessage = "Value must be between 1 and 10")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Value must be an integer")]
        public int AvatarId { get; set; }
        [StringLength(100, ErrorMessage ="Value must be less than 100 symbols")]
        public string? Biography { get; set; }

        public string? Region { get; set; }
    }
}
