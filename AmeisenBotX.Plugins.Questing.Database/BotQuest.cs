using AmeisenBotX.Common.Math;
using AmeisenBotX.Common.Utils;
using AmeisenBotX.Core.Engines.Movement.Enums;
using AmeisenBotX.Core.Engines.Quest.Objects.Objectives;
using AmeisenBotX.Core.Engines.Quest.Objects.Quests;
using AmeisenBotX.Wow.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmeisenBotX.Plugins.Questing.Database
{
    internal class BotQuest : IBotQuest
    {
        public BotQuest()
        {

        }

        public bool Accepted {get; set; }

        public TimegatedEvent ActionEvent { get; } = new(TimeSpan.FromMilliseconds(250));

        public bool Finished { get; set; }

        public int Id { get; init; } = -1;

        public string Name { get; init; } = "Default Title";

        public List<IQuestObjective> Objectives { get; init; }

        public bool Returned { get; set; }

        public void AcceptQuest()
        {
            
        }

        public bool CompleteQuest()
        {
            return false;
        }

        public BotQuestGetPosition GetStartObject { get; set; }
        public BotQuestGetPosition GetEndObject { get; set; }

        public void Execute() => Objectives.FirstOrDefault(e => !e.Finished)?.Execute();
    }
}
