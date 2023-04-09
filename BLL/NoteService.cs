using AutoMapper;
using BLL.Dto;
using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using PdfiumViewer;
using LiteDB;

namespace BLL
{
    public class NoteService : INoteService
    {
        private readonly IStore store;
        private readonly Mapper mapper;
        private readonly PdfService pdfService;

        public NoteService(IStore store, Mapper mapper, PdfService pdfService)
        {
            this.store = store;
            this.mapper = mapper;
            this.pdfService = pdfService;
        }

        public NoteDto GetById(Guid id)
        {
            var repo = store.Notes;
            var note = ((LiteDbRepo<Note>)repo).GetNoteWithIncluded(id);
            return mapper.Map<Note, NoteDto>(note);
        }

        public IEnumerable<NoteDto> GetNotes(int take,
                                             int skip,
                                             IEnumerable<Guid> noteIds = null,
                                             IEnumerable<int> priems = null,
                                             int? groupId = null)
        {
            if(store.Notes is LiteDbRepo<Note> repo)
            {
                var notes = repo.GetNotes(take, skip, noteIds, priems, groupId).ToList();
                return notes.Select(x => mapper.Map<Note, NoteDto>(x));
            }
            throw new ArgumentException("choosen store that isn`t lite database");
        }
        public int GetNotesCount(IEnumerable<int> priems = null)
        {
            if(store.Notes is LiteDbRepo<Note> repo)
            {
                var notesCount = repo.GetNotesCount(priems);
                return notesCount;
            }
            throw new ArgumentException("choosen store that isn`t lite database");
        }

        public void InsertOrUpdate(NoteDto model, Guid? guid = null)
        {
            model.Id = guid ?? Guid.NewGuid();

            if (guid.HasValue)
            {
                model.LastUpdate = DateTime.UtcNow;
            }
            else
            {
                model.PublishDate = DateTime.UtcNow;
            }

            var note = mapper.Map<NoteDto, Note>(model);
            store.Notes.InsertOrUpdate(note);
        }

    }
}
