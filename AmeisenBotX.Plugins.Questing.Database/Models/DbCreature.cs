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
    [Table("creature")]
    public record DbCreature
    {
        public int guid { get; set; }
        public int id { get; set; }
        public int spawnMask { get; set; }
        public int phaseId { get; set; }
        public int phaseGroup { get; set; }
        public int spawntimesecs { get; set; }
        public int curhealth { get; set; }
        public int curmana { get; set; }
        public int npcflag { get; set; }
        public int unit_flags { get; set; }
        public int dynamicflags { get; set; }
        public float position_x { get; set; }
        public float position_y { get; set; }
        public float position_z { get; set; }
        public float orientation { get; set; }
        public float spawndist { get; set; }
        public bool equipment_id { get; set; }
        public bool MovementType { get; set; }
    }
}
