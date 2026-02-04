

using Religions.Religion;

using System;
using System.Collections.Generic;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.SaveSystem;

using static Religions.WarExhaustion.WarExhaustionManager;

namespace Religions.Religion
{
    public class FactionReligionManagerBehavior : CampaignBehaviorBase
    {
        // ---------------------------------------------------------
        //  SAVEABLE DATA
        // ---------------------------------------------------------

        [SaveableField(1)]
        private Dictionary<string, FactionReligionProfile> _clanProfiles;

        [SaveableField(2)]
        private Dictionary<string, FactionReligionProfile> _kingdomProfiles;

        // ---------------------------------------------------------
        //  CONSTRUCTOR
        // ---------------------------------------------------------

        public FactionReligionManagerBehavior()
        {
            _clanProfiles = new Dictionary<string, FactionReligionProfile>();
            _kingdomProfiles = new Dictionary<string, FactionReligionProfile>();
        }

        // ---------------------------------------------------------
        //  REQUIRED OVERRIDES
        // ---------------------------------------------------------
        public override void RegisterEvents()
        {

            // Kingdom lifecycle
            CampaignEvents.KingdomCreatedEvent.AddNonSerializedListener(this, OnKingdomCreated);
            CampaignEvents.KingdomDestroyedEvent.AddNonSerializedListener(this, OnKingdomDestroyed);

            // Clan lifecycle
            CampaignEvents.OnClanCreatedEvent.AddNonSerializedListener(this, OnClanCreated);
            CampaignEvents.OnClanDestroyedEvent.AddNonSerializedListener(this, OnClanDestroyed);
            CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, OnClanChangedKingdom);

            // Leadership changes
            CampaignEvents.OnClanLeaderChangedEvent.AddNonSerializedListener(this, OnClanLeaderChanged);
            CampaignEvents.RulingClanChanged.AddNonSerializedListener(this, OnRulingClanChanged);

            // Game lifecycle
            CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, OnNewGameCreated);
            CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, OnGameLoaded);
        }

        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData("_clanProfiles", ref _clanProfiles);
            dataStore.SyncData("_kingdomProfiles", ref _kingdomProfiles);
        }

        // ---------------------------------------------------------
        //  INITIALIZATION
        // ---------------------------------------------------------

        private void OnNewGameCreated(CampaignGameStarter starter)
        {
            InitializeAllFactions();
        }

        private void OnGameLoaded(CampaignGameStarter starter)
        {
            // Ensure dictionaries exist after load
            _clanProfiles ??= new Dictionary<string, FactionReligionProfile>();
            _kingdomProfiles ??= new Dictionary<string, FactionReligionProfile>();

            InitializeMissingFactions();
        }

        private void InitializeAllFactions()
        {
            foreach (var kingdom in Kingdom.All)
                EnsureKingdomProfile(kingdom);

            foreach (var clan in Clan.All)
                EnsureClanProfile(clan);
        }

        private void InitializeMissingFactions()
        {
            foreach (var kingdom in Kingdom.All)
                EnsureKingdomProfile(kingdom);

            foreach (var clan in Clan.All)
                EnsureClanProfile(clan);
        }

        // ---------------------------------------------------------
        //  PROFILE CREATION
        // ---------------------------------------------------------

        private void EnsureKingdomProfile(Kingdom kingdom)
        {
            if (kingdom == null) return;

            if (!_kingdomProfiles.ContainsKey(kingdom.StringId))
            {
                var profile = new FactionReligionProfile(kingdom.StringId);
                _kingdomProfiles.Add(kingdom.StringId, profile);
            }
        }

        private void EnsureClanProfile(Clan clan)
        {
            if (clan == null) return;

            if (!_clanProfiles.ContainsKey(clan.StringId))
            {
                var profile = new FactionReligionProfile(clan.StringId);
                _clanProfiles.Add(clan.StringId, profile);
            }
        }

        // ---------------------------------------------------------
        //  EVENT HANDLERS
        // ---------------------------------------------------------

        private void OnKingdomCreated(Kingdom kingdom)
        {
            EnsureKingdomProfile(kingdom);
        }

        private void OnKingdomDestroyed(Kingdom kingdom)
        {
            if (kingdom != null)
                _kingdomProfiles.Remove(kingdom.StringId);
        }

        private void OnClanCreated(Clan clan, bool isCompanion)
        {
            EnsureClanProfile(clan);
        }

        private void OnClanDestroyed(Clan clan)
        {
            if (clan != null)
                _clanProfiles.Remove(clan.StringId);
        }

        private void OnClanChangedKingdom(
            Clan clan,
            Kingdom oldKingdom,
            Kingdom newKingdom,
            ChangeKingdomAction.ChangeKingdomActionDetail detail,
            bool showNotification)
        {
            if (clan == null) return;

            EnsureClanProfile(clan);

            // Update allowed/banned lists based on new kingdom
            if (newKingdom != null)
            {
                EnsureKingdomProfile(newKingdom);
                SyncClanWithKingdom(clan, newKingdom);
            }
        }

            private void OnClanLeaderChanged(Hero oldLeader, Hero newLeader)
            {
                if (newLeader == null || newLeader.Clan == null)
                    return;

                Clan clan = newLeader.Clan;

                // Ensure profile exists
                EnsureClanProfile(clan);

                var profile = _clanProfiles[clan.StringId];

                // Update policies based on the new leader
                profile.ReevaluatePoliciesBasedOnLeader(newLeader);
            }


        private void OnRulingClanChanged(Kingdom kingdom, Clan newRulingClan)
        {
            if (kingdom == null || newRulingClan == null)
                return;

            EnsureKingdomProfile(kingdom);

            var profile = _kingdomProfiles[kingdom.StringId];
            profile.ReevaluatePrimaryReligion(newRulingClan.Leader);
        }

        // ---------------------------------------------------------
        //  SYNC LOGIC
        // ---------------------------------------------------------

        private void SyncClanWithKingdom(Clan clan, Kingdom kingdom)
        {
            if (clan == null || kingdom == null)
                return;

            EnsureClanProfile(clan);
            EnsureKingdomProfile(kingdom);

            var clanProfile = _clanProfiles[clan.StringId];
            var kingdomProfile = _kingdomProfiles[kingdom.StringId];

            clanProfile.SyncWithKingdomProfile(kingdomProfile);
        }


        // ---------------------------------------------------------
        //  PUBLIC API
        // ---------------------------------------------------------

        public FactionReligionProfile GetClanProfile(Clan clan)
        {
            EnsureClanProfile(clan);
            return _clanProfiles[clan.StringId];
        }

        public FactionReligionProfile GetKingdomProfile(Kingdom kingdom)
        {
            EnsureKingdomProfile(kingdom);
            return _kingdomProfiles[kingdom.StringId];
        }
    }
}