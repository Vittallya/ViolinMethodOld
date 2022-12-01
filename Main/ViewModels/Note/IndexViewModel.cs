using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.ViewModels.Note
{
    public class IndexViewModel
    {
        public int TotalCount { get; set; }
        public int TakeCount { get; set; }
        public int CurrentPage { get; set; }

        public string SelectedView { get; set; }

        public IEnumerable<string> Views { get; set; }

        public FilterViewModel Filter { get; set; }
    }
}
