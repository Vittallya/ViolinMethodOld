using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class Note
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset PublishDate { get; set; }

        public DateTimeOffset? LastUpdate { get; set; }
        public string FileName { get; set; }
        public int ShowPageNumber { get; set; }
        public List<PageInfo> PageInfo { get; set; }
    }
}
