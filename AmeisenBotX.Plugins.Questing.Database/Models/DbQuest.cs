using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AmeisenBotX.Plugins.Questing.Database.Models
{
    [Table("quest_template")]
    public record DbQuest
    {
        public int Id { get; set; }
        public int Method { get; set; }
        public int Level { get; set; }
        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }
        public int ZoneOrSort { get; set; }
        public int Type { get; set; }
        public int SuggestedPlayers { get; set; }

        public int PointMapId { get; set; }
        public float PointX { get; set; }
        public float PointY { get; set; }

        public int PointOption { get; set; }
        public string Title { get; set; }
        public string Objectives { get; set; }
        public string Details { get; set; }
    }
}
