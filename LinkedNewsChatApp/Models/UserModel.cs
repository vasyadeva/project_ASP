using System.ComponentModel.DataAnnotations;

namespace LinkedNewsChatApp.Models
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Введіть ім'я користувача!")]
        [RegularExpression("[A-Za-z0-9_.]+", ErrorMessage = "Ім'я користувача не може містити пробіли!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Ім'я користувача має містити 3-20 символів!")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Введіть електронну пошту!")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$",
            ErrorMessage = "Введіть згідно з стандарту!")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Введіть пароль!")]
        [RegularExpression(@"^((?=^.{8,}$)(?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$",
            ErrorMessage = "Пароль має містити принаймні 8 символів, містити принаймні одну велику літеру, " + "одну маленьку, число та спеціальний символ!")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Будь ласка, підтвердіть ваш пароль!")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Паролі не співпадають!")]
        public string? ConfirmPassword { get; set; }

        [Range(1, 10, ErrorMessage = "Значення має бути в межах від 1 до 10")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Введіть значення цілочисельного типу")]
        public int AvatarId { get; set; }
        [StringLength(100, ErrorMessage ="Досягнуто ліміт тексту!")]
        public string? Biography { get; set; }
        [Required(ErrorMessage = "Оберіть регіон!")]
        [StringLength(100, ErrorMessage = "Оберіть регіон!", MinimumLength =1)]
        public string Region { get; set; }

    }
}
