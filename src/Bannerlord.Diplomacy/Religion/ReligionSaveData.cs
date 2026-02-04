using System.Collections.Generic;

using TaleWorlds.SaveSystem;

namespace Religions.Religion
{
    //[SaveableRootClass(570010)]
    public class ReligionSaveData
    {

        [SaveableProperty(1)]
        public Dictionary<string, Religion> AllReligions { get; set; }

        [SaveableProperty(2)]
        public Dictionary<string, SettlementReligionContainer> SettlementReligions { get; set; }

        [SaveableProperty(3)]
        public Dictionary<string, HeroReligionContainer> HeroReligionTable { get; set; }

        [SaveableProperty(4)]
        public Dictionary<string, int> ReligionStatTable { get; set; }

        [SaveableProperty(5)]
        public Dictionary<string, int> SettlementBuildingRequirements { get; set; }

        [SaveableProperty(6)]
        public Dictionary<string, FactionReligionProfile> FactionReligionProfiles { get; set; }
    }
}