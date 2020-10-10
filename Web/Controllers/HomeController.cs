using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
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
        public async Task<IActionResult> Index()
        {
            var movies = await _movieRepo.GetAll();
            return View( _mapper.Map<IEnumerable<MovieViewModel>>(movies));
        }
        
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var producers = await _producerRepo.GetAll();
            ViewBag.producers = _mapper.Map<IEnumerable<ProducerViewModel>>(producers);
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(MovieViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            
            //save file
            model.Post = await SaveFile(model.File);
            var created = await _movieRepo.Create(_mapper.Map<Movie>(model));
            if (!created) return NotFound();
            return RedirectToAction(nameof(Index));
        }
        
        [AllowAnonymous]
        public async Task<ActionResult> Details(int id)
        {
            var movie = await _movieRepo.GetById(id);
            if(movie == null) return NotFound();
            var producers = await _producerRepo.GetAll();
            ViewBag.producers = _mapper.Map<IEnumerable<ProducerViewModel>>(producers);
            return View(_mapper.Map<MovieViewModel>(movie));
        }
        
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var producers = await _producerRepo.GetAll();
            ViewBag.producers = _mapper.Map<IEnumerable<ProducerViewModel>>(producers);
            var movie = await _movieRepo.GetById(id);
            if(movie == null) return NotFound();
            return View(_mapper.Map<MovieEditViewModel>(movie));
        }
        
        [HttpPost]
        public async Task<ActionResult> Edit(int id, MovieEditViewModel model)
        {
            var producers = await _producerRepo.GetAll();
            ViewBag.producers = _mapper.Map<IEnumerable<ProducerViewModel>>(producers);
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            var movie = await _movieRepo.GetById(id);
            if (!_movieRepo.CheckUser(movie))
            {
                ModelState.AddModelError(string.Empty, "Редактировать может только тот кто создавал");
                return View(model);
            }

            _mapper.Map(model, movie);
            var updated = await _movieRepo.Update(movie);
            if (!updated) return NotFound();
            return RedirectToAction(nameof(Index));
        }
        
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _movieRepo.Delete(id);
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