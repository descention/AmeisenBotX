using AmeisenBotX.Common;
using AmeisenBotX.Core.Engines.Quest.Objects.Objectives;
using AmeisenBotX.Core.Engines.Quest.Objects.Quests;
using AmeisenBotX.Plugins.Questing.Database.Repository;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.AccessControl;

namespace AmeisenBotX.Plugins.Questing.Database
{
    public class DBProfile : IQuestProfile
    {
        public DBProfile(QuestingDatabaseEngine engine, AmeisenBotInterfaces bot, WorldContext world)
        {
            Name = "Database Driven Profile";
            Engine = engine;
        }

        public Queue<ICollection<IBotQuest>> Quests { get; } = new Queue<ICollection<IBotQuest>>();

        public string Name { get; init; }
        public QuestingDatabaseEngine Engine { get; }

        IQuestEngine IQuestProfile.Engine => this.Engine;

        public override string ToString()
        {
            return Name;
        }
    }
}
