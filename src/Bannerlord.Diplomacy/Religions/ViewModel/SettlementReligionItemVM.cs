using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

using static TaleWorlds.Core.Equipment;

namespace Diplomacy.Religions.ViewModel
{
    public class SettlementReligionItemVM
    {

        private string _name;
        [DataSourceProperty]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {

            }
        }

        public SettlementReligionItemVM(Religion religion, ReligiousBuilding religiousBuilding)
        {
            
        }

    }
}