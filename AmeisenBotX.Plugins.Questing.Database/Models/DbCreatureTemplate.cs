using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmeisenBotX.Plugins.Questing.Database.Models
{
    [Table("creature_template")]
    public record DbCreatureTemplate
    {
        [Key]
        public int Entry { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
    }
}
