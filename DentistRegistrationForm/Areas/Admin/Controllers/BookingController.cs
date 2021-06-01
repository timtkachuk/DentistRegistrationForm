using DentistRegistrationFormData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentistRegistrationForm.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Administrators, Doctors")]
    public class BookingController : Controller
    {
        private readonly AppDbContext context;

        public BookingController(
            AppDbContext context
            )
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {

            var model = await context.Bookings.Where(p =>p.BookingState == BookingStates.New).OrderBy(p => p.dateTime).ToListAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveBooking(int id)
        {
            var model = await context.Bookings.FindAsync(id);

            model.BookingState = BookingStates.Approved;

            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var model = await context.Bookings.FindAsync(id);

            model.BookingState = BookingStates.Cancelled;

            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
