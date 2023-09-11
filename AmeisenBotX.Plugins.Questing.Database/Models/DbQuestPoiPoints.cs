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
    [Table("quest_poi_points")]
    public record DbQuestPoiPoints
    {
        public int QuestId { get; set; }
        public int Id { get; set; }
        public int IdX { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
