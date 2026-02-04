using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaleWorlds.CampaignSystem;

namespace Religions.Religion.Extensions
{
    public static class HeroReligionContainerExtension
    {
        public static Religion GetReligion(this HeroReligionContainer heroReligionContainer)
        {
            if (ReligionManager.Instance == null)
                return null;
            return (ReligionManager.Instance.AllReligions.TryGetValue(heroReligionContainer.ReligionId, out var religion)) ? religion : null;
        }
    }
}
