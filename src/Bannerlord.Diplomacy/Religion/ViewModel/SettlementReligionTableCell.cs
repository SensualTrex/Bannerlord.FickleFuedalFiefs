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
    public class SettlementReligionTableCell : TaleWorlds.Library.ViewModel
    {
        private string _cellValue;

        public SettlementReligionTableCell()
        {
            //CellValue = cellValue;
        }

        /*[DataSourceProperty]
        public string CellValue
        {
            get
            {
                return _cellValue;
            }
            set
            {
                if (value != _cellValue)
                {
                    _cellValue = value;
                    base.OnPropertyChangedWithValue(value, "CellValue");
                }
            }
        }*/

    }
}