﻿using AmeisenBotX.Common.Math;
using AmeisenBotX.Core.Engines.Dungeon.Enums;
using AmeisenBotX.Core.Engines.Dungeon.Objects;
using AmeisenBotX.Wow.Objects.Enums;
using System.Collections.Generic;

namespace AmeisenBotX.Core.Engines.Dungeon.Profiles.TBC
{
    public class TheUnderbogProfile : IDungeonProfile
    {
        public string Author { get; } = "Jannis";

        public string Description { get; } = "Profile for the Dungeon in Outland, made for Level 61 to 65.";

        public Vector3 DungeonExit { get; } = new(5, -14, -3);

        public DungeonFactionType FactionType { get; } = DungeonFactionType.Neutral;

        public int GroupSize { get; } = 5;

        public WowMapId MapId { get; } = WowMapId.TheUnderbog;

        public int MaxLevel { get; } = 65;

        public string Name { get; } = "[61-65] The Underbog";

        public List<IDungeonNode> Nodes { get; private set; } = new()
        {
            new DungeonNode(new(10, -16, -3)),
            new DungeonNode(new(16, -21, -3)),
            new DungeonNode(new(23, -25, -3)),
            new DungeonNode(new(30, -29, -3)),
            new DungeonNode(new(37, -34, -3)),
            new DungeonNode(new(44, -38, -3)),
            new DungeonNode(new(51, -42, -3)),
            new DungeonNode(new(58, -47, -3)),
            new DungeonNode(new(64, -52, -3)),
            new DungeonNode(new(68, -59, -3)),
            new DungeonNode(new(70, -67, -3)),
            new DungeonNode(new(70, -75, -3)),
            new DungeonNode(new(67, -82, -3)),
            new DungeonNode(new(63, -89, -3)),
            new DungeonNode(new(59, -96, -3)),
            new DungeonNode(new(55, -103, -3)),
            new DungeonNode(new(52, -110, -3)),
            new DungeonNode(new(53, -118, -3)),
            new DungeonNode(new(54, -126, -3)),
            new DungeonNode(new(56, -134, -3)),
            new DungeonNode(new(57, -142, -3)),
            new DungeonNode(new(58, -150, -3)),
            new DungeonNode(new(58, -158, -3)),
            new DungeonNode(new(57, -166, -3)),
            new DungeonNode(new(53, -173, -3)),
            new DungeonNode(new(48, -179, -4)),
            new DungeonNode(new(43, -185, -4)),
            new DungeonNode(new(36, -190, -4)),
            new DungeonNode(new(29, -195, -4)),
            new DungeonNode(new(23, -200, -4)),
            new DungeonNode(new(16, -204, -5)),
            new DungeonNode(new(8, -207, -5)),
            new DungeonNode(new(1, -210, -5)),
            new DungeonNode(new(-6, -214, -5)),
            new DungeonNode(new(-13, -218, -5)),
            new DungeonNode(new(-20, -221, -5)),
            new DungeonNode(new(-27, -225, -5)),
            new DungeonNode(new(-33, -230, -5)),
            new DungeonNode(new(-40, -235, -5)),
            new DungeonNode(new(-45, -241, -5)),
            new DungeonNode(new(-50, -248, -5)),
            new DungeonNode(new(-53, -255, -5)),
            new DungeonNode(new(-58, -261, -5)),
            new DungeonNode(new(-62, -268, -5)),
            new DungeonNode(new(-69, -273, -3)),
            new DungeonNode(new(-76, -277, -2)),
            new DungeonNode(new(-83, -281, 0)),
            new DungeonNode(new(-90, -284, 1)),
            new DungeonNode(new(-98, -286, 2)),
            new DungeonNode(new(-106, -286, 3)),
            new DungeonNode(new(-113, -284, 5)),
            new DungeonNode(new(-119, -279, 7)),
            new DungeonNode(new(-122, -272, 9)),
            new DungeonNode(new(-116, -267, 13)),
            new DungeonNode(new(-112, -262, 17)),
            new DungeonNode(new(-108, -258, 22)),
            new DungeonNode(new(-101, -255, 24)),
            new DungeonNode(new(-97, -262, 24)),
            new DungeonNode(new(-97, -270, 25)),
            new DungeonNode(new(-98, -278, 26)),
            new DungeonNode(new(-100, -286, 27)),
            new DungeonNode(new(-103, -294, 28)),
            new DungeonNode(new(-106, -302, 29)),
            new DungeonNode(new(-108, -310, 30)),
            new DungeonNode(new(-111, -317, 31)),
            new DungeonNode(new(-113, -325, 32)),
            new DungeonNode(new(-116, -332, 33)),
            new DungeonNode(new(-115, -340, 33)),
            new DungeonNode(new(-117, -348, 34)),
            new DungeonNode(new(-119, -356, 34)),
            new DungeonNode(new(-119, -364, 35)),
            new DungeonNode(new(-117, -372, 36)),
            new DungeonNode(new(-111, -377, 37)),
            new DungeonNode(new(-104, -380, 37)),
            new DungeonNode(new(-97, -384, 37)),
            new DungeonNode(new(-90, -387, 36)),
            new DungeonNode(new(-82, -388, 35)),
            new DungeonNode(new(-74, -388, 33)),
            new DungeonNode(new(-66, -388, 31)),
            new DungeonNode(new(-58, -388, 31)),
            new DungeonNode(new(-50, -388, 31)),
            new DungeonNode(new(-42, -388, 31)),
            new DungeonNode(new(-34, -386, 32)),
            new DungeonNode(new(-28, -380, 32)),
            new DungeonNode(new(-25, -373, 31)),
            new DungeonNode(new(-22, -366, 31)),
            new DungeonNode(new(-19, -359, 30)),
            new DungeonNode(new(-16, -352, 30)),
            new DungeonNode(new(-13, -345, 30)),
            new DungeonNode(new(-10, -338, 30)),
            new DungeonNode(new(-7, -331, 31)),
            new DungeonNode(new(-4, -324, 31)),
            new DungeonNode(new(2, -319, 31)),
            new DungeonNode(new(9, -315, 31)),
            new DungeonNode(new(16, -312, 32)),
            new DungeonNode(new(23, -309, 32)),
            new DungeonNode(new(30, -305, 32)),
            new DungeonNode(new(37, -302, 32)),
            new DungeonNode(new(44, -299, 33)),
            new DungeonNode(new(51, -295, 33)),
            new DungeonNode(new(58, -292, 33)),
            new DungeonNode(new(65, -288, 32)),
            new DungeonNode(new(73, -285, 32)),
            new DungeonNode(new(81, -286, 32)),
            new DungeonNode(new(86, -292, 32)),
            new DungeonNode(new(90, -299, 32)),
            new DungeonNode(new(93, -307, 32)),
            new DungeonNode(new(94, -315, 33)),
            new DungeonNode(new(95, -323, 33)),
            new DungeonNode(new(95, -331, 33)),
            new DungeonNode(new(93, -339, 33)),
            new DungeonNode(new(91, -347, 33)),
            new DungeonNode(new(89, -355, 33)),
            new DungeonNode(new(86, -362, 33)),
            new DungeonNode(new(83, -369, 33)),
            new DungeonNode(new(78, -376, 33)),
            new DungeonNode(new(73, -383, 33)),
            new DungeonNode(new(71, -391, 33)),
            new DungeonNode(new(79, -393, 34)),
            new DungeonNode(new(87, -393, 34)),
            new DungeonNode(new(95, -396, 35)),
            new DungeonNode(new(102, -398, 38)),
            new DungeonNode(new(109, -401, 40)),
            new DungeonNode(new(116, -404, 43)),
            new DungeonNode(new(123, -408, 45)),
            new DungeonNode(new(130, -411, 48)),
            new DungeonNode(new(137, -415, 49)),
            new DungeonNode(new(144, -418, 49)),
            new DungeonNode(new(151, -422, 49)),
            new DungeonNode(new(158, -425, 48)),
            new DungeonNode(new(165, -422, 48)),
            new DungeonNode(new(170, -416, 48)),
            new DungeonNode(new(174, -409, 48)),
            new DungeonNode(new(179, -403, 48)),
            new DungeonNode(new(184, -397, 48)),
            new DungeonNode(new(189, -390, 48)),
            new DungeonNode(new(194, -384, 48)),
            new DungeonNode(new(199, -378, 48)),
            new DungeonNode(new(205, -373, 48)),
            new DungeonNode(new(212, -370, 51)),
            new DungeonNode(new(218, -368, 56)),
            new DungeonNode(new(225, -366, 61)),
            new DungeonNode(new(232, -364, 65)),
            new DungeonNode(new(238, -363, 70)),
            new DungeonNode(new(246, -364, 72)),
            new DungeonNode(new(248, -372, 72)),
            new DungeonNode(new(242, -377, 73)),
            new DungeonNode(new(234, -379, 73)),
            new DungeonNode(new(227, -382, 73)),
            new DungeonNode(new(219, -384, 73)),
            new DungeonNode(new(212, -387, 72)),
            new DungeonNode(new(205, -390, 72)),
            new DungeonNode(new(198, -395, 72)),
            new DungeonNode(new(191, -400, 72)),
            new DungeonNode(new(185, -405, 72)),
            new DungeonNode(new(180, -411, 72)),
            new DungeonNode(new(175, -417, 72)),
            new DungeonNode(new(171, -424, 72)),
            new DungeonNode(new(168, -431, 72)),
            new DungeonNode(new(165, -438, 72)),
            new DungeonNode(new(162, -445, 72)),
            new DungeonNode(new(158, -452, 72)),
            new DungeonNode(new(157, -460, 73)),
            new DungeonNode(new(162, -466, 75)),
            new DungeonNode(new(170, -468, 76)),
            new DungeonNode(new(178, -469, 76)),
            new DungeonNode(new(186, -470, 77)),
            new DungeonNode(new(194, -470, 78)),
            new DungeonNode(new(202, -471, 79)),
            new DungeonNode(new(208, -476, 80)),
            new DungeonNode(new(215, -479, 81)),
            new DungeonNode(new(222, -476, 81)),
            new DungeonNode(new(229, -473, 81)),
            new DungeonNode(new(236, -469, 81)),
            new DungeonNode(new(243, -466, 81)),
            new DungeonNode(new(250, -462, 81)),
            new DungeonNode(new(246, -455, 81)),
            new DungeonNode(new(238, -457, 81)),
            new DungeonNode(new(246, -459, 81)),
            new DungeonNode(new(254, -461, 81)),
            new DungeonNode(new(261, -464, 81)),
            new DungeonNode(new(269, -463, 81)),
            new DungeonNode(new(277, -464, 81)),
            new DungeonNode(new(279, -465, 81)),
            new DungeonNode(new(292, -468, 49)),
            new DungeonNode(new(300, -469, 49)),
            new DungeonNode(new(308, -470, 49)),
            new DungeonNode(new(316, -472, 49)),
            new DungeonNode(new(324, -473, 49)),
            new DungeonNode(new(332, -475, 52)),
            new DungeonNode(new(336, -475, 52)),
            new DungeonNode(new(348, -474, 24)),
            new DungeonNode(new(354, -469, 24)),
            new DungeonNode(new(356, -461, 26)),
            new DungeonNode(new(357, -454, 29)),
            new DungeonNode(new(358, -447, 32)),
            new DungeonNode(new(359, -440, 35)),
            new DungeonNode(new(359, -433, 38)),
            new DungeonNode(new(357, -426, 42)),
            new DungeonNode(new(353, -420, 45)),
            new DungeonNode(new(348, -413, 46)),
            new DungeonNode(new(343, -407, 47)),
            new DungeonNode(new(338, -401, 45)),
            new DungeonNode(new(333, -394, 45)),
            new DungeonNode(new(335, -386, 44)),
            new DungeonNode(new(337, -379, 41)),
            new DungeonNode(new(339, -372, 38)),
            new DungeonNode(new(340, -365, 35)),
            new DungeonNode(new(341, -357, 32)),
            new DungeonNode(new(340, -349, 29)),
            new DungeonNode(new(337, -342, 26)),
            new DungeonNode(new(333, -335, 23)),
            new DungeonNode(new(329, -328, 21)),
            new DungeonNode(new(323, -322, 19)),
            new DungeonNode(new(317, -317, 19)),
            new DungeonNode(new(310, -313, 19)),
            new DungeonNode(new(303, -310, 19)),
            new DungeonNode(new(296, -306, 19)),
            new DungeonNode(new(289, -303, 19)),
            new DungeonNode(new(282, -299, 19)),
            new DungeonNode(new(277, -293, 21)),
            new DungeonNode(new(273, -286, 23)),
            new DungeonNode(new(270, -279, 24)),
            new DungeonNode(new(268, -271, 25)),
            new DungeonNode(new(268, -263, 26)),
            new DungeonNode(new(269, -255, 27)),
            new DungeonNode(new(269, -247, 27)),
            new DungeonNode(new(269, -239, 28)),
            new DungeonNode(new(269, -231, 28)),
            new DungeonNode(new(269, -223, 29)),
            new DungeonNode(new(269, -215, 29)),
            new DungeonNode(new(268, -207, 29)),
            new DungeonNode(new(266, -199, 29)),
            new DungeonNode(new(263, -192, 28)),
            new DungeonNode(new(260, -185, 29)),
            new DungeonNode(new(257, -178, 29)),
            new DungeonNode(new(254, -170, 29)),
            new DungeonNode(new(253, -162, 29)),
            new DungeonNode(new(254, -154, 29)),
            new DungeonNode(new(259, -147, 29)),
            new DungeonNode(new(265, -142, 30)),
            new DungeonNode(new(272, -138, 30)),
            new DungeonNode(new(279, -134, 30)),
            new DungeonNode(new(286, -130, 30)),
            new DungeonNode(new(290, -123, 30)),
            new DungeonNode(new(283, -119, 30)),
            new DungeonNode(new(275, -121, 30)),
            new DungeonNode(new(267, -123, 30)),
            new DungeonNode(new(260, -126, 29)),
            new DungeonNode(new(252, -127, 29)),
            new DungeonNode(new(244, -128, 28)),
            new DungeonNode(new(236, -128, 26)),
            new DungeonNode(new(228, -129, 26)),
            new DungeonNode(new(220, -129, 26)),
            new DungeonNode(new(212, -129, 27)),
            new DungeonNode(new(204, -129, 28)),
            new DungeonNode(new(196, -128, 28)),
            new DungeonNode(new(188, -128, 28)),
            new DungeonNode(new(180, -126, 27)),
            new DungeonNode(new(172, -123, 26)),
            new DungeonNode(new(165, -119, 25)),
            new DungeonNode(new(160, -113, 25)),
            new DungeonNode(new(160, -105, 26)),
            new DungeonNode(new(162, -97, 26)),
            new DungeonNode(new(165, -90, 27)),
            new DungeonNode(new(167, -82, 27)),
            new DungeonNode(new(170, -75, 27)),
            new DungeonNode(new(174, -68, 27)),
            new DungeonNode(new(180, -62, 26)),
            new DungeonNode(new(186, -57, 26)),
            new DungeonNode(new(193, -53, 27)),
            new DungeonNode(new(199, -48, 27)),
            new DungeonNode(new(205, -43, 27)),
            new DungeonNode(new(211, -38, 28)),
            new DungeonNode(new(216, -32, 28)),
            new DungeonNode(new(219, -25, 28)),
            new DungeonNode(new(218, -17, 28)),
            new DungeonNode(new(215, -10, 28)),
            new DungeonNode(new(212, -3, 28)),
            new DungeonNode(new(208, 4, 28)),
            new DungeonNode(new(202, 9, 28)),
            new DungeonNode(new(194, 11, 28)),
            new DungeonNode(new(186, 12, 27)),
            new DungeonNode(new(178, 14, 27)),
            new DungeonNode(new(170, 15, 27)),
            new DungeonNode(new(162, 16, 27)),
            new DungeonNode(new(154, 14, 27)),
            new DungeonNode(new(147, 10, 27)),
            new DungeonNode(new(140, 6, 27)),
            new DungeonNode(new(133, 3, 27)),
            new DungeonNode(new(126, -1, 27)),
            new DungeonNode(new(119, -4, 28)),
            new DungeonNode(new(111, -6, 27)),
            new DungeonNode(new(104, -8, 24)),
            new DungeonNode(new(97, -10, 21)),
            new DungeonNode(new(89, -11, 20)),
            new DungeonNode(new(81, -13, 18)),
            new DungeonNode(new(73, -15, 17)),
            new DungeonNode(new(65, -16, 19)),
            new DungeonNode(new(57, -17, 21)),
            new DungeonNode(new(49, -19, 20)),
            new DungeonNode(new(42, -22, 19)),
            new DungeonNode(new(40, -23, 19)),
            new DungeonNode(new(30, -28, -3)),
            new DungeonNode(new(22, -29, -3)),
        };

        public List<int> PriorityUnits { get; } = new();

        public int RequiredItemLevel { get; } = 65;

        public int RequiredLevel { get; } = 61;

        public Vector3 WorldEntry { get; } = new(782, 6746, -73);

        public WowMapId WorldEntryMapId { get; } = WowMapId.Outland;
    }
}