using Religions.PatchTools;

using System.Collections.Generic;

using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement;

namespace Religions.Patches
{
    internal sealed class KingdomManagementVMPatch : PatchClass<KingdomManagementVMPatch>
    {
        protected override IEnumerable<Patch> Prepare()
        {
            return new Patch[]
            {
                new Prefix(nameof(FinalizeFix), typeof(KingdomManagementVM), "OnFinalize"),
            };
        }

        private static void FinalizeFix(KingdomManagementVM __instance)
        {
            __instance.Diplomacy.OnFinalize();
        }
    }
}