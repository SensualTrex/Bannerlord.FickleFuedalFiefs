using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaleWorlds.Core;
using TaleWorlds.SaveSystem;

namespace Religions.Religion
{
    public class ReligionStats
    {
        [SaveableProperty(1)]
        public string StringId { get; private set; }




        [SaveableProperty(8)]
        public int Pacifism { get; private set; } = 0;
        [SaveableProperty(9)]
        public int Tolerance { get; private set; } = 0;
        [SaveableProperty(10)]
        public int Honor { get; private set; } = 0;
        [SaveableProperty(11)]
        public int Piety { get; private set; } = 0;

        public ReligionStats()
        {

        }

        public ReligionStats(string stringid, int conversion, double conversionMultiplier, int zeal, double zealMultiplier, Dictionary<string, int> conversionReligionBonus, Dictionary<string, int> conversionCultureBonus, int pacifism, int honor, int piety, int tolerance)
        {
            /* StringId = stringid;
            Conversion = conversion;
            ConversionMultiplier = conversionMultiplier;
            Zeal = zeal;
            ZealMultiplier = zealMultiplier;
            ConversionReligionBonus = conversionReligionBonus ?? new Dictionary<string, int>();
            ConversionCultureBonus = conversionCultureBonus ?? new Dictionary<string, int>();
            Pacifism = pacifism;
            Honor = Honor;
            Piety = piety;
            Tolerance = tolerance;*/
        }

        public static ReligionStats Create(string stringid, int conversion, int zeal, Dictionary<string, int> conversionReligionBonus, Dictionary<string, int> conversionCultureBonus, int pacafism, int chivalry, int piety, int tolerance)
        {
            return new ReligionStats(stringid, conversion, 0, zeal, 0, conversionReligionBonus, conversionCultureBonus, pacafism, chivalry, piety, tolerance);
        }


        /*public static ReligionStats Create(ReligionStats parent, string stringid, Dictionary<string, int> stats, Dictionary<string, int> values, Dictionary<string, int> conversionReligionBonus, Dictionary<string, int> conversionCultureBonus)
        {
            var conversion = stats.ContainsKey("Conversion") ? stats["Conversion"] : parent.Conversion;
            var zeal = stats.ContainsKey("Zeal") ? stats["Zeal"] : parent.Zeal;

            var pacifism = values.ContainsKey("Pacifism") ? values["Pacifism"] : parent.Pacifism;
            var piety = values.ContainsKey("Piety") ? values["Piety"] : parent.Piety;
            var tolerance = values.ContainsKey("Tolerance") ? values["Tolerance"] : parent.Tolerance;
            var chivalry = values.ContainsKey("Honor") ? values["Honor"] : parent.Honor;

            if (parent.ConversionCultureBonus != null && !parent.ConversionCultureBonus.IsEmpty())
                conversionReligionBonus = parent.ConversionReligionBonus.Concat(conversionReligionBonus).GroupBy(kvp => kvp.Key).ToDictionary(g => g.Key, g => g.Last().Value);

            if (parent.ConversionReligionBonus != null && !parent.ConversionReligionBonus.IsEmpty())
                conversionCultureBonus = parent.ConversionCultureBonus.Concat(conversionCultureBonus).GroupBy(kvp => kvp.Key).ToDictionary(g => g.Key, g => g.Last().Value);

            return new ReligionStats(stringid, conversion, 0, zeal, 0, conversionReligionBonus, conversionCultureBonus, pacifism, chivalry, piety, tolerance);
        }*/

    }
}
