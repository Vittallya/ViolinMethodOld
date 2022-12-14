using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.ViewModels.Note
{
    public class FiltersAllViewModel
    {
        public IEnumerable<PriemGroupViewModel> PriemGroups { get; set; }

        public IEnumerable<int> TotalDiffLevels { get; set; }
    }
}
