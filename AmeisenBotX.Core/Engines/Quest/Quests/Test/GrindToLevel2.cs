﻿using AmeisenBotX.Common.Math;
using AmeisenBotX.Core.Engines.Quest.Objects.Objectives;
using AmeisenBotX.Core.Engines.Quest.Objects.Quests;
using System.Collections.Generic;

namespace AmeisenBotX.Core.Engines.Quest.Quests.Test
{
    internal class GrindToLevel2 : GrindingBotQuest
    {
        public GrindToLevel2(AmeisenBotInterfaces bot)
            : base("Test Grind Boars in Valley of Strength",
                new List<IQuestObjective>()
                {
                    new QuestObjectiveChain(new List<IQuestObjective>()
                    {
                        new GrindingObjective(bot, 2, new List<List<Vector3>> {
                            new()
                            {
                                new Vector3(-486.28f, -4144.73f, 54.75f),
                                new Vector3(-550.96f, -4351.29f, 41.22f),
                                new Vector3(-555.42f, -4384.57f, 45.33f),
                                new Vector3(-549.24f, -4421.25f, 42.10f),
                                new Vector3(-486.18f, -4454.21f, 50.13f),
                                new Vector3(-351.91f, -4384.64f, 48.71f),
                                new Vector3(-281.01f, -4322.80f, 61.76f),
                                new Vector3(-308.83f, -4217.85f, 52.60f),
                                new Vector3(-349.29f, -4184.41f, 59.20f),
                            },
                            new()
                            {
                                new Vector3(-717.00f, -4150.75f, 30.07f),
                                new Vector3(-747.26f, -4181.42f, 30.24f),
                                new Vector3(-754.89f, -4219.76f, 42.85f),
                                new Vector3(-749.72f, -4281.92f, 43.21f),
                                new Vector3(-612.62f, -4448.09f, 45.59f),
                                new Vector3(-619.22f, -4382.64f, 43.22f),
                            }
                        }),
                    })
                })
        { }
    }
}