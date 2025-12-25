using Diplomacy.PatchTools;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.CampaignSystem.Settlements;

namespace Diplomacy.Patches
{
    internal sealed class SettlementPatch : PatchClass<SettlementPatch>
    {
        protected override IEnumerable<Patch> Prepare() => new Patch[]
        {
            //new Prefix(nameof(GetSettlment), "CurrentSettlement_getter"),
        };

        public void GetSettlment()
        {

        }
    }
}
