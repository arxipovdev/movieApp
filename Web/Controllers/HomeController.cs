using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Models;
using Web.Repositories;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper _mapper;
        private IWebHostEnvironment _enviroment;
        private readonly IMovieRepository _movieRepo;
        private readonly IProducerRepository _producerRepo;

        public HomeController(ILogger<HomeController> logger,
            IMapper mapper,
            IWebHostEnvironment environment,
            IMovieRepository movieRepo,
            IProducerRepository producerRepo)
        {
            _logger = logger;
            _mapper = mapper;
            _enviroment = environment;
            _movieRepo = movieRepo;
            _producerRepo = producerRepo;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(int page = 1)
        {
            var sizePage = 8;
            var movies = await _movieRepo.GetPaginateAsync(page, sizePage);
            var count = await _movieRepo.CountAsync();
            var data = _mapper.Map<IEnumerable<MovieViewModel>>(movies).ToList();
            var currentUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            data.ForEach(x =>
            {
                var userId = movies.First(movie => movie.Id == x.Id).UserId;
                x.IsEdited = currentUserId == userId;
            });
            var viewModel = new MovieIndexViewModel(page, sizePage, count, data);
            return View(viewModel);
        }
        
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var producers = await _producerRepo.GetAllAsync();
            ViewBag.producers = _mapper.Map<IEnumerable<ProducerViewModel>>(producers);
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(MovieViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            
            //save file
            model.Post = await SaveFile(model.File);
            var created = await _movieRepo.CreateAsync(_mapper.Map<Movie>(model));
            if (!created) return NotFound();
            return RedirectToAction(nameof(Index));
        }
        
        [AllowAnonymous]
        public async Task<ActionResult> Details(int id)
        {
            var movie = await _movieRepo.GetByIdAsync(id);
            if(movie == null) return NotFound();
            var producers = await _producerRepo.GetAllAsync();
            ViewBag.producers = _mapper.Map<IEnumerable<ProducerViewModel>>(producers);
            var viewModel = _mapper.Map<MovieViewModel>(movie);
            var currentUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            viewModel.IsEdited = currentUserId == movie.UserId;
            return View(viewModel);
        }
        
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var producers = await _producerRepo.GetAllAsync();
            ViewBag.producers = _mapper.Map<IEnumerable<ProducerViewModel>>(producers);
            var movie = await _movieRepo.GetByIdAsync(id);
            if(movie == null) return NotFound();
            if (!_movieRepo.CheckUser(movie)) return RedirectToAction("Index");
            return View(_mapper.Map<MovieEditViewModel>(movie));
        }
        
        [HttpPost]
        public async Task<ActionResult> Edit(int id, MovieEditViewModel model)
        {
            var producers = await _producerRepo.GetAllAsync();
            ViewBag.producers = _mapper.Map<IEnumerable<ProducerViewModel>>(producers);
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            var movie = await _movieRepo.GetByIdAsync(id);
            if (!_movieRepo.CheckUser(movie))
            {
                ModelState.AddModelError(string.Empty, "Редактировать может только тот кто создавал");
                return View(model);
            }

            _mapper.Map(model, movie);
            var updated = await _movieRepo.UpdateAsync(movie);
            if (!updated) return NotFound();
            return RedirectToAction(nameof(Index));
        }
        
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _movieRepo.DeleteAsync(id);
            if (!deleted) return NotFound();
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null) return BadRequest();
            var fileName = await SaveFile(file);
            return Json(new { fileName });
        }
        
        private async Task<string> SaveFile(IFormFile file)
        {
            var fileName = file.FileName;
            fileName = Guid.NewGuid().ToString() + fileName.Substring(fileName.LastIndexOf("."));
            var path = $"{_enviroment.WebRootPath}/uploads/{fileName}";
            using (var fileStrim = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(fileStrim);
            }

            return fileName;
        }
    }
}