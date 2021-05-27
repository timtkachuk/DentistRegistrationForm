using DentistRegistrationFormData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentistRegistrationForm.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProcedureController : Controller
    {
        private readonly string entity = "Procedure";

        private readonly AppDbContext context;

        public ProcedureController(
            AppDbContext context
            )
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View(context.Procedures.OrderBy(p => p.Name).ToList());
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Procedure model)
        {
            context.Entry(model).State = EntityState.Added;
            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = $"{entity} successfully added.";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                TempData["error"] = $"The same name of {entity} exists.";
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = await context.Procedures.FindAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Procedure model)
        {
            context.Entry(model).State = EntityState.Modified;
            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = $"{entity} successfully edited.";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                TempData["error"] = $"The same name of {entity} exists.";
                return View(model);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var model = await context.Procedures.FindAsync(id);
            context.Entry(model).State = EntityState.Deleted;
            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = $"{entity} successfully deleted.";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                TempData["error"] = $"This {entity} connected to one or more entities.";
                return RedirectToAction("Index");
            }

        }
    }
}
