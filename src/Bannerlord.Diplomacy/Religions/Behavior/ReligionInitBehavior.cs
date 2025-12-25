using Diplomacy.CivilWar;
using Diplomacy.Religions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Localization;

namespace Diplomacy.Religions.Behavior
{
    internal sealed class ReligionInitBehavior : CampaignBehaviorBase
    {
        private ReligionManager _religionManager;

        private bool _isDeveloper = true; // For testing purposes

        public ReligionInitBehavior(){
            _religionManager = new ReligionManager();
        }

        public override void RegisterEvents()
        {
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, OnSessionLaunched);
        }

        private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
        {
            
        }



        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData("_religionManager", ref _religionManager);

            if (dataStore.IsLoading)
            {
                _religionManager ??= new();
                _religionManager.Sync();
            }
        }
    }
}
