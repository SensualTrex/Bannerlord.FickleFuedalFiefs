using TaleWorlds.CampaignSystem;
using TaleWorlds.Localization;

namespace Religions
{
    interface IDiplomacyCondition
    {
        bool ApplyCondition(Kingdom kingdom, Kingdom otherKingdom, out TextObject? textObject, bool forcePlayerCharacterCosts = false, bool bypassCosts = false);
    }
}