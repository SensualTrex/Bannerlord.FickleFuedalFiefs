using Religions.CivilWar.Factions;

using TaleWorlds.CampaignSystem;

namespace Religions.CivilWar.Actions
{
    public class LeaveFactionAction
    {
        public static void Apply(Clan clan, RebelFaction rebelFaction)
        {
            rebelFaction.RemoveClan(clan);
        }
    }
}