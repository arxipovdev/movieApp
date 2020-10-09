using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IMovieRepository _movieRepo;
        private readonly IProducerRepository _producerRepo;

        public HomeController(ILogger<HomeController> logger, IMapper mapper, IMovieRepository movieRepo, IProducerRepository producerRepo)
        {
            _logger = logger;
            _mapper = mapper;
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
            var created = await _movieRepo.Create(_mapper.Map<Movie>(model));
            if (!created) return NotFound();
            return RedirectToAction(nameof(Index));
        }
        
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
            return await Details(id);
        }
        
        [HttpPost]
        public async Task<ActionResult> Edit(int id, MovieViewModel model)
        {
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}