using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace Religions.Religion.Generators
{
    public static class HolyManGenerator
    {
        public static Hero GenerateHolyMan(
            string religionId,
            CultureObject culture,
            Settlement homeSettlement = null)
        {
            // 1. Pick a wanderer template for this culture
            CharacterObject template = CharacterObject.All
                .FirstOrDefault(c => c.Occupation == Occupation.Wanderer && c.Culture == culture)
                ?? CharacterObject.All.First(c => c.Occupation == Occupation.Wanderer);

            // 2. Create the hero (Bannerlord 1.3+ safe)
            Hero holyMan = HeroCreator.CreateSpecialHero(
                template,
                homeSettlement,
                null,
                null,
                MBRandom.RandomInt(25, 70)
            );

            // 3. Assign culture
            holyMan.Culture = culture;
            

            var first = NameGenerator.Current.GenerateHeroFirstName(holyMan);
            // 4. Generate name
            TextObject firstName;
            TextObject fullName;

            NameGenerator.Current.GenerateHeroNameAndHeroFullName(
                holyMan,
                out firstName,
                out fullName
            );
            holyMan.SetName(first, fullName);

            // 5. Skills
            
            holyMan.HeroDeveloper.SetInitialSkillLevel(DefaultSkills.Charm, 120);
            holyMan.HeroDeveloper.SetInitialSkillLevel(DefaultSkills.Leadership, 80);
            holyMan.HeroDeveloper.SetInitialSkillLevel(DefaultSkills.Medicine, 60);

            // 6. Traits
            holyMan.SetTraitLevel(DefaultTraits.Mercy, 1);
            holyMan.SetTraitLevel(DefaultTraits.Honor, 1);

            // 7. Activate
            holyMan.ChangeState(Hero.CharacterStates.Active);

            // 8. Place in world
            if (homeSettlement != null)
                holyMan.StayingInSettlement = homeSettlement;

            // 9. Register in your religion system
            //ReligionManager.Instance.RegisterHolyMan(holyMan, religionId);

            return holyMan;
        }
    }
}
