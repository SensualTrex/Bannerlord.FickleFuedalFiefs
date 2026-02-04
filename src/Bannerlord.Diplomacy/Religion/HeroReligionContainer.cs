using JetBrains.Annotations;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaleWorlds.CampaignSystem;

using TaleWorlds.SaveSystem;
using Religions.Religion.Extensions;
using TaleWorlds.MountAndBlade;

namespace Religions.Religion
{
    public class HeroReligionContainer
    {
        [UsedImplicitly]
        [SaveableProperty(1)]
        public string ReligionId { get; private set; }
        [UsedImplicitly]
        [SaveableProperty(2)]
        public string HeroId { get; private set; }
        // Skills
        // Evangelism - Affects Conversion
        [UsedImplicitly]
        [SaveableProperty(3)]
        public int Evangelism { get; private set; } = 0;
        // Populism - Affects Zeal
        [UsedImplicitly]
        [SaveableProperty(4)]
        public int Populism { get; private set; } = 0;
        // Exemplarism - Affects Conversion, while increasing Piety
        [UsedImplicitly]
        [SaveableProperty(5)]
        public int Exemplarism { get; private set; } = 0;


        // Traits
        // -3 to 3
        [UsedImplicitly]
        [SaveableProperty(6)]
        public int Tolerance { get; private set; } = 0;
        [SaveableProperty(7)]
        public int Honor { get; private set; } = 0;
        [SaveableProperty(8)]
        public int Empathy { get; private set; } = 0;
        [UsedImplicitly]
        [SaveableProperty(9)]
        public int Piety { get; private set; } = 0;

        [UsedImplicitly]
        [SaveableProperty(10)]
        public int Faith { get; private set; } = 0;

        public HeroReligionContainer(string heroId, string religionId, int evangelism, int populism, int exemplarism, int tolerance, int honor, int empathy, int piety)
        {
            ReligionId = religionId;
            Evangelism = evangelism;
            Populism = populism;
            Exemplarism = exemplarism;
            Tolerance = tolerance;
            Honor = honor;
            Empathy = empathy;
            Piety = piety;
            Faith = 50;
        }

        public static HeroReligionContainer Create(Hero hero)
        {
            var heroCulture = hero.Culture.StringId;
            var heroReligion = "Undefined";

            int evangelism = 0;
            int populism = 0;
            int exemplarism = 0;
            int tolerance = 0;
            int honor = 0;
            int empathy = 0;
            int piety = 0;

            // 1. Default to father's religion
            if (hero.Father != null && hero.Father.GetHeroReligion() != null)
                heroReligion = hero.Father.GetHeroReligion().ReligionId;

            // 2. Marriage override (culture shift + spouse religion)
            if (hero.Spouse != null && hero.Clan.Culture != hero.Culture)
            {
                heroCulture = hero.Clan.Culture.StringId;

                if (hero.Spouse.GetHeroReligion() != null)
                    heroReligion = hero.Spouse.GetHeroReligion().ReligionId;
            }

            // 3. If still undefined, use the new generator
            if (heroReligion == "Undefined")
                heroReligion = HeroReligionGenerator.GenerateReligionForHero(hero);

            // 4. Final fallback (should rarely happen)
            if (heroReligion == "Undefined")
                heroReligion = ReligionFactory.Religions.Keys.First();

            // 5. Roll traits
            evangelism = GenerateTrait();
            populism = GenerateTrait();
            exemplarism = GenerateTrait();
            tolerance = GenerateTrait();

            return new HeroReligionContainer(
                hero.StringId,
                heroReligion,
                evangelism,
                populism,
                exemplarism,
                tolerance,
                honor,
                empathy,
                piety
            );
        }

        public static int GenerateTrait()
        {
            var dice = new Random();
            return (dice.Next(2) + dice.Next(2) + dice.Next(2) - 3);
        }


        public void UpdateStat(string statName, int newValue)
        {

        }

        public void SetHeroReligion(string ReligionId)
        {
            this.ReligionId = ReligionId;
        }

        
    }

}
