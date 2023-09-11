using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmeisenBotX.Plugins.Questing.Database.Models
{
    [Table("creature_questender")]
    [Keyless]
    public record DbCreatureQuestEnder
    {
        [Column("Id")]
        public int Entry { get; set; }

        [Column("Quest")]
        public int QuestId { get; set; }
    }
}
