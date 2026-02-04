using TaleWorlds.Localization;

namespace Religions
{
    internal static class Compat
    {
        internal static class HintViewModel
        {
            public static TaleWorlds.Core.ViewModelCollection.Information.HintViewModel Create(TextObject text)
            {
                return new(text);
            }
        }
    }
}