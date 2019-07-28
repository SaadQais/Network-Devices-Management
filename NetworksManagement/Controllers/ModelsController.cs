using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetworksManagement.Core;
using NetworksManagement.Data.Models;
using NetworksManagement.Data.ViewModels;

namespace NetworksManagement.Controllers
{
    public class ModelsController : Controller
    {
        private readonly IModelRepository _modelsRepository;
        private readonly ICategoriesRepository _categoriesRepository;

        [BindProperty]
        public DeviceModelViewModel ModelVM { get; set; }
        public ModelsController(IModelRepository modelsRepository, ICategoriesRepository categoriesRepository)
        {
            _modelsRepository = modelsRepository;
            _categoriesRepository = categoriesRepository;

            ModelVM = new DeviceModelViewModel
            {
                Model = new DeviceModel(),
                Models = new List<DeviceModel>(),
                Categories = _categoriesRepository.GetAll().ToList()
            };
        }

        public async Task<IActionResult> Index()
        {
            var models = await _modelsRepository.GetAll().ToListAsync();
            ModelVM.Models = models;

            ViewBag.Current = "Models";
            return View(ModelVM);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ModelVM.Model = await _modelsRepository.GetAsync(id);

            if (ModelVM.Model == null)
            {
                return NotFound();
            }

            return View(ModelVM);
        }

        public IActionResult Create()
        {
            return View(ModelVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name, IsCore,CategoryId")] DeviceModel model)
        {
            if (ModelState.IsValid)
            {
                await _modelsRepository.AddAsync(model);

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ModelVM.Model = await _modelsRepository.GetAsync(id);

            if (ModelVM.Model == null)
            {
                return NotFound();
            }
            return View(ModelVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name, IsCore,CategoryId")] DeviceModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _modelsRepository.UpdateAsync(model);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModelExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ModelVM.Model = await _modelsRepository.GetAsync(id);

            if (ModelVM.Model == null)
            {
                return NotFound();
            }

            return View(ModelVM);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var group = await _modelsRepository.GetAsync(id);

            await _modelsRepository.RemoveAsync(group);

            return RedirectToAction(nameof(Index));
        }

        private bool ModelExists(int id)
        {
            return _modelsRepository.Any(id);
        }
    }
}