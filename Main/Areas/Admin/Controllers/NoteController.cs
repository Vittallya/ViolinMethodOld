using AutoMapper;
using BLL;
using BLL.Dto;
using DAL.Models;
using DAL.Repositories;
using Main.ViewModels;
using Main.ViewModels.Note;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

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

        [Route("/admin/note")]
        public ActionResult Index()
        {
            IndexViewModel model = new IndexViewModel
            {
                CurrentPage = 1,
                SelectedView = "TableView",
                TakeCount = 15,
                Views = new string[] { "TableView", "TileView" }
            };
            model.TotalCount = store.Notes.GetCount();

            return View(model);
        }

        public ActionResult GetNotes(IndexViewModel model)
        {
            int skip = (model.CurrentPage - 1) * model.TakeCount;

            IEnumerable<Guid> noteGuids = model?.Filter?.SelectedGuids;
            IEnumerable<int> priems = model?.Filter?.SelectedPriems;

            var notes = noteService.
                GetNotes(model.TakeCount, skip, noteGuids, priems).
                Select(x => mapper.Map<NoteDto, NoteViewModel>(x));

            if (IsAjax(Request))
                return PartialView(model.SelectedView, notes);

            return View(model.SelectedView, notes);
        }

        // GET: NoteController/Details/5
        public ActionResult Details(Guid id)
        {

            return View();
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

            viewModel.AllPriems = new PriemDto[]
            {
                new PriemDto
                {
                    Id = 1,
                    Name = "Прием1",
                    Group = new PriemGroupDto()
                    {
                        Id = 1,
                        Name = "Группа1"
                    }
                },

                new PriemDto
                {
                    Id = 2,
                    Name = "Прием2",
                    Group = new PriemGroupDto()
                    {
                        Id = 1,
                        Name = "Группа1"
                    }
                },

                new PriemDto
                {
                    Id = 3,
                    Name = "Прием3",
                    Group = new PriemGroupDto()
                    {
                        Id = 2,
                        Name = "Группа2"
                    }
                },
            };

            //viewModel.AllPriems = store.Priems.GetAll().Select(x => mapper.Map<Priem, PriemDto>(x));

            if (IsAjax(Request))
                return PartialView(viewModel);

            return View(viewModel);
        }

        // POST: NoteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NoteViewModel item)
        {
            var info = JsonConvert.DeserializeObject<PageInfoDto[]>(item.PageInfoJson);

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

        [HttpPost]
        public ActionResult Test(PageInfoDto dto)
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
