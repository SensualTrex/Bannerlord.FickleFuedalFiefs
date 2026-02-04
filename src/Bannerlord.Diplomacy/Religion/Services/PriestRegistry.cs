using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;

namespace Religions.Religion.Services
{
    public static class PriestRegistry
    {
        private static Dictionary<(string SettlementId, string ReligionId), string> _priestIds
            = new Dictionary<(string, string), string>();

        public static Hero GetPriest(Settlement settlement, string ReligionId)
        {
            if (_priestIds.TryGetValue((settlement.StringId, ReligionId), out var heroId))
                return Hero.FindFirst(h => h.StringId == heroId);

            return null;
        }

        public static void Register(Hero priest, Settlement settlement, string ReligionId)
        {
            _priestIds[(settlement.StringId, ReligionId)] = priest.StringId;
        }

        public static void Sync(IDataStore store)
        {
            store.SyncData("_priestIds", ref _priestIds);
        }
    }
}
