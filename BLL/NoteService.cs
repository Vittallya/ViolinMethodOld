﻿using AutoMapper;
using BLL.Dto;
using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace BLL
{
    public class NoteService : INoteService
    {
        private readonly IStore store;
        private readonly Mapper mapper;

        public NoteService(IStore store, Mapper mapper)
        {
            this.store = store;
            this.mapper = mapper;
        }

        public NoteDto GetById(Guid id)
        {
            var repo = store.Notes;
            var note = ((LiteDbRepo<Note>)repo).GetNoteWithIncluded(id);
            return mapper.Map<Note, NoteDto>(note);
        }

        public IEnumerable<NoteDto> GetNotes(int take, int skip, IEnumerable<Guid> noteIds = null, IEnumerable<int> priems = null)
        {
            throw new NotImplementedException();
        }

        public Task InsertOrUpdateAsync(NoteDto model, Guid? guid = null)
        {
            throw new NotImplementedException();
        }
        public void UploadImage(Stream imgStream, Stream fileStream, int pageNumber, ImageFormat imageFormat)
        {
            throw new NotImplementedException();
        }
    }
}