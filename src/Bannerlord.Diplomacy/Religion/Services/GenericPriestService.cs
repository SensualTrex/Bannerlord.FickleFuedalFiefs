using System;
using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace Religions.Religion.Services
{
    public static class GenericPriestService
    {
        public static Hero ActivePriest { get; private set; }

        public static Hero GetOrCreatePriest(Settlement settlement, string religionId)
        {
            // 1. Check for existing priest of this religion in this settlement
            var existing = PriestRegistry.GetPriest(settlement, religionId);
            if (existing != null)
            {
                ActivePriest = existing;
                return existing;
            }

            // 2. Generate a new priest
            var template = SelectPriestTemplate(settlement.Culture.StringId, religionId);
            if (template == null)
                return null;

            int age = GeneratePriestAge();

            var priest = HeroCreator.CreateSpecialHero(
                template,
                settlement,
                Clan.PlayerClan,
                null,
                age
            );



            var isFemaleDice = new (bool isFemale, float Weight)[]
            {
            (true, 1f),
            (false, 5f)
            };

            priest.IsFemale=MBRandom.ChooseWeighted<bool>(isFemaleDice);

            if (priest != null)
            {
                var hall = settlement.LocationComplex.GetLocationWithId("lordshall");

                if (hall != null)
                {
                    hall.AddCharacter(
                        new LocationCharacter(
                            new AgentData(priest.CharacterObject),
                            null,   // no agent origin
                            null,   // no spawn point
                            default,
                            LocationCharacter.CharacterRelations.Neutral,
                            null,
                            true,   // isHidden = true (do NOT spawn in scene)
                            false   // isFixed = false
                        )
                    );
                }
            }



            OverridePriestName(priest, religionId);

            ApplyRuntimePriestAppearance(priest);

            PriestRegistry.Register(priest, settlement, religionId);

            ActivePriest = priest;
            return priest;
        }

        private static CharacterObject SelectPriestTemplate(string cultureId, string religionId)
        {
            // Filter wanderers by culture
            var candidates = CharacterObject.All
                .Where(co =>
                    co.IsTemplate &&
                    !co.IsHero &&
                    co.Occupation == Occupation.Wanderer &&
                    co.Culture?.StringId == cultureId
                )
                .ToList();

            if (candidates.Count == 0)
                return null;

            // Score based on traits + religion flavor (later JSON)
            var scored = candidates
                .Select(co => new { Template = co, Score = ScorePriestliness(co, religionId) })
                .OrderByDescending(x => x.Score)
                .ToList();

            return scored.First().Template;
        }

        private static int ScorePriestliness(CharacterObject co, string religionId)
        {
            int score = 0;

            string name = co.Name.ToString().ToLower();

            if (name.Contains("healer")) score += 5;
            if (name.Contains("scholar")) score += 4;
            if (name.Contains("surgeon")) score += 4;
            if (name.Contains("silent")) score += 3;

            score += Math.Max(0, co.GetTraitLevel(DefaultTraits.Mercy));
            score += Math.Max(0, co.GetTraitLevel(DefaultTraits.Generosity));
            score += Math.Max(0, co.GetTraitLevel(DefaultTraits.Honor));

            // Later: religion-specific scoring from JSON
            return score;
        }

        private static int GeneratePriestAge()
        {
            var weightedAges = new (int Age, float Weight)[]
            {
            (30, 1f),
            (40, 2f),
            (50, 3f),
            (60, 3f),
            (70, 2f),
            (80, 1f)
            };

            return MBRandom.ChooseWeighted(weightedAges);
        }

        public static void ApplyRuntimePriestAppearance(Hero priest)
        {
            if (priest == null)
                return;

            // -------------------------------------------------
            // EQUIPMENT (Civilian + Battle)
            // -------------------------------------------------
            // Create a fresh civilian equipment set
            var civilian = new Equipment(Equipment.EquipmentType.Civilian);

            // Assign items by string ID (replace with your own)
            civilian[EquipmentIndex.Body] =
                new EquipmentElement(MBObjectManager.Instance.GetObject<ItemObject>("robe_empire"));

            civilian[EquipmentIndex.Cape] =
                new EquipmentElement(MBObjectManager.Instance.GetObject<ItemObject>("hood"));

            //civilian[EquipmentIndex.Head] =
            //    new EquipmentElement(MBObjectManager.Instance.GetObject<ItemObject>("empire_crown_north"));

            // Clone for battle equipment
            var battle = civilian.Clone();

            // Assign to hero (this is the correct place — NOT CharacterObject)
            priest.CivilianEquipment.FillFrom(civilian);
            priest.BattleEquipment.FillFrom(battle);

            // -------------------------------------------------
            // BODY PROPERTIES (Beard, Hair, Age)
            // -------------------------------------------------
            // Get current body properties
            var body = priest.BodyProperties;

            // Modify beard/hair indices (culture‑dependent)
            // You can tweak these numbers later
            var newStatic = body.StaticProperties;
            var newDynamic = new DynamicBodyProperties(
                age: 55f,          // older priest
                weight: body.Weight,
                build: body.Build
            );

            // Beard/hair are encoded in StaticBodyProperties key parts
            // Easiest safe method: use FaceGen to apply tags
            var updated = FaceGen.GetRandomBodyProperties(
                priest.CharacterObject.Race,
                priest.IsFemale,
                priest.CharacterObject.GetBodyPropertiesMin(),
                priest.CharacterObject.GetBodyPropertiesMax(),
                hairCoverType: 0,
                seed: MBRandom.RandomInt(),
                hairTags: "hair_mature",     // optional tag
                beardTags: "beard_full",     // optional tag
                tatooTags: "",
                variationAmount: 0f
            );

            // Overwrite age but keep the randomized beard/hair
            updated = new BodyProperties(newDynamic, updated.StaticProperties);

            // Apply back to hero
            priest.StaticBodyProperties = updated.StaticProperties;
            priest.Weight = updated.Weight;
            priest.Build = updated.Build;
        }


        private static void OverridePriestName(Hero priest, string religionId)
        {
            var first = priest.FirstName?.ToString() ?? priest.Name.ToString();

            // Placeholder until JSON titles are ready
            var newName = new TextObject($"Priest");

            priest.SetName(newName, newName);
        }
    }
}