﻿using Microsoft.AspNetCore.Identity;

namespace FilmCatalog.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
    }
}
