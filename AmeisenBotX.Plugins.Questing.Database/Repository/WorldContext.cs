using AmeisenBotX.Plugins.Questing.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace AmeisenBotX.Plugins.Questing.Database.Repository
{
    public class WorldContext : DbContext
    {
        public WorldContext(DbContextOptions options) : base(options)
        {

        }

        
        
        public DbSet<DbCreature> Creatures { get; set; }
        public DbSet<DbCreatureTemplate> CreatureTemplates { get; set; }
        public DbSet<DbCreatureQuestStarter> CreatureQuestStarters { get; set; }
        public DbSet<DbCreatureQuestEnder> CreatureQuestEnders { get; set; }
        public DbSet<DbCreatureLootTemplate> CreatureLootTemplates { get; set; }

        public DbSet<DbQuest> Quests { get; set; }
        public DbSet<DbQuestPoi> QuestPoi { get; set; }
        public DbSet<DbQuestPoiPoints> QuestPoiPoints { get; set; }
        public DbSet<DbQuestObjective> QuestObjectives { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}

