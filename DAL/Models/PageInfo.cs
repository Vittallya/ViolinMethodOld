using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class PageInfo
    {
        public int PageNumber { get; set; }
        public int DiffLvl { get; set; }
        public string Recs { get; set; }
        public List<Priem> Priems { get; set; }
    }
}
