using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Main.ViewModels
{
    public class PriemGroupViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display( Name ="Название группы")]
        public string Name { get; set; }

        public IEnumerable<PriemViewModel> Priems { get; set; }
    }
}
