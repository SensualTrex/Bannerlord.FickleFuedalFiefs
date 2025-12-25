using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace Diplomacy.Religions.ViewModel
{
    public class SettlementReligionSectionVM : TaleWorlds.Library.ViewModel
    {
        public MBBindingList<SettlementReligionItemVM> Cards { get; set; }

        private string _religionStringId;

        private string _religionName;

        private string _currentPopulation;

        private string _conversionRate;

        [DataSourceProperty]
        public string ReligionName
        {
            get
            {
                return _religionName;
            }
            set
            {
                if(value != _religionName)
                {
                    _religionName = value;
                    base.OnPropertyChangedWithValue<string>(value, "ReligionName");
                }
            }
        }

        [DataSourceProperty]
        public string CurrentPopulation
        {
            get
            {
                return _currentPopulation;
            }
            set
            {
                if (value != _currentPopulation)
                {
                    _currentPopulation = value;
                    base.OnPropertyChangedWithValue<string>(value, "CurrentPopulation");
                }
            }
        }

        [DataSourceProperty]
        public string ConversionRate
        {
            get
            {
                return "(" + _conversionRate + ")";
            }
            set
            {
                if (value != _conversionRate)
                {
                    _conversionRate = value;
                    base.OnPropertyChangedWithValue<string>(value, "ConversionRate");
                }
            }
        }

        public SettlementReligionSectionVM(SettlementReligion settlementReligion, int rowSize=0)
        {
            ReligionName = settlementReligion.ReligionId;
            //CurrentPopulation = settlementReligion.PopulationPercent.ToString();
        }

        public SettlementReligionSectionVM()
        {
            Cards = new MBBindingList<SettlementReligionItemVM>();
        }

        public void AddCard(ItemObject item, CharacterObject troop, string equipmentType)
        {
            //Cards.Add(new SettlementReligionItemVM());
        }

    }
}
