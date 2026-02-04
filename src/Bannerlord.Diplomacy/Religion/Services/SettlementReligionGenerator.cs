using Religions.Religion.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;


namespace Religions.Religion.Services
{
    /// <summary>
    /// Generates the INITIAL religious population distribution for settlements.
    /// This does NOT handle conversion, spread, or post-conquest recalculation.
    /// </summary>
    public class SettlementReligionGenerator
    {
        private readonly Random _dice = new Random();

        /// <summary>
        /// Called once per settlement during world initialization.
        /// Builds the starting religious population using faction, culture,
        /// neighbor influence, and foreign seeds.
        /// </summary>
        public void GenerateSettlementReligion(SettlementReligionContainer container)
        {
            if (container == null || container.Settlement == null)
                return;

            var settlement = container.Settlement;
            var faction = settlement.MapFaction;
            var cultureId = settlement.Culture.StringId;

            // Load faction + culture profiles
            var factionProfile = ReligionFactory.FactionReligionProfiles.ContainsKey(faction.StringId)
                ? ReligionFactory.FactionReligionProfiles[faction.StringId]
                : null;

            var cultureProfile = ReligionFactory.CultureReligionAffinity[cultureId];

            // Build weighted pool
            var weights = BuildReligionWeightPool(settlement, factionProfile, cultureProfile);

            // Normalize
            var normalized = NormalizeWeights(weights);

            // Assign population
            AssignPopulation(container, normalized);
        }

        // ----------------------------------------------------------------------
        // INTERNAL LOGIC
        // ----------------------------------------------------------------------

        private Dictionary<string, double> BuildReligionWeightPool(
            Settlement settlement,
            FactionReligionProfile factionProfile,
            CultureReligionAffinity cultureProfile)
        {
            Dictionary<string, double> weights = new Dictionary<string, double>();

            // 1. Faction influence (strong)
            if (factionProfile != null)
            {
                foreach (var religionId in factionProfile.AllowedReligions)
                {
                    if (!weights.ContainsKey(religionId))
                        weights[religionId] = 0;

                    weights[religionId] += 1.5; // flat faction influence
                }

            }

            // 2. Culture influence (baseline)
            foreach (var kv in cultureProfile.ReligionWeights)
            {
                if (!weights.ContainsKey(kv.Key))
                    weights[kv.Key] = 0;

                weights[kv.Key] += kv.Value;
            }

            // 3. Neighbor pressure (geographic influence)
            foreach (var neighbor in settlement.GetNearbySettlements())
            {
                var neighborContainer = neighbor.GetSettlementReligionContainer();
                if (neighborContainer == null)
                    continue;

                var dominant = neighborContainer.GetDominantReligion();
                if (dominant != null)
                {
                    if (!weights.ContainsKey(dominant))
                        weights[dominant] = 0;

                    weights[dominant] += 5; // small but meaningful
                }
            }

            // 4. Foreign seeds (tiny chance)
            foreach (var kv in cultureProfile.ForeignReligionSeeds)
            {
                if (!weights.ContainsKey(kv.Key))
                    weights[kv.Key] = 0;

                weights[kv.Key] += kv.Value;
            }

            return weights;
        }

        private Dictionary<string, double> NormalizeWeights(Dictionary<string, double> weights)
        {
            double total = weights.Values.Sum();
            Dictionary<string, double> normalized = new Dictionary<string, double>();

            foreach (var kv in weights)
            {
                normalized[kv.Key] = kv.Value / total;
            }

            return normalized;
        }

        private void AssignPopulation(SettlementReligionContainer container, Dictionary<string, double> normalized)
        {
            container.PopulationCount.Clear();
            container.SettlementReligions.Clear();

            double remaining = 1.0;

            foreach (var kv in normalized.OrderByDescending(x => x.Value))
            {
                double variance = 1 + ((_dice.Next(20) - 10) / 100.0); // ±10%
                double pop = Math.Round(kv.Value * variance, 4);

                if (pop > remaining)
                    pop = remaining;

                container.PopulationCount[kv.Key] = pop;
                remaining -= pop;

                // Create SettlementReligion entry
                var sr = new SettlementReligion(kv.Key, container);

                // Building logic (unchanged from your original)
                if (pop <= 0.25)
                    sr.AddBuilding(ReligionBuildingType.Temple, _dice.NextDouble() <= 0.25 ? ReligiousBuildingLevel.Level1 : ReligiousBuildingLevel.None);
                else if (pop <= 0.5)
                    sr.AddBuilding(ReligionBuildingType.Temple,
                        _dice.NextDouble() <= 0.12 ? ReligiousBuildingLevel.Level2 :
                        _dice.NextDouble() <= 0.5 ? ReligiousBuildingLevel.Level1 :
                        ReligiousBuildingLevel.None);
                else
                    sr.AddBuilding(ReligionBuildingType.Temple,
                        _dice.NextDouble() <= 0.25 ? ReligiousBuildingLevel.Level2 : ReligiousBuildingLevel.Level1);

                container.SettlementReligions[kv.Key] = sr;

                if (remaining <= 0)
                    break;
            }

            // Fill leftover with Nonreligious
            if (remaining > 0)
            {
                if (!container.PopulationCount.ContainsKey("Nonreligious"))
                    container.PopulationCount["Nonreligious"] = 0;

                container.PopulationCount["Nonreligious"] += remaining;
            }
        }
    }
}