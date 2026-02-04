using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;

namespace Religions.Religion.Behavior
{
    public class ReligionDummyHeroBehavior : CampaignBehaviorBase
    {
        public static Hero DummyHero;

        public static CharacterObject DummyTemplate;

        public override void RegisterEvents()
        {
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, OnSessionLaunched);
        }

        private void OnSessionLaunched(CampaignGameStarter starter)
        {
            // CharacterObjects are fully loaded here
            DummyTemplate = CharacterObject.Find("npc_commoner_template");
            var template = DummyTemplate;
            /*

            var safeSettlement = Settlement.All.First(s => s.IsTown);

            DummyHero = HeroCreator.CreateSpecialHero(
                template,
                safeSettlement,
                Clan.PlayerClan,
                null,
                30
            );*/
        }

        public override void SyncData(IDataStore dataStore) { }
    }

}
