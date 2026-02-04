using Religions.Religion.Extensions;
using Religions.WarExhaustion.EventRecords;

using JetBrains.Annotations;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Xml.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.MountAndBlade;
using TaleWorlds.SaveSystem;

namespace Religions.Religion
{

    public enum ReligionRelationship
    {
        Neutral,
        Friendly,
        Hostile,
        OnSight,
        Allied,
        AtWar,
    }

    public enum ReligionHeirarchy
    {
        Decentralized,
        CentralizedLeader,
        CentralizedCouncil
    }

    public class Religion
    {
        [SaveableProperty(1)]
        public string ReligionId { get; private set; }

        [SaveableProperty(2)]
        public string Name { get; private set; }

        [SaveableProperty(3)]
        public int Tier { get; private set; }

        //[SaveableProperty(3)]
        //public string Factory { get; private set; }

        [SaveableProperty(4)]
        public ReligionHeirarchy Heirarchy { get; private set; }

        [SaveableProperty(5)]
        public Dictionary<string,bool> ReligionPolicies { get; private set; }

        [SaveableProperty(6)]
        public Dictionary<string, int> ReligionReputation { get; private set; }

        [SaveableProperty(7)]
        public Dictionary<string, int> ReligionHeroReputation { get; private set; }

        [SaveableProperty(8)]
        public Dictionary<string,ReligionRelationship> ReligionRelationships { get; private set; }

        [SaveableProperty(9)]
        public Dictionary<string, int> ReligionDoctrine { get; private set; }

        [SaveableProperty(10)]
        public int Pacifism { get; private set; } = 0;

        [SaveableProperty(11)]
        public int Tolerance { get; private set; } = 0;

        [SaveableProperty(12)]
        public int Honor { get; private set; } = 0;

        [SaveableProperty(13)]
        public int Piety { get; private set; } = 0;

        // Sects are faction related religious specific bonuses and policies

        [SaveableProperty(13)]
        public Dictionary<string, string> SectFactionMap { get; private set; }

        [SaveableProperty(14)]
        public Dictionary<string, Dictionary<string, int>> SectStatBonuses { get; private set; }

        [SaveableProperty(15)]
        public Dictionary<string, Dictionary<string, int>> SectDoctrineBonueses { get; private set; }

        [SaveableProperty(16)]
        public Dictionary<string, Dictionary<string, int>> SectReligionPolicies { get; private set; }


        public Religion(string name, string religionId, int tier, ReligionHeirarchy religionHeirarchy, Dictionary<string, bool> religionPolicies, Dictionary<string, ReligionRelationship> religionRelationships, Dictionary<string, int> religionDoctrine, Dictionary<string, int> religionStats)
        {
            Name = name;
            ReligionId = religionId;
            Tier = tier;
            Heirarchy = religionHeirarchy;
            ReligionPolicies = religionPolicies;
            ReligionRelationships = religionRelationships;
            if (ReligionRelationships == null)
                ReligionRelationships = new Dictionary<string, ReligionRelationship>();
            ReligionDoctrine = religionDoctrine;
            Pacifism = religionStats.ContainsKey("Pacifism") ? religionStats["Pacifism"] : 0;
            Tolerance = religionStats.ContainsKey("Tolerance") ? religionStats["Tolerance"] : 0;
            Honor = religionStats.ContainsKey("Honor") ? religionStats["Honor"] : 0;
            Piety = religionStats.ContainsKey("Piety") ? religionStats["Piety"] : 0;

        }

        public void UpdateHeroReputation(string heroId, int amount)
        {
            if (ReligionHeroReputation == null)
                ReligionHeroReputation = new Dictionary<string, int>();
            if (!ReligionHeroReputation.ContainsKey(heroId))
                ReligionHeroReputation[heroId] = 0;
            ReligionHeroReputation[heroId] += amount;
        }

        public void UpdateReligionReputation(string religionId, int amount)
        {
            if (ReligionHeroReputation == null)
                ReligionHeroReputation = new Dictionary<string, int>();
            if (!ReligionHeroReputation.ContainsKey(religionId))
                ReligionHeroReputation[religionId] = 0;
            ReligionHeroReputation[religionId] += amount;
        }

        public void CreateAlliance(string religionId)
        {
            if (!ReligionRelationships.ContainsKey(religionId))
                ReligionRelationships.Add(religionId, ReligionRelationship.Neutral);
            ReligionRelationships[religionId] = ReligionRelationship.Allied;
        }

        public void DeclareWar(string religionId)
        {
            if (!ReligionRelationships.ContainsKey(religionId))
                ReligionRelationships.Add(religionId, ReligionRelationship.Neutral);
            ReligionRelationships[religionId] = ReligionRelationship.AtWar;
        }

        public void MakePeace(string religionId)
        {     
            if (!ReligionRelationships.ContainsKey(religionId))
                ReligionRelationships.Add(religionId, ReligionRelationship.Neutral);
            ReligionRelationships[religionId] = ReligionRelationship.Neutral;
        }

    }

    public enum SpecialHeroType
    {
        None,
        Clergy,
        Prophit,
        Heretic
    }

   
    

}
