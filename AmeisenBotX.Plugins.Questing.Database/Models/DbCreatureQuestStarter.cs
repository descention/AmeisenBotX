using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmeisenBotX.Plugins.Questing.Database.Models
{
    [Table("creature_queststarter")]
    [Keyless]
    public record DbCreatureQuestStarter
    {
        [Column("Id")]
        public int Entry { get; set; }

        [Column("Quest")]
        public int QuestId { get; set; }
    }
}
