using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmCatalog.ViewModel
{
    public class AddEditViewModel
    {
        public int Id { get; set; }

        [Display(Name ="Название")]
        [Required(ErrorMessage ="не указано название фильма")]
        [StringLength(128, MinimumLength =2,ErrorMessage ="Длина должна быть от 2 до 128 символов")]
        public string Name { get; set; }

        [Display(Name ="Описание"]
        [Required(ErrorMessage ="не указано описание фильма")]
        [StringLength(4096,MinimumLength =10,ErrorMessage = "Длина должна быть от 10 до 4096 символов")]
        public string Description { get; set; }

        [Display(Name = "Год выхода")]
        [Required(ErrorMessage = "Не указан год выхода")]
        [Range(1895, 3000, ErrorMessage = "Недопустимый год, начало с 1895 года")]
        public int YearOfIssue { get; set; }

        [Display(Name = "Режиссёр")]
        [Required(ErrorMessage = "Не указан режиссёр фильма")]
        [StringLength(256, MinimumLength = 3, ErrorMessage = "Длина должна быть от 3 до 256 символов")]
        public string Director { get; set; }
        public bool IsEdit { get; set; }

        [Display(Name = "Постер")]
        [DataType(DataType.Upload)]
        public IFormFile Poster { get; set; }

        public byte[] PosterArr { get; set; }
    }



}
