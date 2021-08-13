using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmCatalog.Models
{
    public class SignUpViewModel
    {
        [EmailAddress(ErrorMessage = "Необходимо указать корректный email")]
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
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Подтвердить пароль*")]
        public string PasswordConfirm { get; set; }

    }
}
