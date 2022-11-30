using DAL.Models;
using System.Collections.Generic;

namespace BLL.Dto
{
    public class PageInfoDto
    {
        public int PageNumber { get; set; }
        public int DiffLvl { get; set; }
        public string Recs { get; set; }
        public List<PriemDto> Priems { get; set; }
    }
}