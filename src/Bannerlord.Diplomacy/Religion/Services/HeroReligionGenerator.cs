using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace Religions.Religion
{
    public static class HeroReligionGenerator
    {
        // ------------------------------------------------------------
        //  Entry Point
        // ------------------------------------------------------------
        public static string GenerateReligionForHero(Hero hero)
        {
            if (hero == null)
                return "Undefined";

            string cultureId = hero.Culture?.StringId ?? "unknown";

            // 1. Try culture affinity
            if (ReligionFactory.CultureReligionAffinity.TryGetValue(cultureId, out var affinity))
            {
                // 1a. Try weighted native religions
                string weighted = PickWeightedReligion(affinity.ReligionWeights);
                if (weighted != null)
                    return weighted;

                // 1b. Try foreign seeds
                string foreign = TryForeignSeed(affinity.ForeignReligionSeeds);
                if (foreign != null)
                    return foreign;

                // 1c. Fallback to default religion
                if (!string.IsNullOrEmpty(affinity.DefaultReligion))
                    return affinity.DefaultReligion;
            }

            // 2. Absolute fallback
            return "Undefined";
        }

        // ------------------------------------------------------------
        //  Weighted Religion Selection
        // ------------------------------------------------------------
        private static string PickWeightedReligion(Dictionary<string, int> weights)
        {
            if (weights == null || weights.Count == 0)
                return null;

            int total = weights.Values.Sum();
            if (total <= 0)
                return null;

            int roll = MBRandom.RandomInt(total);
            int cumulative = 0;

            foreach (var kvp in weights)
            {
                cumulative += kvp.Value;
                if (roll < cumulative)
                    return kvp.Key;
            }

            return weights.Keys.First();
        }

        // ------------------------------------------------------------
        //  Foreign Seed Chance
        // ------------------------------------------------------------
        private static string TryForeignSeed(Dictionary<string, int> seeds)
        {
            if (seeds == null || seeds.Count == 0)
                return null;

            foreach (var kvp in seeds)
            {
                string religionId = kvp.Key;
                int chance = kvp.Value;

                if (chance > 0 && MBRandom.RandomInt(100) < chance)
                    return religionId;
            }

            return null;
        }
    }
}