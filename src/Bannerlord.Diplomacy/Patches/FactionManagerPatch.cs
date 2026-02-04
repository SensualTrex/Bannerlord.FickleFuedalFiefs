using Bannerlord.UIExtenderEx.Attributes;

using Religions.PatchTools;

using Helpers;

using JetBrains.Annotations;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using TaleWorlds.CampaignSystem;

namespace Religions.Patches
{
    internal sealed class FactionManagerPatch : PatchClass<FactionManagerPatch, FactionManager>
    {
        protected override IEnumerable<Patch> Prepare() => new Patch[]
        {
             // new Prefix(nameof(DeclareAlliancePrefix), nameof(FactionHelper.DeclareAlliance)),
        };

        private static bool DeclareAlliancePrefix(IFaction faction1, IFaction faction2)
        {
            if (faction1 == faction2 || faction1.IsBanditFaction || faction2.IsBanditFaction)
                return false;

            //@TODO Fix Later
            var Kingdom1 = TaleWorlds.CampaignSystem.Kingdom.All.Where(k => k.Name == faction1.Name).ToList()[0];
            var Kingdom2 = TaleWorlds.CampaignSystem.Kingdom.All.Where(k => k.Name == faction2.Name).ToList()[0];

            Kingdom1.AlliedKingdoms.Add(Kingdom2);
            Kingdom1.UpdateAlliedKingdoms();
            return false;
        }

        private enum StanceType
        {
            [UsedImplicitly] Neutral,
            [UsedImplicitly] War,
            Alliance,
        }

        //private static readonly Func<IFaction, IFaction, StanceType, StanceLink> SetStance = new Reflect.Method<FactionManager>("SetStance").GetDelegate<Func<IFaction, IFaction, StanceType, StanceLink>>();
    }
}