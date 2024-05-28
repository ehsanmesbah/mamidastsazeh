using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace mamidastsazeh.Models
{
    public class Province
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "استان")]
        public string Name { get; set; }

    }
}
