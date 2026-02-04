using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.SaveSystem;
using Religions.Religion.Extensions;

namespace Religions.Religion
{
    public class SettlementReligionContainer
    {

        [SaveableProperty(1)]
        public Dictionary<string, double> PopulationCount { get; private set; }
        [SaveableProperty(2)]
        public Dictionary<string, double> ConversionRates { get; private set; }
        [SaveableProperty(3)]
        public Dictionary<string, SettlementReligion> SettlementReligions { get; private set; }
        [SaveableProperty(4)]
        public Settlement Settlement { get; private set; }
        [SaveableProperty(5)]
        public Dictionary<string, Dictionary<string, int>> ImmigrationScores { get; private set; }
        [SaveableField(6)]
        public double multiplier = 1000;
        [SaveableProperty(7)]
        public Dictionary<string, List<string>> ImmigrationMap { get; private set; }
        [SaveableProperty(8)]
        public Dictionary<string, bool> SettlementPolicies { get; private set; }

        public SettlementReligionContainer(Settlement settlement)
        {
            if (PopulationCount == null)
                PopulationCount = new Dictionary<string, double>();
            if (ConversionRates == null)
                ConversionRates = new Dictionary<string, double>();
            if (SettlementReligions == null)
                SettlementReligions = new Dictionary<string, SettlementReligion>();
            Settlement = settlement;
        }

        public void CalculateImmigrationScores()
        {
            ImmigrationMap = new Dictionary<string, List<string>>();
            var nearbySettlements = Settlement.GetNearbySettlements();
            foreach (var settlementNear in nearbySettlements)
            {
                var populationCount = settlementNear.GetSettlementReligion().PopulationCount;
                var majorReligions = populationCount.Where(x => x.Value >= 0.20);
                foreach (var religion in majorReligions)
                {
                    if (!ImmigrationMap.ContainsKey(religion.Key))
                    {
                        ImmigrationMap.Add(religion.Key, new List<string>());
                    }
                    if (!ImmigrationMap[religion.Key].Contains(settlementNear.StringId))
                    {
                        ImmigrationMap[religion.Key].Add(settlementNear.StringId);
                    }
                    /*if (!ImmigrationScores.ContainsKey(religion.Key))
                    {
                        ImmigrationScores.Add(religion.Key, new Dictionary<string, int>());
                    }
                    if (!ImmigrationScores[religion.Key].ContainsKey(settlementNear.StringId))
                    {
                        ImmigrationScores[religion.Key].Add(settlementNear.StringId, 1);
                    }*/

                }

            }
            foreach (var religion in ImmigrationMap)
            {
                if (!this.HasReligion(religion.Key))
                {
                    AddReligion(religion.Key, 0.0);
                }
                var ImmigrationScoreTotal = religion.Value.Count;
                SettlementReligions[religion.Key].SetImmigrationMap(ImmigrationMap[religion.Key]);
            }
        }

        public bool HasReligion(string religionId)
        {
            return PopulationCount.ContainsKey(religionId);
        }

        public void AddReligion(string religionId, double initialPopulation)
        {
            if (!PopulationCount.ContainsKey(religionId))
            {
                PopulationCount.Add(religionId, initialPopulation);
            }
            if (!SettlementReligions.ContainsKey(religionId))
            {
                var settlementReligion = new SettlementReligion(religionId, this);
                SettlementReligions.Add(religionId, settlementReligion);
            }
        }

        public double GetReligionPopulation(string religionId)
        {
            double population = 0;
            if (PopulationCount.ContainsKey(religionId))
            {
                population += PopulationCount[religionId];
            }
            return population;
        }

        public void CaluculateCurrentConversionRate()
        {
            if (multiplier == 0)

                multiplier = 1000;

            ConversionRates.Clear();
            double conversionScore = 0.0;
            foreach (var religionX in SettlementReligions)
            {
                conversionScore = 0.0;
                foreach (var religionY in SettlementReligions)
                {
                    var conversionScoreAdd = ((religionX.Value.GetCurrentConversionScore() - religionY.Value.GetCurrentConversionScore()) *
                        (PopulationCount[religionY.Key] + PopulationCount[religionX.Key])) / multiplier;
                    conversionScore += conversionScoreAdd;
                }
                ConversionRates.Add(religionX.Key, conversionScore);
            }
        }

        public void SettlementTick()
        {
            ConversionTick();
            BuildingTick();
        }


        public void ConversionTick()
        {
            //Caluclates immigration scores for first run
            if (ImmigrationScores == null)
                CalculateImmigrationScores();
            CaluculateCurrentConversionRate();
            foreach (var religion in ConversionRates.Keys)
            {
                if (!PopulationCount.ContainsKey(religion))
                {
                    PopulationCount.Add(religion, 0.0);
                }
                PopulationCount[religion] += ConversionRates[religion];
            }
            //Runs immigration score calculation after conversion
            CalculateImmigrationScores();
        }

        public void BuildingTick()
        {
            var SettlementProsperity = Settlement.Town.Prosperity;
            foreach (var religion in SettlementReligions)
            {
                int resources = (int) (SettlementProsperity * PopulationCount[religion.Key]);
                religion.Value.AddBuildingResources(resources);
            }
        }

        public string GetDominantReligion()
        {
            if (PopulationCount == null || PopulationCount.Count == 0)
                return null;

            // Return the religion with the highest population share
            return PopulationCount
                .OrderByDescending(kv => kv.Value)
                .First()
                .Key;
        }

    }
}
