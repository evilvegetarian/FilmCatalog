using FilmCatalog.Models;
using System.Collections.Generic;

namespace FilmCatalog.ViewModel
{
    public class IndexViewModel
    {
        public IEnumerable<Movie> Movies { get; set; }
        public PageViewModel PageViewModel { get; set; }

    }
}
