using FilmCatalog.Data;
using FilmCatalog.Models;
using FilmCatalog.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FilmCatalog.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext dbContext;
        private readonly UserManager<User> userManager;
        private readonly ILogger<HomeController> logger;
        private IWebHostEnvironment appEnvironment;

        public HomeController(ApplicationDbContext dbContext,
                              UserManager<User> userManager,
                              ILogger<HomeController> logger,
                              IWebHostEnvironment webHostEnvironment)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.logger = logger;
            this.appEnvironment = webHostEnvironment;
        }


        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 8;
            var movies = await dbContext.Movies.Include(x => x.Poster).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            foreach (var movie in movies)
            {
                movie.Poster.Path = ChekPoster(movie);
            }
            var moviesCount = await dbContext.Movies.CountAsync();
            IndexViewModel viewModel = new IndexViewModel();
            viewModel.Movies = movies;
            viewModel.PageViewModel = new PageViewModel(moviesCount, page, pageSize);
            return View(viewModel);
        }

        private string ChekPoster(Movie movie)
        {
            if (System.IO.File.Exists(appEnvironment.WebRootPath + movie.Poster.Path))
                return movie.Poster.Path;

            else
                return "/Files/NotFound.jpeg";
        }
        public async Task<IActionResult> Info(int id)
        {
            var movie = dbContext.Movies.Include(x => x.Poster).Where(x => x.Id == id).FirstOrDefault();
            InfoViewModel info = new InfoViewModel();
            info.Movie = movie;
            var user = await userManager.GetUserAsync(User);

            if (movie != null && user?.Id == movie.UserId)
            {
                movie.Poster.Path = ChekPoster(movie);
                info.CanEdit = true;
            }
            return View(info);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }


}
