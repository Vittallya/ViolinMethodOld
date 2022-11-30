using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData.Add("Title", "Скрипичные приемы");
            return View();
        }
    }
}
