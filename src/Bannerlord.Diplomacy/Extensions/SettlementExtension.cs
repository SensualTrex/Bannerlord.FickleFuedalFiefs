using Diplomacy.Religions;

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

namespace Diplomacy.Extensions
{
    public static class SettlementExtension 
    {
        private static Settlement _settlement;

        private static List<Religion> _settlementReligion = new List<Religion>();
        
        //private static Dictionary<string, >
        private static List<Religion>? GetSettlementReligion(this Settlement settlement)
        {
            return _settlementReligion;
        }

        public static SettlementReligionContainer GetSettlementReligionContainer(this Settlement settlement)
        {
            return ReligionManager.Instance?.SettlementReligions[settlement.StringId];
        }

        public static bool IsHolyCity(this Settlement settlement)
        { 
             return false;
            // var religion = settlement.GetSettlementReligion();
            // return religion is not null && settlement == religion.HolyCity;
        }
    }
}
