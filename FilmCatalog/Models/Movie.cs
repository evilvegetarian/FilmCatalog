﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmCatalog.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(128), Display(Name = "Название")]
        public string Name { get; set; }

        [Required, MaxLength(4096), Display(Name = "Описание")]
        public string Description { get; set; }

        [Required, Display(Name = "Год издания")]
        public int YearOfIssue { get; set; }

        [Required, MaxLength(256), Display(Name = "Режиссёр")]
        public string Director { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public int PosterId { get; set; }

        [ForeignKey("PosterId")]
        public Poster Poster { get; set; }
    }
}
