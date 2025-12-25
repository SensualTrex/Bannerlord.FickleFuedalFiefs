using Diplomacy.CivilWar;
using Diplomacy.CivilWar.Actions;
using Diplomacy.CivilWar.Factions;
using Diplomacy.Extensions;
using Diplomacy.Religions;
using Diplomacy.ViewModel;

using Helpers;

using JetBrains.Annotations;

using NavalDLC.GameComponents;

using SandBox.CampaignBehaviors;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;


namespace Diplomacy.Religions
{
    internal class ReligionManager
    {

        public static ReligionManager Instance { get; private set; }

        [SaveableProperty(1)]
        [UsedImplicitly]
        public Dictionary<string, Religion> AllReligions { get; private set; }

        [SaveableProperty(2)]
        [UsedImplicitly]
        public Dictionary<string, SettlementReligionContainer> SettlementReligions { get; private set; }

        [SaveableProperty(3)]
        [UsedImplicitly]
        public Dictionary<string, int> ReligionStatTable { get; private set; } = new Dictionary<string, int>();

        internal void Sync()
        {
            Instance = this;
        }

        public ReligionManager()
        {  
            Instance = this;
            if(AllReligions == null)
                AllReligions = ReligionFactory.LoadReligions();
            init();

        }

        public void init()
        {
            AllReligions = new Dictionary<string, Religion>();
         

            SettlementReligions = new Dictionary<string, SettlementReligionContainer>();
            foreach (Settlement settlement in Settlement.All)
            {
                SettlementReligions.Add(settlement.StringId, new SettlementReligionContainer(settlement));
            }
        }

        internal void OnAfterSaveLoaded()
        {
            /*//Remove factions of dead kingdoms
            var keysToRemove = AllRebelFactions.Keys.Where(k => k.IsEliminated).ToList();
            foreach (var keyToRemove in keysToRemove)
            {
                RebelFactions.Remove(keyToRemove);
            }
            //Account for eliminated clans
            var factionsToClean = AllRebelFactions.Values.SelectMany(x => x).Where(x => x.Clans.Any(clan => clan.IsEliminated)).ToList();
            foreach (var faction in factionsToClean)
            {
                if (faction.Clans.All(clan => clan.IsEliminated))
                {
                    //Destroy dead factions
                    DestroyRebelFaction(faction);
                    continue;
                }
                foreach (var clan in faction.Clans.ToList())
                {
                    //Clear rest of the factions from dead clans
                    if (clan.IsEliminated) faction.RemoveClan(clan);
                }
            }
            //Fix factions that count as dead but not actually dead
            var kingdomsToReanimate = DeadRebelKingdoms.Where(k => !k.IsEliminated).ToList();
            foreach (var kingdom in kingdomsToReanimate)
            {
                DeadRebelKingdoms.Remove(kingdom);
                var enemyKingdomList = FactionHelper.GetEnemyKingdoms(kingdom).Where(k => !k.IsEliminated).ToList();
                if (enemyKingdomList.Count == 1)
                {
                    var parentKingdom = enemyKingdomList.First();
                    MakePeaceAction.Apply(kingdom, parentKingdom);
                    ConsolidateKingdomsAction.Apply(kingdom, parentKingdom);
                    DeadRebelKingdoms.Add(kingdom);
                }
            }*/
        }

        public static Religion GetReligion(string ReligionId)
        {
            return Instance.AllReligions[ReligionId];
        }
        public static Dictionary<string, Religion> GetAllReligions()
        {
            return Instance.AllReligions;
        }

    }
}
