using AutoMapper;
using BLL;
using BLL.Dto;
using DAL.Repositories;
using Main.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Areas.Controllers
{
    [Area("Admin")]
    public class NoteController : Controller
    {
        private readonly INoteService noteService;
        private readonly IStore store;
        private readonly Mapper mapper;

        public NoteController(INoteService noteService, IStore store, Mapper mapper)
        {
            this.noteService = noteService;
            this.store = store;
            this.mapper = mapper;
        }

        [Route("/admin/notes")]
        public ActionResult Index()
        {
            return View();
        }

        // GET: NoteController/Details/5
        public ActionResult Details(Guid id)
        {

            return View();
        }

        // GET: NoteController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NoteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: NoteController/Edit/5
        public ActionResult Edit(Guid? id = null)
        {
            NoteViewModel viewModel = default;

            if (id.HasValue)
            {
                var dto = noteService.GetById(id.Value);
                viewModel = mapper.Map<NoteDto, NoteViewModel>(dto);
            }
            else
            {
                viewModel = new NoteViewModel
                {
                    IsNew = true,
                    IsOtherFile = true,
                };
            }

            if (IsAjax(Request))
                return PartialView(viewModel);

            return View(viewModel);
        }

        // POST: NoteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: NoteController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: NoteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        bool IsAjax(HttpRequest req) => req.Headers["X-Requested-With"] == "XMLHttpRequest";
    }
}
