using AmeisenBotX.Common.Math;
using AmeisenBotX.Wow.Objects;
using AmeisenBotX.Wow.Objects.Enums;
using AmeisenBotX.Wow.Objects.Raw.SubStructs;
using AmeisenBotX.Wow548.Objects.Descriptors;
using System.Text;

namespace AmeisenBotX.Wow548.Objects
{
    [Serializable]
    public unsafe class WowPlayer548 : WowUnit548, IWowPlayer
    {
        protected WowPlayerDescriptor548? PlayerDescriptor;

        private IEnumerable<VisibleItemEnchantment> itemEnchantments;

        private IEnumerable<QuestlogEntry> questlogEntries = new List<QuestlogEntry>();

        public int ComboPoints => Memory.Read(Memory.Offsets.ComboPoints, out byte comboPoints) ? comboPoints : 0;

        public bool IsGhost => HasBuffById(8326);

        public bool IsOutdoors { get; set; }

        public bool IsUnderwater { get; set; }

        public IEnumerable<VisibleItemEnchantment> ItemEnchantments => itemEnchantments;

        public int NextLevelXp => GetPlayerDescriptor().NextLevelXp;

        public IEnumerable<QuestlogEntry> QuestlogEntries => questlogEntries;

        public int Xp => GetPlayerDescriptor().Xp;

        public double XpPercentage => BotMath.Percentage(Xp, NextLevelXp);

        public bool IsAlliance()
        {
            return Race is WowRace.Draenei
                or WowRace.Human
                or WowRace.Dwarf
                or WowRace.Gnome
                or WowRace.Nightelf
                or WowRace.Worgen
                or WowRace.PandarenA;
        }

        public bool IsHorde()
        {
            return Race is WowRace.Undead
                or WowRace.Orc
                or WowRace.Bloodelf
                or WowRace.Tauren
                or WowRace.Troll
                or WowRace.Goblin
                or WowRace.PandarenH;
        }

        public override string ReadName()
        {
            if (Memory.Read(IntPtr.Add(Memory.Offsets.NameStore, (int)Memory.Offsets.NameMask), out uint nameMask)
                && Memory.Read(IntPtr.Add(Memory.Offsets.NameStore, (int)Memory.Offsets.NameBase), out uint nameBase))
            {
                uint shortGuid = (uint)Guid & 0xfffffff;
                uint offset = 12 * (nameMask & shortGuid);

                if (Memory.Read(new(nameBase + offset + 8), out uint current)
                    && Memory.Read(new(nameBase + offset), out offset))
                {
                    if ((current & 0x1) == 0x1)
                    {
                        return string.Empty;
                    }

                    Memory.Read(new(current), out uint testGuid);

                    while (testGuid != shortGuid)
                    {
                        Memory.Read(new(current + offset + 4), out current);

                        if ((current & 0x1) == 0x1)
                        {
                            return string.Empty;
                        }

                        Memory.Read(new(current), out testGuid);
                    }

                    if (Memory.ReadString(new(current + (int)Memory.Offsets.NameString), Encoding.UTF8, out string name, 16))
                    {
                        return name;
                    }
                }
            }

            return string.Empty;
        }

        public override string ToString()
        {
            return $"Player: {Guid} lvl. {Level}";
        }

        public override void Update()
        {
            base.Update();

            var obj = GetPlayerDescriptor();

            questlogEntries = new QuestlogEntry[]
            {
                obj.QuestlogEntry1,
                obj.QuestlogEntry2,
                obj.QuestlogEntry3,
                obj.QuestlogEntry4,
                obj.QuestlogEntry5,
                obj.QuestlogEntry6,
                obj.QuestlogEntry7,
                obj.QuestlogEntry8,
                obj.QuestlogEntry9,
                obj.QuestlogEntry10,
                obj.QuestlogEntry11,
                obj.QuestlogEntry12,
                obj.QuestlogEntry13,
                obj.QuestlogEntry14,
                obj.QuestlogEntry15,
                obj.QuestlogEntry16,
                obj.QuestlogEntry17,
                obj.QuestlogEntry18,
                obj.QuestlogEntry19,
                obj.QuestlogEntry20,
                obj.QuestlogEntry21,
                obj.QuestlogEntry22,
                obj.QuestlogEntry23,
                obj.QuestlogEntry24,
                obj.QuestlogEntry25,
            }.Where(t => t.Id > 0);

            
        }

        protected WowPlayerDescriptor548 GetPlayerDescriptor()
        {
            return PlayerDescriptor ??= Memory.Read(DescriptorAddress + sizeof(WowObjectDescriptor548) + sizeof(WowUnitDescriptor548), out WowPlayerDescriptor548 objPtr) ? objPtr : new();
        }
    }
}