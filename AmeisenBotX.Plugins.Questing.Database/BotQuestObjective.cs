using AmeisenBotX.Core.Engines.Quest.Objects.Objectives;
using AmeisenBotX.Plugins.Questing.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmeisenBotX.Plugins.Questing.Database
{
    public class BotQuestObjective : IQuestObjective
    {
        

        public DbQuestObjective? DbQuestObjective { get; init; }
        
        public bool Finished => Progress == 100.0;

        public double Progress { get; set; } = 0;

        public Action? Action { get; set; }

        public void Execute() => Action?.Invoke();
    }
}
