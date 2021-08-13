using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmCatalog.Models
{
    public class SignInViewModel
    {
        [Required(ErrorMessage ="обязательное поле")]
        [Display(Name ="email")]
        public string  Email { get; set; }

        [Required(ErrorMessage ="Обязательное поле")]
        [DataType(DataType.Password)]
        [Display(Name ="Пароль")]
        public string Password { get; set; }

        [Display(Name ="запомнить")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }



    }

}
