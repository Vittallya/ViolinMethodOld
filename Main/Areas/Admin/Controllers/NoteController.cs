using AutoMapper;
using BLL;
using BLL.Comparers;
using BLL.Dto;
using DAL.Models;
using DAL.Repositories;
using Main.ViewModels;
using Main.ViewModels.Note;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Main.Areas.Controllers
{
    [Area("Admin")]
    public class NoteController : Controller
    {
        private readonly INoteService noteService;
        private readonly IStore store;
        private readonly Mapper mapper;
        private readonly IWebHostEnvironment env;
        private readonly PdfService pdfService;

        public NoteController(INoteService noteService, IStore store, Mapper mapper, IWebHostEnvironment env, PdfService pdfService)
        {
            this.noteService = noteService;
            this.store = store;
            this.mapper = mapper;
            this.env = env;
            this.pdfService = pdfService;
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
            //model.TotalCount = store.Notes.GetCount();

            return View(model);
        }

        [HttpGet]
        public ActionResult GetFilterView()
        {
            FiltersAllViewModel model = new FiltersAllViewModel();
            IEnumerable<Priem> priems = store.Priems.GetAll();
            var groups = priems.
                GroupBy(x => x.Group, new GenericComparer<PriemGroup>(x => x.Id));

            model.PriemGroups = groups.Select(y =>
            {
                PriemGroupViewModel vm = mapper.Map<PriemGroup, PriemGroupViewModel>(y.Key);
                vm.Priems = y.Select(x => mapper.Map<Priem, PriemViewModel>(x)).ToList();
                return vm;
            }).ToList();

            if (IsAjax(Request))
                return PartialView("FilterView", model);

            return View("FilterView", model);
        }

        public ActionResult GetNotes(IndexViewModel model, FilterViewModel filter = null)
        {
            int skip = (model.CurrentPage - 1) * model.TakeCount;

            IEnumerable<Guid> noteGuids = filter?.SelectedGuids;
            IEnumerable<int> priems = filter?.SelectedPriems;

            var notes = noteService.
                GetNotes(model.TakeCount, skip, noteGuids, priems).
                Select(x => mapper.Map<NoteDto, NoteViewModel>(x));

            model.Notes = notes;

            if(filter == null)
            {
                model.TotalCount = store.Notes.GetCount();
            }
            else
            {
                model.TotalCount = notes.Count();
            }

            if (IsAjax(Request))
                return PartialView(model.SelectedView, model);

            return View(model.SelectedView, model);
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

            viewModel.AllPriems = store.Priems.GetAll().Select(x => mapper.Map<Priem, PriemDto>(x));

            if (IsAjax(Request))
                return PartialView(viewModel);

            return View(viewModel);
        }


        // POST: NoteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Edit(NoteViewModel item)
        {
            bool isAjax = IsAjax(Request);

            if (!item.IsNew && !item.IsOtherFile && item.File == null)
            {
                ModelState.Remove("File");
            }


            if (item.File != null && Path.GetExtension(item.File.FileName) != ".pdf")
            {
                ModelState.AddModelError("", "Файл должен быть в формате .pdf");
            }


            else if (ModelState.IsValid)
            {
                var info = JsonConvert.DeserializeObject<PageInfoDto[]>(item.PageInfoJson);
                item.PageInfo = info.ToList();
                var dto = mapper.Map<NoteViewModel, NoteDto>(item);

                if (item.File != null)
                {
                    dto.FileName = item.File.FileName;
                }

                noteService.InsertOrUpdate(dto, item.Id);

                if(item.File != null)
                {
                    string folderName = dto.Id.ToString();

                    string path = Path.Combine(env.WebRootPath, "notefiles\\", folderName + "\\", dto.FileName);

                    string folderPath = Path.Combine(env.WebRootPath, "notefiles\\" + folderName + "\\");
                    string imgPath = Path.Combine(folderPath, "images\\");
                    string imgFullName = Path.Combine(imgPath, dto.ShowPageNumber + ".jpg");

                    if (!Directory.Exists(imgPath))
                    {
                        Directory.CreateDirectory(imgPath);
                    }

                    using var str = System.IO.File.Create(path);
                    item.File.CopyTo(str);
                    using var imgStr = System.IO.File.Create(imgFullName);
                    pdfService.UnwrapImage(str, dto.ShowPageNumber, imgStr);
                }
                else if (!item.IsNew && store.Notes.FindById(dto.Id).ShowPageNumber != dto.ShowPageNumber)
                {
                    string path = Path.Combine(env.WebRootPath, "notefiles\\", dto.Id + "\\", dto.FileName);
                    string imgPath = Path.Combine(env.WebRootPath, "notefiles\\", dto.Id + "\\", "images\\" + dto.ShowPageNumber + ".jpg");

                    using FileStream str = System.IO.File.OpenRead(path);
                    using FileStream imgStr = System.IO.File.OpenWrite(imgPath);
                    pdfService.UnwrapImage(str, dto.ShowPageNumber, imgStr,  System.Drawing.Imaging.ImageFormat.Jpeg);
                }


                if (isAjax)
                {
                    return Ok();
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            if (isAjax)
            {
                return ValidationProblem(ModelState);
            }
            else
            {
                return View(item);
            }
        }

        public ActionResult Delete(Guid id)
        {
            try
            {
                store.Notes.Delete(id);
                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

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
