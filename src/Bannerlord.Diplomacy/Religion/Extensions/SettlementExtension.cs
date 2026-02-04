using Religions.Religion;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace Religions.Religion.Extensions
{
    public static class SettlementExtension 
    {
        private static Settlement _settlement;

        private static List<Religion> _settlementReligion = new List<Religion>();
        
        public static SettlementReligion GetReligion(this Settlement settlement, string religionId)
        {
            return ReligionManager.Instance?.SettlementReligions[settlement.StringId].SettlementReligions[religionId];
        }

        public static SettlementReligionContainer GetSettlementReligionContainer(this Settlement settlement)
        {
            return ReligionManager.Instance?.SettlementReligions[settlement.StringId];
        }

        public static SettlementReligionContainer GetReligions(this Settlement settlement)
        {
            return ReligionManager.Instance?.SettlementReligions[settlement.StringId];
        }

        public static SettlementReligionContainer GetSettlementReligion(this Settlement settlement)
        {
            var container = ReligionManager.Instance?.SettlementReligions[settlement.StringId];
            if (container is null) return null;
            return container;
        }


        public static bool IsHolyCity(this Settlement settlement)
        { 
             return false;
            // var religion = settlement.GetSettlementReligion();
            // return religion is not null && settlement == religion.HolyCity;
        }

        public static IEnumerable<Settlement> GetNearbySettlements(this Settlement settlement, float radius = 40f)
        {
            return Settlement.All
                .Where(s =>
                    (s.IsTown || s.IsCastle) && 
                    s != settlement &&
                    s.GetPosition2D.Distance(settlement.GetPosition2D) <= radius);
        }
    }
}
