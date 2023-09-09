using AmeisenBotX.Plugins.Questing.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmeisenBotX.Plugins.Questing.Database.Repository
{
    public class WorldContext: DbContext
    {
        public WorldContext():base()
        { 

        }

        public WorldContext(DbContextOptions<WorldContext> options) : base(options)
        {

        }

        public DbSet<DbQuest> Quests { get; set; }
        public DbSet<DbQuestObjective> QuestObjectives { get; set; }
    }
}
