using Bannerlord.UIExtenderEx.Attributes;
using Bannerlord.UIExtenderEx.ViewModels;

using Religions.Extensions;

using JetBrains.Annotations;

using NavalDLC;
using NavalDLC.ViewModelCollection.Map.MapBar;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapBar;
using TaleWorlds.Localization;

namespace Religions.ViewModelMixin
{
    
    /*[ViewModelMixin("RefreshPermissionValues")]
    [UsedImplicitly]
    internal sealed class MapNavigationVMMixin : BaseViewModelMixin<NavalMapBarVM>
    {
        private static readonly TextObject _TRebelKingdomText = new("{=f66sNaiz}You cannot be part of a rebellion");
        //NavalDLC.ViewModelCollection.Map.MapBar.NavalMapBarVM
        public MapNavigationVMMixin(NavalMapBarVM vm) : base(vm) { }

        public override void OnRefresh()
        {
            if (Clan.PlayerClan.Kingdom?.IsRebelKingdom() ?? false)
            {
                //@VIEWMODEL
                this.ViewModel!.
                //ViewModel!.ExecuteOpenKingdom();
                
                //ViewModel!.IsKingdomEnabled = false;
                //ViewModel!.KingdomHint.SetHintCallback(() => _TRebelKingdomText.ToString());
            }
        }
    }*/
}