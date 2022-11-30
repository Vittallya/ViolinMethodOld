using System.ComponentModel.DataAnnotations;

namespace Main.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [UIHint("password")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        [Display(Name = "Запомнить меня?")]
        public bool RememberMe { get; set; }
        [Required]
        [Display(Name = "Логин")]
        public string UserName { get; set; }
    }
}