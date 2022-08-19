using System.ComponentModel.DataAnnotations;

namespace WebH.Models
{
    public class RegisterBindingModel
    {
        [Display(Name = "Login")]
        [Required(ErrorMessage = "Пожалуйста, введите  логин.")]
        [UIHint("Login")]
        public string Login { get; set; }

        [Display(Name = "Password")]
        [UIHint("Password")]
        [Required(ErrorMessage = "Пожалуйста, введите  пароль.")]
        [StringLength(50, ErrorMessage = "Длина должна быть от 6 до 50 символов", MinimumLength = 6)]
        public string Password { get; set; }

        [Display(Name = "PasswordConfirm")]
        [UIHint("Password")]
        [Required(ErrorMessage = "Пожалуйста, введите  пароль.")]
        [Compare("Password")]
        [StringLength(50, ErrorMessage = "Длина должна быть от 6 до 50 символов", MinimumLength = 6)]
        public string PasswordConfirm { get; set; }
    }

    public class LoginBindingModel
    {
        [UIHint("Login")]
        [Required(ErrorMessage = "Пожалуйста, введите  логин.")]
        public string Login { get; set; }


        [Required(ErrorMessage = "Пожалуйста, введите  пароль.")]
        [UIHint("Password")]
        [StringLength(50, ErrorMessage = "Длина должна быть от 6 до 50 символов", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
