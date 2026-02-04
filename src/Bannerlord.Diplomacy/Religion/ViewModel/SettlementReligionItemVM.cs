using HarmonyLib;

using NavalDLC.Storyline.Quests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.Core;
using TaleWorlds.Core.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

using static TaleWorlds.Core.Equipment;
using System.IO;
using Religions.Religion.Behavior;

namespace Religions.Religion.ViewModel
{
    public class SettlementReligionItemVM : TaleWorlds.Library.ViewModel
    {
        public MBBindingList<ItemValuePairVM> ItemProperties { get; set; }

        private readonly string lvlSpritePath = "SPGeneral\\TownManagement\\level_";
        //private readonly string lvlSpritePath = "PartyScreen\\FormationIcons\\";
        private readonly string cultureSpritePath = "_keep";

        private float _itemDetailsWidth = 175;
        private float _itemTotalWidth = 355;

        private string _levelSprite;
        private string _itemSprite;
        private string _itemName;
        private bool _showButtons = true;

        private string _itemLevelText;

        private bool _showIcon;

        private Settlement _settlement;
        private string _religionId;

        [DataSourceProperty]
        public bool ShowIcon
        {
            get
            {
                return _showIcon;
            }
            set
            {
                if (value != _showIcon)
                {
                    _showIcon = value;
                    base.OnPropertyChangedWithValue(value, "ShowIcon");
                }
            }
        }

        [DataSourceProperty]
        public float ItemDetailsWidth
        {
            get
            {
                return _itemDetailsWidth;
            }
            set
            {
                if (value != _itemDetailsWidth)
                {
                    _itemDetailsWidth = value;
                    base.OnPropertyChangedWithValue(value, "ItemDetailsWidth");
                }
            }
        }

        [DataSourceProperty]
        public float ItemTotalWidth
        {
            get
            {
                return _itemTotalWidth;
            }
            set
            {
                if (value != _itemTotalWidth)
                {
                    _itemTotalWidth = value;
                    base.OnPropertyChangedWithValue(value, "ItemTotalWidth");
                }
            }
        }

        [DataSourceProperty]
        public string ItemName
        {
            get
            {
                return _itemName;
            }
            set
            {
                if (value != _itemName)
                {
                    _itemName = value;
                    base.OnPropertyChangedWithValue<string>(value, "ItemName");
                }
            }
        }

        [DataSourceProperty]
        public string ItemLevelText
        {
            get
            {
                return _itemLevelText;
            }
            set
            {
                if (value != _itemLevelText)
                {
                    _itemLevelText = value;
                    base.OnPropertyChangedWithValue<string>(value, "ItemLevelText");
                }
            }
        }

        [DataSourceProperty]
        public string LevelSprite
        {
            get
            {
                return _itemSprite;
            }
            set
            {
                if (value != _itemSprite)
                {
                    _itemSprite = value;
                    base.OnPropertyChangedWithValue(value, "LevelSprite");
                }
            }
        }

        [DataSourceProperty]
        public string ItemSprite
        {
            get
            {
                return _levelSprite;
            }
            set
            {
                if (value != _levelSprite)
                {
                    _levelSprite = value;
                    base.OnPropertyChangedWithValue(value, "ItemSprite");
                }
            }
        }

        [DataSourceProperty]
        public bool ShowButtons
        {
            get
            {
                return _showButtons;
            }
            set
            {
                if (value != _showButtons)
                {
                    _showButtons = value;
                    base.OnPropertyChangedWithValue(value, "ShowButtons");
                }
            }
        }

        public SettlementReligionItemVM(string itemName, string CultureId, string ReligionId, int Level=1, string currentSettlement="", Dictionary<string,string> properties=null)
        {
            ItemName = itemName;
            //ShowButtons = true;
            ItemSprite = CultureId + cultureSpritePath;
            LevelSprite = lvlSpritePath + Level.ToString();
            ItemProperties = new MBBindingList<ItemValuePairVM>();
            if (properties == null)
                properties = new Dictionary<string, string>();
            foreach (var property in properties) {
                ItemProperties.Add(new ItemValuePairVM(property.Key, property.Value));
            }
            ShowIcon = true;
            _settlement = Settlement.Find(currentSettlement);
            _religionId = ReligionId;
        }

        /*public SettlementReligionItemVM(SettlementReligionSectionVM parent, string itemTitle, Dictionary<string, string> properties = null)
        {
            ItemName = itemTitle;
            //ShowButtons = true;

            ItemSprite = CultureId + cultureSpritePath;

            LevelSprite = lvlSpritePath + Level.ToString();
            ItemProperties = new MBBindingList<ItemValuePairVM>();
            if (properties == null)
                properties = new Dictionary<string, string>();
            foreach (var property in properties)
            {
                ItemProperties.Add(new ItemValuePairVM(property.Key, property.Value));
            }
            ShowIcon = true;
            _settlement = Settlement.Find(currentSettlement);
            _religionId = ReligionId;
        }*/


        public SettlementReligionItemVM(Dictionary<string,string> properties, string ReligionId)
        {
            ItemDetailsWidth = 225;
            ItemTotalWidth = 225;
            ItemName = "Religion Settlement Details";
            ShowButtons = false;
            ItemProperties = new MBBindingList<ItemValuePairVM>();
            ShowIcon = false;
            foreach (var property in properties)
            {
                ItemProperties.Add(new ItemValuePairVM(property.Key, property.Value));
            }
            ShowIcon = false;
            _religionId = ReligionId;
        }

        public void Convert()
        {
            ReligionConversationBehavior.SetRequestedTopic("conversion_oath");
            ReligionConversationBehavior.StartPriestConversation(Settlement.CurrentSettlement, this._religionId);
        }

        public void TalkPriest()
        {
            ReligionConversationBehavior.SetRequestedTopic("talk");
            ReligionConversationBehavior.StartPriestConversation(Settlement.CurrentSettlement, this._religionId);
        }
        public void Pray()
        {
            ReligionConversationBehavior.SetRequestedTopic("topic_blessing");
            ReligionConversationBehavior.StartPriestConversation(Settlement.CurrentSettlement, this._religionId);
        }

    }
}