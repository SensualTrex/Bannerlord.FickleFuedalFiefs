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
using Religions.Religion;
using Religions.Religion.Extensions;

namespace Religions.Religion.ViewModel
{
    public class SettlementReligionSectionVM : TaleWorlds.Library.ViewModel
    {
        public MBBindingList<SettlementReligionItemVM> Cards { get; set; }

        public MBBindingList<SettlementReligionPopVM> Population { get; set; }

        public SettlementReligionVM Parent { get; private set; }

        private string _religionStringId;

        private string _religionName;

        private string _currentPopulation;

        private string _conversionRate;

        private string _templeLevel;
        
        private string _templeResources;

        private string _immigrationCities;

        private float _sectionHeight=45;

        private string _isPlayerReligion="";

        [DataSourceProperty]
        public string IsPlayerReligion
        {
            get
            {
                return _isPlayerReligion;
            }
            set
            {
                if (value != _isPlayerReligion)
                {
                    _isPlayerReligion = value;
                    base.OnPropertyChangedWithValue(value, "IsPlayerReligion");
                }
            }
        }

        [DataSourceProperty]
        public float SectionHeight
        {
            get
            {
                return _sectionHeight;
            }
            set
            {
                if (value != _sectionHeight)
                {
                    _sectionHeight = value;
                    base.OnPropertyChangedWithValue(value, "SectionHeight");
                }
            }
        }

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


        [DataSourceProperty]
        public string TempleLevel
        {
            get
            {
                return _templeLevel;
            }
            set
            {
                if (value != _templeLevel)
                {
                    _templeLevel = value;
                    base.OnPropertyChangedWithValue<string>(value, "TempleLevel");
                }
            }
        }

        [DataSourceProperty]
        public string TempleResources
        {
            get
            {
                return _templeResources;
            }
            set
            {
                if (value != _templeResources)
                {
                    _templeResources = value;
                    base.OnPropertyChangedWithValue<string>(value, "ConversionBrush");
                }
            }
        }
        // ImmigrationCities
        [DataSourceProperty]
        public string ImmigrationCities
        {
            get
            {
                return _immigrationCities;
            }
            set
            {
                if (value != _immigrationCities)
                {
                    _immigrationCities = value;
                    base.OnPropertyChangedWithValue<string>(value, "ImmigrationCities");
                }
            }
        }

        private Color _conversionRateColor;

        [DataSourceProperty]
        public Color ConversionRateColor
        {
            get => _conversionRateColor;
            set
            {
                if (value != _conversionRateColor)
                {
                    _conversionRateColor = value;
                    OnPropertyChangedWithValue(value, nameof(ConversionRateColor));
                }
            }
        }

        public SettlementReligionSectionVM(SettlementReligion settlementReligion, double currentPopulation, double conversionRate, int rowSize=0)
        {
            // -------------------------------------------------
            // INITIALIZE CARD LIST + BASIC RELIGION INFO
            // -------------------------------------------------
            _religionStringId = settlementReligion.ReligionId;
            Cards = new MBBindingList<SettlementReligionItemVM>();
            ReligionName = ReligionManager.GetReligion(settlementReligion.ReligionId).Name;

            // Display population and conversion rate as percentages
            CurrentPopulation = (Math.Round(currentPopulation, 4) * 100).ToString() + "%";
            ConversionRate = (conversionRate > 0 ? "+" : "") + (Math.Round(conversionRate, 4) * 100).ToString() + "%";

            // Color coding for conversion rate (green = positive, red = negative, white = neutral)
            ConversionRateColor = conversionRate > 0
                ? new Color(0f, 1f, 0f, 1f)
                : conversionRate < 0
                    ? new Color(1f, 0f, 0f, 1f)
                    : new Color(1f, 1f, 1f, 1f);


            // -------------------------------------------------
            // POPULATION BAR (10 segments, fractional support allowed)
            // -------------------------------------------------
            Population = new MBBindingList<SettlementReligionPopVM>();

            var PartialPop = (currentPopulation * 10) % 1;       // fractional segment
            var WholePop = (currentPopulation * 10) - PartialPop; // whole segments

            // Add whole population segments
            for (var i = 0; i < WholePop; i++)
            {
                Population.Add(new SettlementReligionPopVM());
            }

            // Add fractional segment if needed
            if (PartialPop > 0)
            {
                Population.Add(new SettlementReligionPopVM(PartialPop));
            }


            // -------------------------------------------------
            // RELIGION DETAIL CARD (conversion bonuses, etc.)
            // -------------------------------------------------
            Dictionary<string, string> religionDetails = new Dictionary<string, string>();

            religionDetails.Add("Conversion #", "+" + settlementReligion.CurrentConversionRate.ToString());

            if (settlementReligion.hasOwnerBonus())
                religionDetails.Add("Owner Bonus", "+2");

            if (settlementReligion.hasGovernorBonus())
                religionDetails.Add("Governor Bonus", "+1");

            int buildingBonus = settlementReligion.GetBuildingConversionBonus();
            if (buildingBonus > 0)
                religionDetails.Add("Building Bonus", "+" + buildingBonus.ToString());

            // Add the detail card
            Cards.Add(new SettlementReligionItemVM(religionDetails, this._religionStringId));


            // -------------------------------------------------
            // TEMPLE LEVEL + RESOURCE PROGRESS
            // -------------------------------------------------
            int nextLevelRequirement = (int) TempleResourceLevels.Level1;
            int currentLevel = (int) settlementReligion.Temple;

            // Determine next level requirement
            if (currentLevel == 1)
                nextLevelRequirement = (int) TempleResourceLevels.Level2;
            else if (currentLevel >= 2)
                nextLevelRequirement = (int) TempleResourceLevels.Level3;

            // Display temple level and resource progress
            TempleLevel = "Level " + ((int) settlementReligion.Temple).ToString();
            TempleResources = settlementReligion.TempleBuildResources.ToString() + "/" + nextLevelRequirement.ToString();


            // -------------------------------------------------
            // IMMIGRATION SOURCES (cities contributing population)
            // -------------------------------------------------
            var immigrationMap = settlementReligion.GetParent().ImmigrationMap;
            var immigrationCity = "";

            if (immigrationMap != null && immigrationMap.ContainsKey(settlementReligion.ReligionId))
            {
                foreach (var city in immigrationMap[settlementReligion.ReligionId])
                {
                    immigrationCity += TaleWorlds.CampaignSystem.Settlements.Settlement
                        .Find(city).Name.ToString() + ",\r\n";
                }
            }

            // Trim trailing comma
            ImmigrationCities = immigrationCity.TrimEnd(',');


            // -------------------------------------------------
            // TEMPLE BUILDING CARD (if temple exists)
            // -------------------------------------------------
            if (settlementReligion.Temple != ReligiousBuildingLevel.None)
            {
                Dictionary<string, string> itemProperties = new Dictionary<string, string>();

                // Conversion bonus scales with temple level (3 per level)
                itemProperties.Add("Conversion", "+" + (((int) settlementReligion.Temple) * 3).ToString());

                Cards.Add(new SettlementReligionItemVM(
                    "Temple",
                    settlementReligion.GetParent().Settlement.Culture.StringId,
                    this._religionStringId,
                    ((int) settlementReligion.Temple),
                    settlementReligion.SettlmentId,
                    itemProperties
                ));
            }


            isPlayerReligion();
        }

        public SettlementReligionSectionVM(SettlementReligionVM parent, SettlementReligion settlementReligion, double currentPopulation, double conversionRate, int rowSize = 0) : this(settlementReligion, currentPopulation, conversionRate, rowSize)
        {
            Parent = parent;           
        }

        public void isPlayerReligion()
        {
            var heroReligion = Hero.MainHero.GetHeroReligion();
            if (heroReligion != null && heroReligion.ReligionId == this._religionStringId)
            {
                IsPlayerReligion = "*";
            }
            else
            {
                IsPlayerReligion = "";
            }
        }

        public void ExpandSection()
        {
            if(SectionHeight == 45)
                SectionHeight = 201;
            else
                SectionHeight = 45;

        }

        public override void RefreshValues()
        {
            base.RefreshValues();
        }

    }
}
