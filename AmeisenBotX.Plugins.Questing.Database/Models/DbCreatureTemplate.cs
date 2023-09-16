using ServiceStack.DataAnnotations;
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

        public int questItem1 { get; set; }
        public int questItem2 { get; set; }
        public int questItem3 { get; set; }
        public int questItem4 { get; set; }
        public int questItem5 { get; set; }
        public int questItem6 { get; set; }

        [Computed]
        public IEnumerable<int> QuestItems => new[] { questItem1, questItem2, questItem3, questItem4, questItem5, questItem6 };
    }
}
