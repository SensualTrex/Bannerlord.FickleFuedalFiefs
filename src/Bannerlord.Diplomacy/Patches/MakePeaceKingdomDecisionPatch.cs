using Religions.DiplomaticAction.WarPeace;
using Religions.PatchTools;

using System.Collections.Generic;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Election;

namespace Religions.Patches
{
    internal sealed class MakePeaceKingdomDecisionPatch : PatchClass<MakePeaceKingdomDecisionPatch, MakePeaceKingdomDecision>
    {
        protected override IEnumerable<Patch> Prepare() => new Patch[]
        {
            new Prefix(nameof(ApplyChosenOutcomePrefix), "ApplyChosenOutcome")
        };

        private static bool ApplyChosenOutcomePrefix(DecisionOutcome chosenOutcome, MakePeaceKingdomDecision __instance, bool ____applyResults)
        {
            if (__instance.FactionToMakePeaceWith is not Kingdom kingdomToMakePeaceWith)
                return true;

            if (!____applyResults || !((MakePeaceKingdomDecision.MakePeaceDecisionOutcome) chosenOutcome).ShouldPeaceBeDeclared)
                return false;

            KingdomPeaceAction.ApplyPeace(__instance.Kingdom, kingdomToMakePeaceWith, dailyTribute: __instance.DailyTributeToBePaid, skipPlayerPrompts: true);
            return false;
        }
    }
}