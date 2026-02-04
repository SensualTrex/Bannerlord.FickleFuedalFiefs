using Bannerlord.UIExtenderEx.Attributes;
using Bannerlord.UIExtenderEx.ViewModels;

using Religions.Actions;
using Religions.Costs;
using Religions.Events;
using Religions.GauntletInterfaces;
using Religions.Messengers;
using Religions.Religion;
using Religions.Religion.Extensions;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ScreenSystem;

namespace Religions.Religion.ViewModelMixin
{
    [ViewModelMixin(nameof(EncyclopediaHeroPageVM.RefreshValues))]
    internal sealed class EncyclopediaHeroPageVMMixin : BaseViewModelMixin<EncyclopediaHeroPageVM>
    {
        private readonly Hero _hero;
        private string _religionName;

        public EncyclopediaHeroPageVMMixin(EncyclopediaHeroPageVM vm) : base(vm)
        {
            _hero = (vm.Obj as Hero)!;
            var heroReligionId = _hero.GetHeroReligion().ReligionId;
            ReligionText = ReligionManager.GetReligion(heroReligionId).Name;
            vm.RefreshValues();
        }

        public override void OnRefresh()
        {
            //ViewModel?.RefreshValues();

            // this is called before the constructor the first time
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (_hero is null)
            {
                return;
            }

            
        }

        [DataSourceProperty]
        public string ReligionText
        {
            get
            {
                return this._religionName;
            }
            set
            {
                if (value != this._religionName)
                {
                    this._religionName = value;
                    base.OnPropertyChangedWithValue(value, "ReligionText");
                }
            }
        }
    }
}