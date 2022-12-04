using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Areas.Admin.Controllers
{
    [Area("admin")]
    public class PriemController : Controller
    {
        private readonly IStore store;

        public PriemController(IStore store)
        {
            this.store = store;
        }

        [Route("/admin/priem")]
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetPriems()
        {
            return Json(store.Priems.GetAllClear());
        }

        public JsonResult GetGroups()
        {
            return Json(store.PriemGroups.GetAllClear());
        }
    }
}
