using TaleWorlds.CampaignSystem;
using TaleWorlds.SaveSystem;
using System.Collections.Generic;

namespace Religions.Religion
{
    public class FactionReligionManager
    {
        public static FactionReligionManager Instance { get; private set; }

        [SaveableProperty(1)]
        public Dictionary<string, FactionReligionProfile> Profiles { get; private set; }

        public FactionReligionManager()
        {
            Profiles = new Dictionary<string, FactionReligionProfile>();
            Instance = this;
        }

        public FactionReligionProfile GetProfile(string factionId)
        {
            if (!Profiles.ContainsKey(factionId))
                Profiles[factionId] = new FactionReligionProfile(factionId);

            return Profiles[factionId];
        }

        public FactionReligionProfile GetProfile(IFaction faction)
        {
            return GetProfile(faction.StringId);
        }

        public void SetPrimaryReligion(IFaction faction, string religionId)
        {
            var profile = GetProfile(faction);
            profile.SetPrimaryReligion(religionId);
        }

        public void AllowReligion(IFaction faction, string religionId)
        {
            GetProfile(faction).AllowReligion(religionId);
        }

        public void BanReligion(IFaction faction, string religionId)
        {
            GetProfile(faction).BanReligion(religionId);
        }

        public bool IsReligionAllowed(IFaction faction, string religionId)
        {
            var profile = GetProfile(faction);
            if (profile == null)
                return false;

            return profile.IsAllowed(religionId);
        }

        public bool IsReligionBanned(IFaction faction, string religionId)
        {
            var profile = GetProfile(faction);
            if (profile == null)
                return false;

            return profile.IsBanned(religionId);
        }

        public void UpdateFactionReputation(IFaction faction, string religionId, int amount)
        {
            var profile = GetProfile(faction);
            if (profile == null)
                return;

            profile.AdjustReputation(religionId, amount);
        }
    }
}
