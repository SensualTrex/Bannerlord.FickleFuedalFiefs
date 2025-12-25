using JetBrains.Annotations;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Xml.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.MountAndBlade;
using TaleWorlds.SaveSystem;

namespace Diplomacy.Religions
{
    public class Religion
    {
        [UsedImplicitly]
        [SaveableProperty(1)]
        public string ReligionId { get; private set; }

        [UsedImplicitly]
        [SaveableProperty(2)]
        public string Name { get; private set; }

        [UsedImplicitly]
        [SaveableProperty(3)]
        public ReligionStats Stats { get; private set; }

        [UsedImplicitly]
        [SaveableProperty(4)]
        public Dictionary<string, int> Factory { get; private set; }

        public Religion(string name, string religionId, ReligionStats religionStats, Dictionary<string,int> factory)
        {
            Name = name;
            ReligionId = religionId;
            Stats = religionStats;
            Factory = factory;
        }

    }

    public struct ReligionStats
    {
        [SaveableProperty(1)]
        public string StringId { get; private set; }
        [SaveableProperty(2)]
        public int Conversion { get; private set; } = 0;
        [SaveableProperty(3)]
        public double ConversionMultiplier { get; private set; } = 1.0;
        [SaveableProperty(4)]
        public int Zeal { get; private set; } = 0;
        [SaveableProperty(5)]
        public double ZealMultiplier { get; private set; } = 1.0;
        [SaveableProperty(6)]
        public Dictionary<string, int> ConversionReligionBonus { get; private set; } = new Dictionary<string, int>();
        [SaveableProperty(7)]
        public Dictionary<string, int> ConversionCultureBonus { get; private set; } = new Dictionary<string, int>();
        
        [SaveableProperty(8)]
        public int Pacifism { get; private set; } = 0;
        [SaveableProperty(9)]
        public int Tolerance { get; private set; } = 0;
        [SaveableProperty(10)]
        public int Chivalry { get; private set; } = 0;
        [SaveableProperty(11)]
        public int Piety { get; private set; } = 0;
        public ReligionStats(string stringid, int conversion, double conversionMultiplier,int zeal, double zealMultiplier, Dictionary<string, int> conversionReligionBonus, Dictionary<string, int> conversionCultureBonus, int pacifism, int chivalry, int piety, int tolerance) {
            StringId = stringid;
            Conversion = conversion;
            ConversionMultiplier = conversionMultiplier;
            Zeal = zeal;
            ZealMultiplier = zealMultiplier;
            ConversionReligionBonus = conversionReligionBonus ?? new Dictionary<string, int>();
            ConversionCultureBonus = conversionCultureBonus ?? new Dictionary<string, int>();
            Pacifism = pacifism;
            Chivalry = chivalry;
            Piety = piety;
            Tolerance = tolerance;
        }

        public static ReligionStats Create(string stringid, int conversion, int zeal,Dictionary<string, int> conversionReligionBonus, Dictionary<string, int> conversionCultureBonus, int pacafism, int chivalry, int piety, int tolerance)
        {
            return new ReligionStats(stringid, conversion, 0, zeal, 0, conversionReligionBonus, conversionCultureBonus, pacafism, chivalry, piety, tolerance);    
        }


        public static ReligionStats Create(ReligionStats parent, string stringid, Dictionary<string,int> stats, Dictionary<string, int> values, Dictionary<string, int> conversionReligionBonus, Dictionary<string, int> conversionCultureBonus)
        {
            var conversion = stats.ContainsKey("Conversion") ? stats["Conversion"] : parent.Conversion;
            var zeal = stats.ContainsKey("Zeal") ? stats["Zeal"] : parent.Zeal;

            var pacifism = values.ContainsKey("Pacifism") ? values["Pacifism"] : parent.Pacifism;
            var piety = values.ContainsKey("Piety") ? values["Piety"] : parent.Piety;
            var tolerance = values.ContainsKey("Tolerance") ? values["Tolerance"] : parent.Tolerance;
            var chivalry = values.ContainsKey("Chivalry") ? values["Chivalry"] : parent.Chivalry;

            if(parent.ConversionCultureBonus != null && !parent.ConversionCultureBonus.IsEmpty())
                conversionReligionBonus = parent.ConversionReligionBonus.Concat(conversionReligionBonus).GroupBy(kvp => kvp.Key).ToDictionary(g => g.Key, g => g.Last().Value);
            
            if(parent.ConversionReligionBonus != null && !parent.ConversionReligionBonus.IsEmpty())
                conversionCultureBonus = parent.ConversionCultureBonus.Concat(conversionCultureBonus).GroupBy(kvp => kvp.Key).ToDictionary(g => g.Key, g => g.Last().Value);

            return new ReligionStats(stringid, conversion, 0, zeal, 0, conversionReligionBonus, conversionCultureBonus, pacifism, chivalry, piety, tolerance);
        }
    }


    public class ReligionStatContainer
    {
        // Base stats
        [SaveableProperty(1)]
        public ReligiousStat Pacifism { get; private set; } = new ReligiousStat(0);
        [SaveableProperty(2)]
        public ReligiousStat Zeal { get; private set; } = new ReligiousStat(0);
        [SaveableProperty(3)]
        public ReligiousStat Chivalry { get; private set; } = new ReligiousStat(0);
        [SaveableProperty(4)]
        public ReligiousStat Tolerance { get; private set; } = new ReligiousStat(0);
        //[SaveableProperty(5)]
        //public ReligiousStat Piety { get; private set; } = new ReligiousStat(ReligiousStatType.Skill, 0);
        //[SaveableProperty(6)]
        //public ReligiousStat Evangelism { get; private set; } = new ReligiousStat(ReligiousStatType.Skill, 0);
        [SaveableProperty(5)]
        public ReligiousStat Conversion { get; private set; } = new ReligiousStat(ReligiousStatType.Skill, 5);

        [SaveableProperty(6)]
        public Dictionary<string, ReligionStatContainer> Modifiers { get; private set; }
        public ReligionStatContainer(Dictionary<string, int> religiousStats = null)
        {
           
            var Properties = this.GetType().GetProperties();
            int value = 0;
            double multiplier = 0;
            int bonus = 0;
            ReligiousStatType statType = ReligiousStatType.Belief;
            ReligiousStat currentStat=null;
            foreach(var Property in Properties)
            {
                currentStat = (ReligiousStat)Property.GetValue(this);
                if((religiousStats != null) && religiousStats.ContainsKey(Property.Name))
                {
                    if(currentStat.Value != religiousStats[Property.Name])
                    {
                        Property.SetValue(this, new ReligiousStat(currentStat.ReligiousStatType, religiousStats[Property.Name]));
                    }
                }
            }
        }

        public ReligionStatContainer(bool isModifier, Dictionary<string, ReligiousStat> religiousStats = null)
        {
            
            if(religiousStats != null || !religiousStats.IsEmpty()) {
                var Properties = this.GetType().GetProperties();
                ReligiousStat currentStat = null;
                foreach (var Property in Properties)
                {
                    if (religiousStats.ContainsKey(Property.Name))
                    {
                        Property.SetValue(this, religiousStats[Property.Name]);
                    }
                }
            }
        }

        public static ReligionStatContainer CreateModifier (Dictionary<string, int> bonuses = null, Dictionary<string, double> multipliers = null) {
            Dictionary<string, ReligiousStat> newStats = new Dictionary<string, ReligiousStat>();
            if(bonuses != null)
            {
                foreach (var key in bonuses.Keys)
                {
                    newStats.Add(key, new ReligiousStat(ReligiousStatType.Modifier, bonuses[key], (multipliers.ContainsKey(key) ? multipliers[key] : 0)));
                }

                foreach (var key in multipliers.Keys)
                {
                    if (!bonuses.ContainsKey(key))
                    {
                        newStats.Add(key, new ReligiousStat(ReligiousStatType.Modifier, multipliers[key]));
                    }
                }
            }
            return new ReligionStatContainer(true, newStats);
        }

        public static ReligionStatContainer CreateModifier(string stat, int bonus=0, double multiplier=0)
        {
            return new ReligionStatContainer(true, new Dictionary<string, ReligiousStat>() { { stat, new ReligiousStat(ReligiousStatType.Modifier, bonus, multiplier) } });   
        }

        public static ReligionStatContainer CreateModifier(string stat, double multiplier)
        {
            return ReligionStatContainer.CreateModifier(stat, 0, multiplier);
        }

        public static ReligionStatContainer CreateModifier(Dictionary<string, double> multipliers = null) {  return CreateModifier(null, multipliers); }
        
        public void AddModifier(string Source, ReligionStatContainer religionStatContainer)
        {

        }

        public void RemoveModifier(string Source, ReligionStatContainer religionStatContainer)
        {

        }

    }

    public enum ReligiousStatType
    {
        Belief,
        Skill,
        Modifier,
        Rate
    }

    public class ReligiousStat
    {
        [UsedImplicitly]
        [SaveableProperty(1)]
        public int Value { get; private set; } = 0;
        [UsedImplicitly]
        [SaveableProperty(2)]
        public double Multiplier { get; private set; } = 1.0;
        [UsedImplicitly]
        [SaveableProperty(3)]
        public int Modifier { get; private set; } = 0;

        private double _rateValue;
        public ReligiousStatType ReligiousStatType { get; private set; }
        public ReligiousStat(ReligiousStatType religiousStatType, int value, double multiplier)
        {
            this.ReligiousStatType = religiousStatType;
            if (religiousStatType == ReligiousStatType.Belief)
            {
                Value = value <= -100 ? -100 : value <= 100 ? 100 : value;
            } else if (religiousStatType == ReligiousStatType.Modifier) {
                Multiplier = 1.0 + (multiplier >= 1.0 ? multiplier : multiplier <= -1.0 ? -1.0 : multiplier);
                Modifier = value <= -100 ? -100 : value <= 100 ? 100 : value;
            }
            else if (religiousStatType == ReligiousStatType.Skill)
            {
                Value = value <= 330 ? 330 : value;
            }
        }
        public ReligiousStat(ReligiousStatType religiousStatType, int value) : this(religiousStatType, value, 0) { }

        public ReligiousStat(ReligiousStatType religiousStatType, double multiplier) : this(religiousStatType, 0, multiplier) { }

        public ReligiousStat(int value) : this(ReligiousStatType.Belief, value, 0) { }

        public static implicit operator int(ReligiousStat score)
        {
            if(score?.ReligiousStatType != ReligiousStatType.Modifier)
                return score?.Value ?? 0;
            else
            {
                return score?.Modifier ?? 0;
            }
        }

        public static implicit operator double(ReligiousStat score)
        {
            return score?.Multiplier ?? 0;
        }

        public double RateValue{
            get
            {
                return _rateValue;
            }
        }
    }


        public class ReligiousPerk
    {
        private string _name;
        private string _stringid;
        private ReligionStatContainer _stat;
        public string Name { get { return _name;  }}
        public ReligionStatContainer ReligionStats { get { return _stat; } }
        public ReligiousPerk(string name, ReligionStatContainer stats)
        {
            _name = name;
            _stat = stats;
        }
    }
}
