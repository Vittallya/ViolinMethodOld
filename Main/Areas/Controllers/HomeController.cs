﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Areas.Controllers
{
    [Area("admin")]
    public class HomeController: Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
