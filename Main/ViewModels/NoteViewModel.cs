using BLL.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.ViewModels
{
    public class NoteViewModel: ICloneable
    {

        [Display(Name = "Показывать номер страницы")]
        [Required]
        public int ShowPageNumber { get; set; }

        [Display(Name = "Файл")]
        [Required]
        public IFormFile File { get; set; }

        [Required]
        [Display(Name = "Название")]
        public string Name { get; set; }
        public DateTimeOffset PublishDate { get; set; }
        public DateTimeOffset? LastUpdate { get; set; }
        public string FileName { get; set; }
        public Guid? Id { get; set; }
        public bool IsNew { get; set; }
        public IEnumerable<PriemDto> AllPriems { get; set; }
        public string PageInfoJson { get; set; }
        public bool IsOtherFile { get; set; }
        public List<PageInfoDto> PageInfo { get; set; }

        public PageInfoDto Info { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
