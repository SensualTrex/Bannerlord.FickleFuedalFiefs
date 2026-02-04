using System.Collections.Generic;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;

namespace Religions.Religion
{
    public class ReligionManager
    {
        public static ReligionManager Instance { get; private set; }

        public Dictionary<string, Religion> AllReligions => _data.AllReligions;
        
        public Dictionary<string, FactionReligionProfile> FactionReligionProfiles => _data.FactionReligionProfiles;
        public Dictionary<string, SettlementReligionContainer> SettlementReligions => _data.SettlementReligions;
        public Dictionary<string, HeroReligionContainer> HeroReligionTable => _data.HeroReligionTable;
        public Dictionary<string, int> ReligionStatTable => _data.ReligionStatTable;
        public Dictionary<string, int> SettlementBuildingRequirements => _data.SettlementBuildingRequirements;
       

        private ReligionSaveData _data;

        public ReligionManager(ReligionSaveData data)
        {
            Instance = this;
            _data = data;
        }

        public void OnAfterLoad()
        {
            RebuildRuntimeCaches();
            RegisterRuntimeEvents();
            RebindGameReferences();
        }

        private void RebuildRuntimeCaches()
        {
            // Add runtime-only cache rebuilding here
            //foreach(var settlementReligion in SettlementReligions.Values)
            //{
            //    settlementReligion.SettlementTick();
            //}
        }

        private void RegisterRuntimeEvents()
        {
            // Add event listeners here
        }

        private void RebindGameReferences()
        {

        }


        public static Religion GetReligion(string id) => Instance.AllReligions[id];

        public static SettlementReligionContainer GetSettlementContainer(string id) => Instance.SettlementReligions[id];
    }
}
