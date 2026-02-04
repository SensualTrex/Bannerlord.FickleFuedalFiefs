using TaleWorlds.CampaignSystem;

namespace Religions.Events
{
    public readonly struct WarExhaustionInitializedEvent
    {
        public WarExhaustionInitializedEvent(Kingdom kingdom, Kingdom otherKingdom)
        {
            Kingdom = kingdom;
            OtherKingdom = otherKingdom;
        }

        public Kingdom Kingdom { get; }
        public Kingdom OtherKingdom { get; }
    }
}