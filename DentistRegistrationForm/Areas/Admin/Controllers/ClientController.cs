﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentistRegistrationForm.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}