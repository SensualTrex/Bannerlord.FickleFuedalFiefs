using JetBrains.Annotations;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using TaleWorlds.SaveSystem;

namespace Religions.Religion
{
    public enum ReligionType
    {
        Polytheism,
        Monotheism,
        Animism,
        AncestorWorship,
        Shamanism,
        Totemism
    }
    public class ReligionLore
    {
        [UsedImplicitly]
        [SaveableProperty(1)]
        public string ReligionId { get; private set; }

        [UsedImplicitly]
        [SaveableProperty(2)]
        public string Name { get; private set; }

        [UsedImplicitly]
        [SaveableProperty(3)]
        public string Description { get; private set; }

        [UsedImplicitly]
        [SaveableProperty(4)]
        public Dictionary<string, string> LoreLibrary { get; private set; }

        [UsedImplicitly]
        [SaveableProperty(5)]
        public Dictionary<string, string> Dieties { get; private set; }

    }
}
