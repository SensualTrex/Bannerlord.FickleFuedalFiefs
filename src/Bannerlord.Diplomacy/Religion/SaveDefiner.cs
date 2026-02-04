using Religions.DiplomaticAction;
using Religions.Messengers;
using Religions.WarExhaustion.EventRecords;
using Religions.Religion;
using System.Collections.Generic;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.SaveSystem;

namespace Religions.Religion {
    public class ReligionSaveDefiner : SaveableTypeDefiner
    {
        public ReligionSaveDefiner() : base(2_527_000)
        {
        }

        protected override void DefineClassTypes()
        {
            AddClassDefinition(typeof(ReligionSaveData), 1);
            AddClassDefinition(typeof(Religion), 2);
            AddClassDefinition(typeof(HeroReligionContainer), 3);
            AddClassDefinition(typeof(SettlementReligionContainer), 4);
            AddClassDefinition(typeof(ReligionStats), 5);
            AddClassDefinition(typeof(SettlementReligion), 6);
        }

        protected override void DefineContainerDefinitions()
        {
            ConstructContainerDefinition(typeof(Dictionary<string, Religion>));
            ConstructContainerDefinition(typeof(Dictionary<string, SettlementReligionContainer>));
            ConstructContainerDefinition(typeof(Dictionary<string, SettlementReligion>));
            ConstructContainerDefinition(typeof(Dictionary<string, HeroReligionContainer>));
            ConstructContainerDefinition(typeof(Dictionary<string, double>));
            ConstructContainerDefinition(typeof(Dictionary<string, int>));
        }

        protected override void DefineEnumTypes()
        {
            AddEnumDefinition(typeof(ReligiousBuildingLevel), 20);
            AddEnumDefinition(typeof(ReligiousBuildingType), 21);
        }

    }
}