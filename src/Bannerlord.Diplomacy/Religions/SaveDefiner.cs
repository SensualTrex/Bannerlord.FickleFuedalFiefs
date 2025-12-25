using Diplomacy.CivilWar;
using Diplomacy.CivilWar.Factions;
using Diplomacy.DiplomaticAction;
using Diplomacy.Messengers;
using Diplomacy.WarExhaustion;
using Diplomacy.WarExhaustion.EventRecords;

using JetBrains.Annotations;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.SaveSystem;

namespace Diplomacy.Religions
{
    [UsedImplicitly]
    internal class SaveDefiner : SaveableTypeDefiner
    {
        private const int SaveBaseId = 1_984_110_150;

        public SaveDefiner() : base(SaveBaseId) { }



        protected override void DefineClassTypes()
        {
            AddClassDefinition(typeof(ReligionManager), 40);
            AddClassDefinition(typeof(Religion), 41);
            //Lore Types
            AddClassDefinition(typeof(ReligionLore), 42);
            //Stat Types
            AddClassDefinition(typeof(ReligionStatContainer), 43);
            AddClassDefinition(typeof(ReligiousStat), 45);
            //Building Manager Types
            AddClassDefinition(typeof(SettlementReligion), 46);
            AddClassDefinition(typeof(SettlementReligionContainer), 47);

        }

        protected override void DefineStructTypes()
        {
            //AddStructDefinition(typeof(FactionPair), 9);
            //AddStructDefinition(typeof(WarExhaustionRecord), 18);
            AddStructDefinition(typeof(ReligionStats), 60);
        }

        protected override void DefineEnumTypes()
        {
            AddEnumDefinition(typeof(ReligiousBuildingType), 50);
            AddEnumDefinition(typeof(ReligiousBuildingLevel), 51);
            AddEnumDefinition(typeof(ReligiousStatType), 52);

            //AddEnumDefinition(typeof(VictoriousFactionType), 16);
            //AddEnumDefinition(typeof(ActiveQuestState), 17);
        }

        protected override void DefineContainerDefinitions()
        {
            ConstructContainerDefinition(typeof(Dictionary<string, Religion>));
            ConstructContainerDefinition(typeof(Dictionary<string, SettlementReligion>));

        }
    }
}
