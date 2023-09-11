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
    [Table("quest_poi")]
    public record DbQuestPoi
    {
        public int QuestId { get; set; }
        public int Id { get; set; }
        public int ObjIndex { get; set; }
        public int Mapid { get; set; }
        public int WorldMapAreaId { get; set; }
        public int FloorId { get; set; }
    }
}
