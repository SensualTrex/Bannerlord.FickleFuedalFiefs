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
    public class ItemValuePairVM : TaleWorlds.Library.ViewModel
    {
        private string _property;
        private string _value;

        public ItemValuePairVM(string property, string value)
        {
            Property = property;
            Value = value;
        }

        [DataSourceProperty]
        public string Property
        {
            get
            {
                return _property;
            }
            set
            {
                if (value != _property)
                {
                    _property = value;
                    base.OnPropertyChangedWithValue(value, "Property");
                }
            }
        }

        [DataSourceProperty]
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value != _value)
                {
                    _value = value;
                    base.OnPropertyChangedWithValue(value, "Value");
                }
            }
        }

    }
}