using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.ViewModels.Note
{
    public class IndexViewModel
    {
        public int TakeCount { get; set; }
        public int CurrentPage { get; set; }

        public string SelectedView { get; set; }

        public IEnumerable<string> Views { get; set; }

        //------------------------------------------- меняется сервером:
        public int TotalCount { get; set; }

        public IEnumerable<NoteViewModel> Notes { get; set; }
    }
}
