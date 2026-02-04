using Religions.Religion;
using Religions.Religion.Extensions;
using Religions.Religion.Services;

using System;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Religions.Religion.Behavior
{
    public class ReligionConversationBehavior : CampaignBehaviorBase
    {
        public override void RegisterEvents() { 
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, OnSessionLaunched);
        }

        public override void SyncData(IDataStore dataStore) { }

        public void OnSessionLaunched(CampaignGameStarter starter)
        {
            AddPriestConversation(starter);
        }

        private void AddPriestConversation(CampaignGameStarter starter)
        {
            // -------------------------------------------------
            // INTRO (NPC speaks → transitions to topics)
            // -------------------------------------------------
            starter.AddDialogFlow(
                DialogFlow.CreateDialogFlow("start")
                    .NpcLine("{=rel_priest_intro}Greetings, traveler.", null, null, "start", "priest_topics")
                    .Condition(() => IsPriestConversation())
                    .Consequence(() =>
                    {
                        var topic = ConsumeRequestedTopic();
                        if (topic == "conversion_oath")
                        {
                            // Force jump directly to oath
                            Campaign.Current.ConversationManager.ActiveToken =
                                Campaign.Current.ConversationManager.GetStateIndex("priest_oath_1");
                        }
                        else if (topic == "topic_blessing")
                        {
                            // Force jump directly to oath
                            Campaign.Current.ConversationManager.ActiveToken =
                                Campaign.Current.ConversationManager.GetStateIndex("priest_blessing");
                        }
                    })
            );

            // -------------------------------------------------
            // TOPIC MENU (Player chooses)
            // -------------------------------------------------
            starter.AddDialogFlow(
                DialogFlow.CreateDialogFlow("priest_topics")
                    // Convert
                    .PlayerLine("{=rel_priest_convert}I wish to convert to your faith.",
                        null,
                        "priest_topics",
                        "priest_oath_1")

                    // Ask questions
                    .PlayerLine("{=rel_priest_questions}Tell me more about your beliefs.",
                        null,
                        "priest_topics",
                        "priest_explain")

                    // Ask questions
                    .PlayerLine("{=rel_priest_pray}I would like to receive a blessing.",
                        null,
                        "priest_topics",
                        "priest_blessing")

                    .NpcLine("{=rel_priest_explain}Our faith teaches balance, duty, and the sacred flame that guides all.",
                        null,
                        null,
                        "priest_explain",
                        "priest_topics")

                    // Leave
                    .PlayerLine("{=rel_priest_leave}Perhaps another time.",
                        null,
                        "priest_topics",
                        "close_window")
            );


            // -------------------------------------------------
            // BLESSING MENU
            // -------------------------------------------------
            starter.AddDialogFlow(
                DialogFlow.CreateDialogFlow("priest_blessing")
                    .NpcLine("{=priest_blessing}Which blessing would you like to receive?",
                        null, null,
                        "priest_blessing",
                        "priest_blessing_options")
            );

            // -------------------------------------------------
            // BLESSING OPTIONS
            // -------------------------------------------------
            starter.AddDialogFlow(
                DialogFlow.CreateDialogFlow("priest_blessing_options")
                    .BeginPlayerOptions("priest_blessing_options")

                        .PlayerOption("{=bless_health}Blessing of Health",
                            null,
                            "priest_blessing_options",
                            "priest_bless_health")

                        .PlayerOption("{=bless_strength}Blessing of Strength",
                            null,
                            "priest_blessing_options",
                            "priest_bless_strength")

                        .PlayerOption("{=bless_fortune}Blessing of Fortune",
                            null,
                            "priest_blessing_options",
                            "priest_bless_fortune")

                        .PlayerOption("{=bless_leave}I need nothing more.",
                            null,
                            "priest_blessing_options",
                            "close_window")

                    .EndPlayerOptions()
            );

            // -------------------------------------------------
            // INDIVIDUAL BLESSINGS
            // -------------------------------------------------

            // Health
            starter.AddDialogFlow(
                DialogFlow.CreateDialogFlow("priest_bless_health")
                    .NpcLine("{=bless_health_desc}May vitality flow through your veins.",
                        null, null,
                        "priest_bless_health",
                        "priest_blessing_options")
                    .Consequence(() => ApplyHealthBlessing())
            );

            // Strength
            starter.AddDialogFlow(
                DialogFlow.CreateDialogFlow("priest_bless_strength")
                    .NpcLine("{=bless_strength_desc}May your arm strike true and strong.",
                        null, null,
                        "priest_bless_strength",
                        "priest_blessing_options")
                    .Consequence(() => ApplyStrengthBlessing())
            );

            // Fortune
            starter.AddDialogFlow(
                DialogFlow.CreateDialogFlow("priest_bless_fortune")
                    .NpcLine("{=bless_fortune_desc}May fortune smile upon your path.",
                        null, null,
                        "priest_bless_fortune",
                        "priest_blessing_options")
                    .Consequence(() => ApplyFortuneBlessing())
            );

            // -------------------------------------------------
            // OATH SEQUENCE (6 LINES, ALTERNATING)
            // -------------------------------------------------

            // Priest line 1
            starter.AddDialogFlow(
                DialogFlow.CreateDialogFlow("priest_oath_1")
                    .NpcLine("{=oath_1}Do you come freely to the sacred flame?",
                        null, null,
                        "priest_oath_1",
                        "priest_oath_2")
            );

            // Player line 2
            starter.AddDialogFlow(
                DialogFlow.CreateDialogFlow("priest_oath_2")
                    .PlayerLine("{=oath_2}I come freely, seeking truth.",
                        null,
                        "priest_oath_2",
                        "priest_oath_3")
            );

            // Priest line 3
            starter.AddDialogFlow(
                DialogFlow.CreateDialogFlow("priest_oath_3")
                    .NpcLine("{=oath_3}Do you vow to walk in balance and duty?",
                        null, null,
                        "priest_oath_3",
                        "priest_oath_4")
            );

            // Player line 4
            starter.AddDialogFlow(
                DialogFlow.CreateDialogFlow("priest_oath_4")
                    .PlayerLine("{=oath_4}I vow to walk the path set before me.",
                        null,
                        "priest_oath_4",
                        "priest_oath_5")
            );

            // Priest line 5
            starter.AddDialogFlow(
                DialogFlow.CreateDialogFlow("priest_oath_5")
                    .NpcLine("{=oath_5}Do you offer your spirit to be reshaped in faith?",
                        null, null,
                        "priest_oath_5",
                        "priest_oath_6")
            );

            // Player line 6
            starter.AddDialogFlow(
                DialogFlow.CreateDialogFlow("priest_oath_6")
                    .PlayerLine("{=oath_6}I offer it, without fear or hesitation.",
                        null,
                        "priest_oath_6",
                        "priest_oath_complete")
            );

            // -------------------------------------------------
            // OATH COMPLETE → APPLY CONVERSION → BLESSING MENU
            // -------------------------------------------------

            starter.AddDialogFlow(
                DialogFlow.CreateDialogFlow("priest_oath_complete")
                    .NpcLine("{=oath_complete}Then rise, reborn in the faith.",
                        null, null,
                        "priest_oath_complete",
                        "priest_blessing")
                    .Consequence(() => ApplyConversion())
            );

        }

        // -----------------------------
        // CONDITION: Is this a priest?
        // -----------------------------
        public static bool IsPriestConversation()
        {
            var priest = GenericPriestService.ActivePriest;
            var convo = CharacterObject.OneToOneConversationCharacter?.HeroObject;

            InformationManager.DisplayMessage(
                new InformationMessage("IsPriestConversation: TRUE")
            );


            return priest != null && convo == priest;
        }

        // -----------------------------
        // CONSEQUENCE: Apply conversion
        // -----------------------------
        public static void ApplyConversion()
        {
            try
            {
                var manager = ReligionManager.Instance;
                if (manager == null)
                {
                    InformationManager.DisplayMessage(
                        new InformationMessage("ReligionManager not initialized.")
                    );
                    return;
                }
                InformationManager.DisplayMessage(new InformationMessage($"CONVERTING: Religion set to {CurrentReligionId}"));
                Hero.MainHero.SetHeroReligion(CurrentReligionId);

                InformationManager.DisplayMessage(
                    new InformationMessage("You have embraced a new faith.")
                );
            }
            catch (Exception ex)
            {
                InformationManager.DisplayMessage(
                    new InformationMessage($"Conversion failed: {ex.Message}")
                );
            }
        }

        public static void StartPriestConversation(Settlement settlement, string religionId)
        {
            CurrentReligionId = religionId;
            InformationManager.DisplayMessage(new InformationMessage($"Religion set to {CurrentReligionId}"));
            var priest = GenericPriestService.GetOrCreatePriest(settlement, religionId);
            if (priest == null)
            {
                InformationManager.DisplayMessage(new InformationMessage("No priest found."));
                return;
            }

            // Start conversation
            CampaignMapConversation.OpenConversation( 
                new ConversationCharacterData(Hero.MainHero.CharacterObject),
                new ConversationCharacterData(priest.CharacterObject)
            );
        }

        private static string _requestedTopic;

        public static void SetRequestedTopic(string topic)
        {
            _requestedTopic = topic;
        }

        public static string ConsumeRequestedTopic()
        {
            var t = _requestedTopic;
            _requestedTopic = null;
            return t;
        }


        public void ApplyHealthBlessing()
        {
            // Placeholder: Implement health blessing logic
            InformationManager.DisplayMessage(new InformationMessage("You feel healthier."));
        }

        public void ApplyStrengthBlessing()
        {
            // Placeholder: Implement strength blessing logic
            InformationManager.DisplayMessage(new InformationMessage("You feel stronger."));
        }
        public void ApplyFortuneBlessing()
        {
            // Placeholder: Implement fortune blessing logic
            InformationManager.DisplayMessage(new InformationMessage("You feel luckier."));
        }

        public static string CurrentReligionId { get; set; }
    }
}