using FilmCatalog.Data;
using FilmCatalog.Enums;
using FilmCatalog.Helpers;
using FilmCatalog.Models;
using FilmCatalog.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FilmCatalog.Controllers
{
    public class MovieController : Controller
    {
        private ApplicationDbContext dbContext;
        private UserManager<User> userManager;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly int fileSize;



        public MovieController(ApplicationDbContext dbContext,
                               UserManager<User> userManager,
                               IWebHostEnvironment webHostEnvironment, 
                               IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            fileSize = _configuration.GetValue<int>("PosterSize");
        }

        [HttpGet]
        public async Task<IActionResult> AddEdit(PageInteractionType interactionType, int id)
        {
            if (interactionType == PageInteractionType.Add)
            {
                return View(new AddEditViewModel());
            }
            else if (interactionType == PageInteractionType.Edit)
            {
                var movie = dbContext.Movies.Include(x => x.Poster).Where(x => x.Id == id).FirstOrDefault();

                if (movie is null)
                {
                    return View("NotFount");
                }
                else
                {
                    var user = await userManager.GetUserAsync(User);
                    if (movie.UserId == user.Id)
                    {
                        byte[] imageDate = null;
                        var posterPath = webHostEnvironment.WebRootPath + movie.Poster.Path;
                        if (System.IO.File.Exists(posterPath))
                        {
                            imageDate = await System.IO.File.ReadAllBytesAsync(posterPath);
                        }
                        else
                        {
                            imageDate = await System.IO.File.ReadAllBytesAsync(webHostEnvironment.WebRootPath + "/Files/NotFound.jpeg");
                        }
                        var addEditViewModel = new AddEditViewModel()
                        {
                            Name = movie.Name,
                            Description = movie.Description,
                            YearOfIssue = movie.YearOfIssue,
                            Director = movie.Director,
                            PosterArr = imageDate,
                            IsEdit = true
                        };
                        return View(addEditViewModel);
                    }
                    else
                    {
                        return Forbid("Отказано в доступе");
                    }
                }
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddEdit(AddEditViewModel viewModel)
        {
            var user = await userManager.GetUserAsync(User);

            if (ModelState.IsValid && user != null)
            {
                Poster file = new Poster();
                byte[] imageDate = null;
                if (viewModel?.Poster != null)
                {
                    imageDate = FormFileExtensions.GetImageByteArr(viewModel.Poster);
                    if (FormFileExtensions.ValidateImageSize(viewModel.Poster, fileSize))
                    {
                        ModelState.AddModelError("Poster", $"Размер файла не должен превышать {fileSize} МБ.");
                        return View(viewModel);
                    }
                    if (FormFileExtensions.ValidateImageExtension(viewModel.Poster))
                    {
                        ModelState.AddModelError("Poster", $"Неправильный тип файла");
                        return View(viewModel);
                    }
                    if (FormFileExtensions.ValidateImageExtension(viewModel.Poster))
                    {
                        ModelState.AddModelError("Poster", $"Не получается прочитать файл.");
                        return View(viewModel);
                    }
                    string path = "/Files/" + Guid.NewGuid() + viewModel.Poster.FileName;
                    using (var fileStream = new FileStream(webHostEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await viewModel.Poster.CopyToAsync(fileStream);
                    }
                    file.Name = viewModel.Poster.FileName;
                    file.Path = path;

                }
                else
                {
                    string name = "NotFound.jpg";
                    string path = $"{webHostEnvironment.WebRootPath}/Files/{name}";
                    if (System.IO.File.Exists(path))
                    {
                        file.Name = name;
                        file.Path = $"/Files/{name}";
                    }
                    else
                    {
                        file.Name = string.Empty;
                        file.Path = string.Empty;
                    }
                }


                if (viewModel.IsEdit)
                {
                    var movie = dbContext.Movies.Include(x => x.Poster).FirstOrDefault(x => x.Id == viewModel.Id);
                    if (movie is null)
                    {
                        return BadRequest();
                    }
                    else if (movie.User.Id==user.Id)
                    {
                        movie.Name = viewModel.Name;
                        movie.Description = viewModel.Description;
                        movie.YearOfIssue = viewModel.YearOfIssue;
                        movie.Director = viewModel.Director;
                        movie.User = user;

                        var path = webHostEnvironment.WebRootPath + movie.Poster.Path;
                        if (!string.IsNullOrEmpty(file.Name)&& file.Name !="NotFound.jpeg")
                        {
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                            movie.Poster = file;
                        }
                        dbContext.Movies.Add(movie);
                    }
                    else
                    {
                        return Forbid("Отказано в доступе.");
                    }
                    
                }
                else
                {
                    dbContext.Movies.Add(new Movie()
                    {
                        Name = viewModel.Name,
                        Description = viewModel.Description,
                        YearOfIssue = viewModel.YearOfIssue,
                        Director = viewModel.Director,
                        User = user,
                        Poster = file
                    });
                }
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(viewModel);
            }

        }
    }
}
