using TaleWorlds.CampaignSystem;

namespace Religions.Helpers
{
    internal static class CampaignTimeHelper
    {
        public static float ElapsedDaysBetween(CampaignTime endDate, CampaignTime srartDate) => (FieldAccessHelper.CampaignTimeNumTicksByRef(ref endDate) - FieldAccessHelper.CampaignTimeNumTicksByRef(ref srartDate)) / 8.64E+08f;
    }
}