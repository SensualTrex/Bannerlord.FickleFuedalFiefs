using Bannerlord.UIExtenderEx.Attributes;
using Bannerlord.UIExtenderEx.ViewModels;

using Religions.GauntletInterfaces;

using JetBrains.Annotations;

using TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Overlay;

using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ScreenSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TownManagement;
using TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.CampaignSystem.ComponentInterfaces;


namespace Religions.ViewModelMixin
{
    [ViewModelMixin(nameof(SettlementMenuOverlayVM.RefreshValues))]
    [UsedImplicitly]
    internal sealed class SettlementVMMixin : BaseViewModelMixin<SettlementMenuOverlayVM>
    {

        [DataSourceProperty]
        public string FactionsLabel { get; set; }
        public bool IsEnabled => true;

        public SettlementVMMixin(SettlementMenuOverlayVM vm) : base(vm)
        {
            TaleWorlds.CampaignSystem.GameMenus.GameMenuInitializationHandlers.PlayerTownVisit playerTownVisit = new();
            vm.ContextList.Add(new StringItemWithEnabledAndHintVM(null, "Social", IsEnabled, "SettlementSocialButtonAction"));
           
            //KingdomSettlementVMement
        }

        public override void OnFinalize()
        {
            ViewModel!.OnFinalize();
        }

        public override void OnRefresh()
        {
            ViewModel!.ContextList.Add(new StringItemWithEnabledAndHintVM(null, "Social", IsEnabled, "SettlementSocialButtonAction"));
            ViewModel!.RefreshValues();
        }

        /* [DataSourceMethod]
        [UsedImplicitly]
        public void ExecuteShowFactions()
        {
            //_rebelFactionsInterface.ShowInterface(ScreenManager.TopScreen, ViewModel!.Kingdom);
        }*/
    }
}