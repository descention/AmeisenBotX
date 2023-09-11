using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmeisenBotX.Plugins.Questing.Database.Models
{
    [Keyless]
    [Table("creature_loot_template")]
    public record DbCreatureLootTemplate
    {
        public float ChanceOrQuestChance { get; set; }
        public int entry { get; set; }
        public int item { get; set; }
        public int mincountOrRef { get; set; }
        public short groupid { get; set; }
        public short maxcount { get; set; }
    }
}
