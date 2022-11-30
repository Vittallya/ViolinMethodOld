using System;
using System.Collections.Generic;

namespace BLL.Dto
{
    public class NoteDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset PublishDate { get; set; }

        public DateTimeOffset? LastUpdate { get; set; }
        public string FileName { get; set; }
        public int ShowPageNumber { get; set; }
        public List<PageInfoDto> PageInfo { get; set; }
    }
}
