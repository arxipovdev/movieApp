using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class SignUpViewModel
    {
        // [Required(ErrorMessage = "Обязательное поле")]
        [EmailAddress(ErrorMessage = "email не валидный")]
        // [DataType(DataType.EmailAddress)]
        [Display(Name = "Email*")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Имя*")]
        public string FirstName { get; set; }
        
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
        
        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }
        
        [Required(ErrorMessage = "Обязательное поле")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль*")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Обязательное поле")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совподают")]
        [Display(Name = "Подтвердить пароль*")]
        public string PasswordConfirm { get; set; }
    }
}