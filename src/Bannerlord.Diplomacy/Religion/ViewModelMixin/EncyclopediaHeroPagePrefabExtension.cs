using Bannerlord.UIExtenderEx.Attributes;
using Bannerlord.UIExtenderEx.Prefabs2;

using JetBrains.Annotations;

using System.Xml;

using TaleWorlds.GauntletUI.BaseTypes;

namespace Religions.Religion.ViewModelMixin
{ 
    //[PrefabExtension("EncyclopediaHeroPage", "descendant::RichTextWidget[@Text='@InformationText']")]
    [PrefabExtension("EncyclopediaHeroPage", "descendant::GridWidget[@Id='StatsGrid']")]
    [UsedImplicitly]
    internal sealed class EncyclopediaHeroPagePrefabExtension : PrefabExtensionInsertPatch
    {
        public override InsertType Type => InsertType.Append;

        private readonly XmlDocument _document;

        public EncyclopediaHeroPagePrefabExtension()
        {
            _document = new XmlDocument();
            _document.LoadXml(@"<EncyclopediaHeroPageReligionInject />");

            var checker = true;
        }

        [PrefabExtensionXmlDocument]
        [UsedImplicitly]
        public XmlDocument GetPrefabExtension() => _document;
    }
}