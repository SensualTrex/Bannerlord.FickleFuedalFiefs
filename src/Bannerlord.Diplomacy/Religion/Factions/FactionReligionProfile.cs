using Newtonsoft.Json;

using Religions.Religion;
using Religions.Religion.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.SaveSystem;

namespace Religions.Religion
{
    public class FactionReligionProfile
    {
        // ------------------------------------------------------------
        //  Saveable Properties
        // ------------------------------------------------------------

        [SaveableProperty(1)]
        public string FactionId { get; private set; }

        // Kingdoms ALWAYS have a PrimaryReligion.
        // Clans MAY override it (optional).
        [SaveableProperty(2)]
        public string PrimaryReligionId { get; private set; }

        // Religions this faction tolerates.
        [SaveableProperty(3)]
        public HashSet<string> AllowedReligions { get; private set; }

        // Religions this faction rejects.
        [SaveableProperty(4)]
        public HashSet<string> BannedReligions { get; private set; }

        // Faction policies (string → bool)
        [SaveableProperty(5)]
        public Dictionary<string, bool> Policies { get; private set; }

        // Cached reputation with each religion (string → int)
        // This is NOT the religion’s reputation dictionary — this is the faction’s view.
        [SaveableProperty(6)]
        public Dictionary<string, int> ReligionReputation { get; private set; }

        // ------------------------------------------------------------
        //  Constructors
        // ------------------------------------------------------------

        /*public FactionReligionProfile()
        {
            AllowedReligions = new HashSet<string>();
            BannedReligions = new HashSet<string>();
            Policies = new Dictionary<string, bool>();
        }*/


        public FactionReligionProfile(string factionId)
        {
            FactionId = factionId;

            AllowedReligions = new HashSet<string>();
            BannedReligions = new HashSet<string>();
            Policies = new Dictionary<string, bool>();
            ReligionReputation = new Dictionary<string, int>();
        }


        [JsonConstructor]
        public FactionReligionProfile(
            string factionId,
            string primaryReligionId,
            IEnumerable<string> allowed,
            IEnumerable<string> banned,
            Dictionary<string, bool> policies)
        {
            FactionId = factionId;
            PrimaryReligionId = primaryReligionId;

            AllowedReligions = allowed != null ? new HashSet<string>(allowed) : new HashSet<string>();
            BannedReligions = banned != null ? new HashSet<string>(banned) : new HashSet<string>();
            Policies = policies ?? new Dictionary<string, bool>();
            ReligionReputation = new Dictionary<string, int>();
        }

        public void InitializePolicies(Dictionary<string, bool> defaultPolicies)
        {
            if (Policies == null)
                Policies = new Dictionary<string, bool>();

            foreach (var kvp in defaultPolicies)
            {
                if (!Policies.ContainsKey(kvp.Key))
                    Policies[kvp.Key] = kvp.Value;
            }
        }


        // ------------------------------------------------------------
        //  Primary Religion
        // ------------------------------------------------------------

        public void SetPrimaryReligion(string religionId)
        {
            PrimaryReligionId = religionId;
        }

        public bool HasPrimaryReligion => !string.IsNullOrEmpty(PrimaryReligionId);

        // ------------------------------------------------------------
        //  Allowed / Banned Management
        // ------------------------------------------------------------

        public bool IsAllowed(string religionId)
        {
            if (religionId == PrimaryReligionId)
                return true;

            return AllowedReligions.Contains(religionId);
        }

        public bool IsBanned(string religionId)
        {
            return BannedReligions.Contains(religionId);
        }

        public void AllowReligion(string religionId)
        {
            if (BannedReligions.Contains(religionId))
                BannedReligions.Remove(religionId);

            AllowedReligions.Add(religionId);
        }

        public void BanReligion(string religionId)
        {
            if (AllowedReligions.Contains(religionId))
                AllowedReligions.Remove(religionId);

            BannedReligions.Add(religionId);
        }

        public void ClearReligionStatus(string religionId)
        {
            AllowedReligions.Remove(religionId);
            BannedReligions.Remove(religionId);
        }

        // ------------------------------------------------------------
        //  Policy Management
        // ------------------------------------------------------------

        public void SetPolicy(string policyId, bool value)
        {
            Policies[policyId] = value;
        }

        public bool GetPolicy(string policyId)
        {
            return Policies.TryGetValue(policyId, out bool value) && value;
        }

        // ------------------------------------------------------------
        //  Reputation Management
        // ------------------------------------------------------------

        public void AdjustReputation(string religionId, int amount)
        {
            if (!ReligionReputation.ContainsKey(religionId))
                ReligionReputation[religionId] = 0;

            ReligionReputation[religionId] += amount;
        }

        public int GetReputation(string religionId)
        {
            return ReligionReputation.TryGetValue(religionId, out int rep) ? rep : 0;
        }

        // ------------------------------------------------------------
        //  Policy Alignment Logic
        // ------------------------------------------------------------

        public int CalculatePolicyAlignmentScore(Religion religion)
        {
            if (religion.ReligionPolicies == null)
                return 0;

            int score = 0;

            foreach (var kvp in religion.ReligionPolicies)
            {
                string policyId = kvp.Key;
                bool religionPrefers = kvp.Value;

                if (Policies.TryGetValue(policyId, out bool factionValue))
                {
                    if (factionValue == religionPrefers)
                        score += 5; // aligned
                    else
                        score -= 5; // misaligned
                }
            }

            return score;
        }

        // ------------------------------------------------------------
        //  Reevaluate Primary Religion (based on leader)
        // ------------------------------------------------------------
        public void ReevaluatePrimaryReligion(Hero newLeader)
        {
            if (newLeader == null)
                return;

            var leaderReligion = newLeader.GetHeroReligion()?.GetReligion();
            if (leaderReligion == null)
                return;

            // If the leader's religion is banned, unban it.
            if (IsBanned(leaderReligion.ReligionId))
                BannedReligions.Remove(leaderReligion.ReligionId);

            // If the leader's religion is not allowed, allow it.
            if (!IsAllowed(leaderReligion.ReligionId))
                AllowedReligions.Add(leaderReligion.ReligionId);

            // Set as primary if different.
            if (PrimaryReligionId != leaderReligion.ReligionId)
                PrimaryReligionId = leaderReligion.ReligionId;
        }



        // ------------------------------------------------------------
        //  Reevaluate Policies Based on Leader
        // ------------------------------------------------------------
        public void ReevaluatePoliciesBasedOnLeader(Hero newLeader)
        {
            if (newLeader == null)
                return;

            var leaderReligion = newLeader.GetHeroReligion().GetReligion();
            if (leaderReligion == null || leaderReligion.ReligionPolicies == null)
                return;

            foreach (var kvp in leaderReligion.ReligionPolicies)
            {
                string policyId = kvp.Key;
                bool religionPrefers = kvp.Value;

                // If faction has no explicit stance, adopt leader’s preference.
                if (!Policies.ContainsKey(policyId))
                {
                    Policies[policyId] = religionPrefers;
                    continue;
                }

                // If faction stance contradicts leader’s religion, adjust toward alignment.
                bool factionValue = Policies[policyId];

                if (factionValue != religionPrefers)
                {
                    // Soft alignment: 50% chance to flip
                    if (MBRandom.RandomFloat < 0.5f)
                        Policies[policyId] = religionPrefers;
                }
            }
        }


        // ------------------------------------------------------------
        //  Sync With Kingdom Profile (for clans)
        // ------------------------------------------------------------
        public void SyncWithKingdomProfile(FactionReligionProfile kingdomProfile)
        {
            if (kingdomProfile == null)
                return;

            // Inherit primary religion if clan has none.
            if (string.IsNullOrEmpty(PrimaryReligionId))
                PrimaryReligionId = kingdomProfile.PrimaryReligionId;

            // Inherit allowed religions (union)
            foreach (var r in kingdomProfile.AllowedReligions)
                AllowedReligions.Add(r);

            // Inherit banned religions (union)
            foreach (var r in kingdomProfile.BannedReligions)
                BannedReligions.Add(r);

            // Inherit policies (kingdom overrides clan)
            foreach (var kvp in kingdomProfile.Policies)
                Policies[kvp.Key] = kvp.Value;
        }




        // ------------------------------------------------------------
        //  Debug / Utility
        // ------------------------------------------------------------

        public override string ToString()
        {
            return $"FactionReligionProfile({FactionId}) Primary={PrimaryReligionId}";
        }
    }
}
