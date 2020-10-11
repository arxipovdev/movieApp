using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Models;
using Web.Repositories;
using Web.ViewModels;

namespace Movies.Web.Controllers
{
    [Authorize]
    public class ProducerController : Controller
    {
        private readonly ILogger<ProducerController> _logger;
        private readonly IMapper _mapper;
        private readonly IProducerRepository _repository;

        public ProducerController(ILogger<ProducerController> logger, IMapper mapper, IProducerRepository repository)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var producers = await _repository.GetAllAsync();
            return View( _mapper.Map<IEnumerable<ProducerViewModel>>(producers));
        }
        
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(ProducerViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var created = await _repository.CreateAsync(_mapper.Map<Producer>(model));
            if (!created) return NotFound();
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<ActionResult> Details(int id)
        {
            var producer = await _repository.GetByIdAsync(id);
            if(producer == null) return NotFound();
            return View(_mapper.Map<ProducerViewModel>(producer));
        }
        
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
             return await Details(id);
        }
        
        [HttpPost]
        public async Task<ActionResult> Edit(int id, ProducerViewModel model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            var producer = await _repository.GetByIdAsync(id);
            if (!_repository.CheckUser(producer))
            {
                ModelState.AddModelError(string.Empty, "Редактировать может только тот кто создавал");
                return View(model);
            }
            _mapper.Map(model, producer);
            var updated = await _repository.UpdateAsync(producer);
            if (!updated) return NotFound();
            return RedirectToAction(nameof(Index));
        }
        
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _repository.DeleteAsync(id);
            if (!deleted) return NotFound();
            return RedirectToAction(nameof(Index));
        }
    }
}