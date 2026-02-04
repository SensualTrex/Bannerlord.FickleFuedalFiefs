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

namespace Religions.Religion.ViewModel
{
    public class SettlementReligionPopVM : TaleWorlds.Library.ViewModel
    {
        private int _popWidth;

        public SettlementReligionPopVM(double percent=1.0)
        {
            if(percent >= 1)
            {
                PopWidth = 28;
            }
            else
            {
                PopWidth = (int)Math.Round(percent * 28); 
            }
        }

        [DataSourceProperty]
        public int PopWidth
        {
            get
            {
                return _popWidth;
            }
            set
            {
                if (value != _popWidth)
                {
                    _popWidth = value;
                    base.OnPropertyChangedWithValue(value, "PopWidth");
                }
            }
        }

    }
}