using Religions.CivilWar;
using Religions.DiplomaticAction.WarPeace;
using Religions.Religion;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;
using Religions.Religion.Services;

namespace Religions.Religion.Behavior
{
    internal sealed class ReligionInitBehavior : CampaignBehaviorBase
    {
        private readonly SettlementReligionGenerator _settlementReligionGenerator = new SettlementReligionGenerator();

        private ReligionSaveData _saveData;

        private bool _isDeveloper = true; // For testing purposes

        public ReligionInitBehavior(){
            _ = typeof(ReligionSaveData);
        }

        public override void RegisterEvents()
        {
            CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, OnNewGameCreated);
            CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, OnGameLoaded);
            //CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, OnSessionLaunched);
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, DailyTick);
        }

        private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
        {
            //_religionManager = new ReligionManager();
        }

        private void OnNewGameCreated(CampaignGameStarter starter)
        {
            _saveData = CreateNewSaveData();
            new ReligionManager(_saveData);
        }

        private void OnGameLoaded(CampaignGameStarter starter)
        {
            if(ReligionManager.Instance == null)
            {
                if(_saveData == null)
                {
                    _saveData = CreateNewSaveData();
                }
                new ReligionManager(_saveData);
            }
            ReligionManager.Instance.OnAfterLoad();
        }


        private ReligionSaveData CreateNewSaveData()
        {
            ReligionFactory.LoadAll();
            var data = new ReligionSaveData
            {
                AllReligions = ReligionFactory.Religions,
                SettlementReligions = new Dictionary<string, SettlementReligionContainer>(),
                HeroReligionTable = new Dictionary<string, HeroReligionContainer>(),
                ReligionStatTable = new Dictionary<string, int>(),
                SettlementBuildingRequirements = new Dictionary<string, int>
                {
                    { "Level1", 50000 },
                    { "Level2", 500000 },
                    { "Level3", 1200000 }
                }
            };

            foreach (var hero in Hero.AllAliveHeroes)
            {
                if (hero != Hero.MainHero)
                    data.HeroReligionTable.Add(hero.StringId, HeroReligionContainer.Create(hero));
            }

            foreach (var settlement in Settlement.All.Where(x => x.IsTown || x.IsCastle))
            {
                var container = new SettlementReligionContainer(settlement);
                _settlementReligionGenerator.GenerateSettlementReligion(container);
                data.SettlementReligions.Add(settlement.StringId, container);
            }

            return data;
        }

        private void DailyTick()
        {
            if (_saveData == null)
                return;

            foreach (var settlements in _saveData.SettlementReligions)
            {
                settlements.Value.SettlementTick();
            }
        }

        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData("ReligionSaveData", ref _saveData);

            if (dataStore.IsLoading)
            {
                if(_saveData == null)
                {
                    _saveData = CreateNewSaveData();
                }
                new ReligionManager(_saveData);
            }
        }
    }
}
