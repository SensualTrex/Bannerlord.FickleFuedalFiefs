using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaleWorlds.CampaignSystem.Settlements;
using Newtonsoft.Json.Linq;
using TaleWorlds.CampaignSystem.GameComponents;

namespace Diplomacy.Religions
{
    internal class ReligionFactory
    {
        public ReligionFactory() {
            
        }
        public static Dictionary<string, Dictionary<string, int>> ReligionFactoryData = new Dictionary<string, Dictionary<string, int>>();
        public static Dictionary<string,  Religion> LoadReligions()
        {
            string filePath = AppContext.BaseDirectory + "..\\..\\Modules\\Bannerlord.Diplomacy\\ModuleData\\Database\\religionstats.json";
            
            Dictionary<string, Religion> religions = new Dictionary<string, Religion>();
            Dictionary<string, ReligionStats> religionsStats = new Dictionary<string, ReligionStats>();
            

            string jsonString = File.ReadAllText(filePath);
            dynamic obj = JsonConvert.DeserializeObject(jsonString);
           
            if (obj != null)
            {
                foreach (var religionStat in obj.ReligionStats)
                {
                    string ParentId = religionStat.ParentId;
                    string ReligionId = religionStat.StringId;
                    string Name = religionStat.Name;
                    var values = religionStat.Values != null ? (Dictionary<string, int>) religionStat.Values.ToObject<Dictionary<string, int>>() : new Dictionary<string, int>();
                    var mystats = religionStat.Stats != null ? (Dictionary<string, int>) religionStat.Stats.ToObject<Dictionary<string, int>>() : new Dictionary<string, int>();
                    var religionFactory = (Dictionary<string, int>)religionStat.CultureFactory.ToObject<Dictionary<string, int>>();
                    var religionBonus = (Dictionary<string, int>)religionStat.CultureBonus.ToObject<Dictionary<string, int>>();
                    var cultureBonus = (Dictionary<string, int>)religionStat.ReligionBonus.ToObject<Dictionary<string, int>>();
                    var parent = religionsStats.ContainsKey("ParentId") ? religionsStats["ParentId"] : new ReligionStats();

                    var ReligionStat = ReligionStats.Create(parent, ReligionId, mystats, values, religionBonus, cultureBonus);
                    religionsStats.Add(ReligionId, ReligionStat);
                    religions.Add(ReligionId, new Religion(Name, ReligionId, ReligionStat, religionFactory));

                    foreach(var culture in religionFactory.Keys)
                    {
                        if (!ReligionFactoryData.ContainsKey(culture))
                        {
                            ReligionFactoryData.Add(culture, new Dictionary<string, int>());
                        }
                        if (religionFactory[culture] > 0) {
                            ReligionFactoryData[culture].Add(culture, religionFactory[culture]);
                        }
                    }
                } 
            }

            return religions;
        }


    }
}
