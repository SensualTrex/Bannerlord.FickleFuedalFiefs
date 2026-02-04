using Religions.DiplomaticAction.Alliance.Conditions;

using System.Collections.Generic;

namespace Religions.DiplomaticAction.Alliance
{
    class BreakAllianceConditions : AbstractConditionEvaluator<BreakAllianceConditions>
    {
        private static readonly List<IDiplomacyCondition> Conditions = new()
        {
            new TimeElapsedSinceAllianceFormedCondition()
        };
        public BreakAllianceConditions() : base(Conditions) { }
    }
}