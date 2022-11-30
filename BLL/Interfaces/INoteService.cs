using BLL.Dto;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace BLL
{
    public interface INoteService
    {
        public NoteDto GetById(Guid id);
        public IEnumerable<NoteDto> GetNotes(int take, int skip, IEnumerable<Guid> noteIds = null, IEnumerable<int> priems = null);
        public Task InsertOrUpdateAsync(NoteDto model, Guid? guid = null);
        public void UploadImage(Stream imgStream, Stream fileStream, int pageNumber, ImageFormat imageFormat);
    }
}
