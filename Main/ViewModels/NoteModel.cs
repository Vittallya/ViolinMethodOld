using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.ViewModels
{
    public class NoteModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset PublishDate { get; set; }

        public DateTimeOffset? LastUpdate { get; set; }
        public string FileName { get; set; }
        public int ShowPageNumber { get; set; }
        public List<PageInfoModel> PageInfo { get; set; }
    }
}
