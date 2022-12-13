using AutoMapper;
using BLL.Dto;
using DAL.Models;
using DAL.Repositories;
using Main.ViewModels;
using Microsoft.AspNetCore.Http;
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

            if (IsAjax(Request))
                return PartialView(group);

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

                if (IsAjax(Request))
                    return Ok();
                return RedirectToAction("Index");
            }
            if (IsAjax(Request))
                return ValidationProblem(ModelState);
            return View(model);
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
                Select(x => map.Map<PriemGroup, PriemGroupViewModel>(x)).ToList();

            if (IsAjax(Request))
                return PartialView(priem);

            return View(priem);
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPriem(PriemViewModel model)
        {
            if (ModelState.IsValid)
            {
                var item = map.Map<PriemViewModel, Priem>(model);
                item.Group = new PriemGroup { Id = model.GroupId };
                store.Priems.InsertOrUpdate(item);
                if (IsAjax(Request))
                    return Ok(item.Id);
                return RedirectToAction("Index");
            }
            
            if (IsAjax(Request))
                return ValidationProblem(ModelState);

            model.Groups = store.PriemGroups.GetAllClear().
                Select(x => map.Map<PriemGroup, PriemGroupViewModel>(x)).ToList();
            return View(model);
        }

        public JsonResult GetPriems()
        {
            return Json(store.Priems.GetAllClear());
        }

        public JsonResult GetPriemsAll()
        {
            return Json(store.Priems.GetAll());
        }

        public JsonResult GetGroups()
        {
            return Json(store.PriemGroups.GetAllClear());
        }

        [Route("admin/priem/deletePriem/{id}")]
        public ActionResult DeletePriem(int id)
        {
            try
            {
                store.Priems.Delete(id);
                return Ok();
            }
            catch(Exception e)
            {
                return ValidationProblem(e.Message);
            }
        }

        [Route("admin/priem/deletegroup/{id}")]
        public ActionResult DeleteGroup(int id)
        {
            try
            {
                store.PriemGroups.Delete(id);
                return Ok();
            }
            catch(Exception e)
            {
                return ValidationProblem(e.Message);
            }
        }

        bool IsAjax(HttpRequest req) => req.Headers["X-Requested-With"] == "XMLHttpRequest";
    }
}
