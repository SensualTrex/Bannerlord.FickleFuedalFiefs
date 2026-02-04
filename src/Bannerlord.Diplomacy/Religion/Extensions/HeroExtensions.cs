using System;
using System.Collections.Generic;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace Religions.Religion.Extensions
{
    public static class HeroExtensions
    {
        public static HeroReligionContainer GetHeroReligion (this Hero hero)
        {
            if (ReligionManager.Instance == null)
                return null;
            return (ReligionManager.Instance.HeroReligionTable.ContainsKey(hero.StringId) ? ReligionManager.Instance.HeroReligionTable[hero.StringId] : null);
        }


        public static void SetHeroReligion (this Hero hero, string heroReligionId)
        {
            if (ReligionManager.Instance == null)
                return;
            if(ReligionManager.Instance.HeroReligionTable.ContainsKey(hero.StringId) == false)
            {
                var newHeroReligion = new HeroReligionContainer(hero.StringId, heroReligionId, 0, 0, 0, 0, 0, 0, 0);
                ReligionManager.Instance.HeroReligionTable.Add(hero.StringId, newHeroReligion);
                return;
            }
            ReligionManager.Instance.HeroReligionTable[hero.StringId].SetHeroReligion(heroReligionId);
        }
    }
}
