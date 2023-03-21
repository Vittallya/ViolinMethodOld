using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.ViewModels.Note
{
    public class FilterViewModel
    {
        public int[] SelectedDiffLvls { get; set; }
        public Guid[] SelectedGuids { get; set; }
        public int[] SelectedPriems { get; set; }

        public int? SelectedGroup { get; set; }

        public bool Setted { get; set; }
    }
}
