using AmeisenBotX.Common.Math;
using AmeisenBotX.Common.Utils;
using AmeisenBotX.Core.Engines.Quest.Objects.Objectives;
using AmeisenBotX.Wow.Objects;
using System.Collections.Generic;

namespace AmeisenBotX.Core.Engines.Quest.Objects.Quests
{
    public delegate (IWowObject Unit, Vector3 Position) BotQuestGetPosition();

    public interface IBotQuest
    {
        bool Accepted { get; set; }

        TimegatedEvent ActionEvent { get; }

        bool Finished { get; }

        int Id { get; }

        BotQuestGetPosition GetEndObject { get; set; }

        BotQuestGetPosition GetStartObject { get; set; }

        string Name { get; }

        List<IQuestObjective> Objectives { get; }

        bool Returned { get; set; }

        void AcceptQuest();

        bool CompleteQuest();

        void Execute();
    }
}