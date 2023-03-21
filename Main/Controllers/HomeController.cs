using AutoMapper;
using BLL;
using BLL.Comparers;
using BLL.Dto;
using DAL.Models;
using DAL.Repositories;
using Main.ViewModels;
using Main.ViewModels.Note;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStore store;
        private readonly Mapper map;
        private readonly INoteService noteService;

        public HomeController(IStore store, Mapper map, INoteService noteService)
        {
            this.store = store;
            this.map = map;
            this.noteService = noteService;
        }

        public IActionResult Index()
        {
            ViewData.Add("Title", "Скрипичные приемы");
            return RedirectToAction(nameof(List));
        }

        public ActionResult Details(Guid id)
        {
            var noteDto = noteService.GetById(id);
            var noteVm = map.Map<NoteDto, NoteViewModel>(noteDto);

            if (IsAjax(Request))
                return PartialView(noteVm);

            return View(noteVm);
        }

        public ActionResult GetAllGropus()
        {


            var notes = store.Notes.GetAll();

            var groups = notes.
                SelectMany
                (x => x.PageInfo.
                SelectMany(x => x.Priems.Select(x => map.Map<PriemGroup, PriemGroupViewModel>(x.Group)))).
                Distinct(new GenericComparer<PriemGroupViewModel>(x => x.Id));


            if (IsAjax(Request))
                return PartialView("ListGroup", new FiltersAllViewModel { PriemGroups = groups });

            return View("ListGroup", new FiltersAllViewModel { PriemGroups = groups });
        }

        //public ActionResult GetNotesByGroup(int groupId)
        //{
        //    var notes = store.Notes.get
        //}

        public ActionResult List()
        {
            IndexViewModel model = new IndexViewModel
            {
                CurrentPage = 1,
                SelectedView = "/Views/Home/TileView.cshtml",
                TakeCount = 15,
                Views = new string[] { "TableView", "TileView" },
                TotalCount = store.Notes.GetCount()
            };

            return View(model);
        }
        bool IsAjax(HttpRequest req) => req.Headers["X-Requested-With"] == "XMLHttpRequest";
    }
}
