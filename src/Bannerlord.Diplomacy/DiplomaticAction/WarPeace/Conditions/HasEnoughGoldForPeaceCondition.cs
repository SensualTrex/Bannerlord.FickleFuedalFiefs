using Religions.Costs;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Localization;

namespace Religions.DiplomaticAction.WarPeace.Conditions
{
    class HasEnoughGoldForPeaceCondition : AbstractCostCondition
    {
        protected override TextObject FailedConditionText => new(StringConstants.NotEnoughGold);

        protected override bool ApplyConditionInternal(Kingdom kingdom, Kingdom otherKingdom, ref TextObject? textObject, bool forcePlayerCharacterCosts = false)
        {
            var hasEnoughGold = DiplomacyCostCalculator.DetermineGoldCostForMakingPeace(kingdom, otherKingdom, forcePlayerCharacterCosts).CanPayCost();
            if (!hasEnoughGold)
            {
                textObject = FailedConditionText;
            }
            return hasEnoughGold;
        }
    }
}