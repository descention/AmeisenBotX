using System.Collections.Generic;

namespace AmeisenBotX.Core.Engines.Quest.Objects.Quests
{
    public interface IQuestProfile
    {
        IQuestEngine Engine { get; }
        Queue<ICollection<IBotQuest>> Quests { get; }
    }
}