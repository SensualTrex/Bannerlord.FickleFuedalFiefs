using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.IO;

using TaleWorlds.Core;
using TaleWorlds.Library;
using Religions.Religion;

namespace Religions.Religion
{
    public static class ReligionFactory
    {
        // ----------------------------------------------------------------------
        // STATIC REGISTRIES
        // ----------------------------------------------------------------------

        public static Dictionary<string, Religion> Religions { get; private set; }
            = new Dictionary<string, Religion>();

        public static Dictionary<string, ReligionStats> ReligionStatsDict { get; private set; }
            = new Dictionary<string, ReligionStats>();

        public static Dictionary<string, FactionReligionProfile> FactionReligionProfiles { get; private set; }
            = new Dictionary<string, FactionReligionProfile>();

        public static Dictionary<string, CultureReligionAffinity> CultureReligionAffinity { get; private set; }
            = new Dictionary<string, CultureReligionAffinity>();

        // ----------------------------------------------------------------------
        // ENTRY POINT
        // ----------------------------------------------------------------------

        public static void LoadAll()
        {
            Religions.Clear();
            ReligionStatsDict.Clear();
            FactionReligionProfiles.Clear();
            CultureReligionAffinity.Clear();

            LoadReligionStats();              // existing gameplay stats
            LoadFactionProfiles();            // new
            LoadCultureAffinity();            // new
            // BuildReligionObjects();           // construct Religion instances
        }

        // ----------------------------------------------------------------------
        // LOADERS
        // ----------------------------------------------------------------------

        private static void LoadReligionStats()
        {
            string path = GetPath("religion_stats_v2.json");
            if (!File.Exists(path))
            {
                InformationManager.DisplayMessage(new InformationMessage($"[ReligionFactory] Missing file: {path}"));
                return;
            }

            var json = File.ReadAllText(path);
            var root = JObject.Parse(json);

            foreach (var entry in root["ReligionStats"])
            {
                string id = entry["StringId"]?.ToString();
                string name = entry["Name"]?.ToString();

                string parentId = entry["Parent"]?.ToString();
                string hierarchyType = entry["HierarchyType"]?.ToString();
                ReligionHeirarchy heirarchy = Enum.TryParse(hierarchyType, true, out ReligionHeirarchy result) ? result : ReligionHeirarchy.Decentralized;


                int tier = entry["Tier"]?.ToObject<int>() ?? 0;

                // Doctrine + Values
                var doctrine = entry["Doctrine"]?.ToObject<Dictionary<string, int>>()
                               ?? new Dictionary<string, int>();

                var values = entry["Values"]?.ToObject<Dictionary<string, int>>()
                             ?? new Dictionary<string, int>();

                var relations = entry["ReligionRelations"]?.ToObject<Dictionary<string, List<string>>>()
                                ?? new Dictionary<string, List<string>>();

                // ⭐ Flatten Policies
                var flatPolicies = new Dictionary<string, bool>();

                var policiesNode = entry["Policies"];
                if (policiesNode != null)
                {
                    foreach (var category in policiesNode.Children<JProperty>())
                    {
                        string categoryName = category.Name;

                        foreach (var policy in category.Value.Children<JProperty>())
                        {
                            string policyName = policy.Name;
                            bool value = policy.Value.ToObject<bool>();

                            string flatKey = $"Policy_{categoryName}_{policyName}";
                            flatPolicies[flatKey] = value;
                        }
                    }
                }


                // Handle inheritance
                Religion parent = null;
                if (!string.IsNullOrEmpty(parentId) && Religions.ContainsKey(parentId))
                    parent = Religions[parentId];
                

                // Create the object using your updated constructor/factory
                var religionObj = new Religion(
                    name: name,
                    religionId: id,
                    tier: tier,
                    religionHeirarchy: heirarchy,
                    religionPolicies: flatPolicies,
                    religionRelationships: new Dictionary<string, ReligionRelationship>(),
                    religionDoctrine: doctrine,
                    religionStats: values
                );

                Religions[id] = religionObj;
            }
        }

        private static void LoadFactionProfiles()
        {
            string path = GetPath("religion_faction_profile.json");
            if (!File.Exists(path))
                return;

            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<FactionReligionProfile>>(json);

            foreach (var profile in list) { 
                var defaultPolicies = Religions[profile.PrimaryReligionId].ReligionPolicies;
                profile.InitializePolicies(defaultPolicies);
                FactionReligionProfiles[profile.FactionId] = profile;
            }
        }

        private static void LoadCultureAffinity()
        {
            string path = GetPath("religion_culture_profile.json");
            if (!File.Exists(path))
                return;

            var json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<CultureReligionAffinity>>(json);

            foreach (var affinity in list)
                CultureReligionAffinity[affinity.CultureId] = affinity;
        }

        // ----------------------------------------------------------------------
        // BUILD RELIGION OBJECTS
        // ----------------------------------------------------------------------

        private static void BuildReligionObjects()
        {
            foreach (var kv in ReligionStatsDict)
            {
                string id = kv.Key;
                ReligionStats stats = kv.Value;

                // Name comes from stats JSON for now
                string name = stats.StringId;

                // Factory = culture weighting (from CultureReligionAffinity)
                Dictionary<string, int> factory = BuildFactoryForReligion(id);

                //var religion = new Religion(name, id, stats, factory);
                //religion.CreateInitialPolicies();

                //Religions[id] = religion;
            }
        }

        private static Dictionary<string, int> BuildFactoryForReligion(string religionId)
        {
            Dictionary<string, int> factory = new Dictionary<string, int>();

            foreach (var culture in CultureReligionAffinity.Values)
            {
                if (culture.ReligionWeights.ContainsKey(religionId))
                    factory[culture.CultureId] = culture.ReligionWeights[religionId];
            }

            return factory;
        }

        // ----------------------------------------------------------------------
        // HELPERS
        // ----------------------------------------------------------------------

        private static string GetPath(string fileName)
        {
            return Path.Combine(AppContext.BaseDirectory,
                "..\\..\\Modules\\Bannerlord.Religions\\ModuleData\\Database\\", fileName);
        }
    }

    public class CultureReligionAffinity
    {
        public string CultureId { get; set; }
        public string DefaultReligion { get; set; }

        // Gameplay-only weighting for initial settlement generation
        public Dictionary<string, int> ReligionWeights { get; set; }
            = new Dictionary<string, int>();

        // Tiny seed chances for exotic religions
        public Dictionary<string, int> ForeignReligionSeeds { get; set; }
            = new Dictionary<string, int>();
    }

}