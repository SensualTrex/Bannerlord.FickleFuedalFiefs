using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomacy.Religions
{
    public enum ReligiousBuildingLevel
    {
        None,
        Level1,
        Level2,
        Level3
    }

    public enum ReligiousBuildingType
    {
        Temple,
        Monestary,
        Monument
    }

    public class ReligiousBuilding
    {

        public string Name { get; }
        public string Description { get; }
        public Religion AssociatedReligion { get; }
        
        public Dictionary<string, string> Features = new Dictionary<string, string>();
        public Dictionary<int, ReligiousBuildingLevel> Level { get; }
        public ReligiousBuilding(string name, string description, Religion associatedReligion)
        {

            Name = name;
            Description = description;
            AssociatedReligion = associatedReligion;
        }
    }

    public class ReligiousBuildingDescription
    {
        public string BuildingId { get; private set; }
        public string BuildingName { get; private set; }

        public string Description { get; private set; }

        public string ReligionId { get; private set; }
        
        public ReligiousBuildingLevel Level { get; private set; }

        public ReligiousBuildingType BuildingType { get; private set; }

        public ReligionStatContainer ReligionStats { get; private set; }

        public ReligiousBuildingDescription(string buldingId, string buldingName, string description, string religionId, ReligiousBuildingType type, ReligiousBuildingLevel level, ReligionStatContainer religionStat)

        {
            BuildingId = buldingId;
            BuildingName = buldingName;
            Description = description;
            ReligionId = religionId;
            Level = level;
            ReligionStats = religionStat;
            BuildingType = type;
        }
        

    }
}
