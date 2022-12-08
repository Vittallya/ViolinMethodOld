using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.ViewModels
{
    public class PriemViewModel
    {

        public int Id { get; set; }

        [Required]
        [Display(Name = "Название приема")]
        public string Name { get; set; }

        public string Description { get; set; }

        public int GroupId { get; set; }

        public IEnumerable<PriemGroupViewModel> Groups { get; set; }
    }
}
