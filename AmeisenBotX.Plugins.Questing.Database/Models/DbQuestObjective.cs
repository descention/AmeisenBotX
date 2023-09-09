using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmeisenBotX.Plugins.Questing.Database.Models
{
    [Table("quest_objective")]
    public record DbQuestObjective
    {
        public int Id { get; set; }
        public int QuestId { get; set; }
        public int Index { get; set; }
        public int Type { get; set; }
        public int ObjectId { get; set; }
        public int Amount { get; set; }
        public int Flags { get; set; }
        public string? Description { get; set; }
    }
}
