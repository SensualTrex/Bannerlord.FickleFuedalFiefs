using Religions.DiplomaticAction.GenericConditions;
using Religions.DiplomaticAction.NonAggressionPact;
using Religions.DiplomaticAction.WarPeace.Conditions;

using System.Collections.Generic;

namespace Religions.DiplomaticAction.WarPeace
{
    internal sealed class DeclareWarConditions : AbstractConditionEvaluator<DeclareWarConditions>
    {
        private static readonly List<IDiplomacyCondition> Conditions = new()
        {
            new HasEnoughInfluenceForWarCondition(),
            new NoNonAggressionPactCondition(),
            new NotInAllianceCondition(),
            new AtPeaceCondition(),
            new HasEnoughTimeElapsedForWarCondition(),
            new NotRebelKingdomCondition(),
            new NotEliminatedCondition(),
            new BadRelationCondition()
        };

        public DeclareWarConditions() : base(Conditions) { }
    }
}