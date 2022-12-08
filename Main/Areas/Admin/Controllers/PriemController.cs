using AutoMapper;
using BLL.Dto;
using DAL.Models;
using DAL.Repositories;
using Main.ViewModels;
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
        private readonly Mapper map;

        public PriemController(IStore store, Mapper map)
        {
            this.store = store;
            this.map = map;
        }

        [Route("/admin/priem")]
        public IActionResult Index()
        {
            return View();
        }


        public ActionResult EditGroup(int? id = null)
        {
            var group = new PriemGroupViewModel();

            if (id.HasValue)
            {
                group = map.Map<PriemGroup, PriemGroupViewModel>(store.PriemGroups.FindById(id.Value));
            }

            return View(group);
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGroup(PriemGroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var item = map.Map<PriemGroupViewModel, PriemGroup>(model);

                store.PriemGroups.InsertOrUpdate(item);
                return Ok();
            }

            return ValidationProblem(ModelState);
        }

        public ActionResult EditPriem(int? id = null)
        {
            var priem = new PriemViewModel();

            if (id.HasValue)
            {
                var orig = store.Priems.FindById(id.Value);
                priem = map.Map<Priem, PriemViewModel>(orig);
                priem.GroupId = orig.Group.Id;
            }

            priem.Groups = store.PriemGroups.GetAllClear().
                Select(x => map.Map<PriemGroup, PriemGroupViewModel>(x));

            return View(priem);
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPriem(PriemGroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var item = map.Map<PriemGroupViewModel, PriemGroup>(model);

                store.PriemGroups.InsertOrUpdate(item);
                return Ok();
            }

            return ValidationProblem(ModelState);
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
