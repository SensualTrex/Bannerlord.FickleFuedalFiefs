using Diplomacy.GauntletInterfaces;

using SandBox.View.Map;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

using TaleWorlds.Localization;
using TaleWorlds.ScreenSystem;

namespace Diplomacy.CampaignBehaviors
{

    internal sealed class TownMenuBehavior : CampaignBehaviorBase
    {
        private bool _isDeveloper = true; // For testing purposes
        public override void RegisterEvents()
        {
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, OnSessionLaunched);
        }

        private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
        {
            AddGameMenus(campaignGameStarter);
        }

        private void AddGameMenus(CampaignGameStarter campaignGameStarter)
        {
            // Add new submenu for town social interactions
            campaignGameStarter.AddGameMenu("town_social_menu", "Town Social Menu", TownSocialMenuInit);

            campaignGameStarter.AddGameMenuOption("town_social_menu", "town_social_religion", "Town Holy District",
                (MenuCallbackArgs args) => {
                    args.optionLeaveType = GameMenuOption.LeaveType.Mission;
                    return true;
                }, 
                (MenuCallbackArgs args) =>
                {
                    Settlement currentSettlement = Settlement.CurrentSettlement;
                    var TownReligionInterface = new SettlementReligionInterface();
                    TownReligionInterface.ShowInterface(ScreenManager.TopScreen, currentSettlement);
                }, false, 0, false);
            campaignGameStarter.AddGameMenuOption("town_social_menu", "town_social_leave", "Leave",
                (MenuCallbackArgs args) => {
                    args.optionLeaveType = GameMenuOption.LeaveType.Mission;
                    return true;
                },
                (MenuCallbackArgs args) =>
                {
                    GameMenu.SwitchToMenu("town");
                }, false, 1, false);

            campaignGameStarter.AddGameMenuOption("town", "town_social", "Social",
            // Condition Delegate (returns true to show the button)
            TownHasSocialOptionCondition, TownSocialMenuConsequence, false, 2, false);
        }

        // Condition: only show if is the 
        private bool TownHasSocialOptionCondition(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.Submenu;

            // Default value is 20 
            // Added a config setting to disable for testing purposes
            if ((Settlement.CurrentSettlement.Owner.GetRelationWithPlayer() < 20) && !(_isDeveloper))
                args.IsEnabled = false;
                args.Tooltip = new TextObject("{=Diplomacy_TownMenu_Social_TooLow}Your relation with the lord is too low to engage in social activities.");

            return true;
        }

        private void TownSocialMenuConsequence(MenuCallbackArgs args)
        {
            // Logic to open social submenu goes here
            GameMenu.SwitchToMenu("town_social_menu");
        }

        private void TownSocialMenuInit(MenuCallbackArgs args)
        {
            // Logic to initialize the social menu goes here
            
        }

        public override void SyncData(IDataStore dataStore)
        {

        }
    }
}
