using DAL.Models;
using System.Collections.Generic;

namespace Main.ViewModels
{
    public class PageInfoModel
    {
        public int PageNumber { get; set; }
        public int DiffLvl { get; set; }
        public string Recs { get; set; }
        public List<PriemModel> Priems { get; set; }
    }
}