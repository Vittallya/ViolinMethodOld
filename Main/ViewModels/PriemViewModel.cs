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

        [Display(Name ="Описание")]
        public string Description { get; set; }

        [Display(Name = "Группа")]
        public int GroupId { get; set; }

        public IEnumerable<PriemGroupViewModel> Groups { get; set; }
    }
}
