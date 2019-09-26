using System.ComponentModel;
using System.Xml.Serialization;
using ProtoBuf;

namespace Sisk.ColorfulIcons.Settings {
    [ProtoContract]
    public class ModSettings {
        private const bool BLOCKS = true;
        private const bool COMPONENTS = true;
        private const bool INGOTS = true;
        private const bool ORES = true;
        private const bool TOOLS = true;

        [ProtoMember(4)]
        [XmlElement(Order = 4)]
        [DefaultValue(BLOCKS)]
        public bool Blocks { get; set; } = BLOCKS;

        [ProtoMember(3)]
        [XmlElement(Order = 3)]
        [DefaultValue(COMPONENTS)]
        public bool Components { get; set; } = COMPONENTS;

        [ProtoMember(2)]
        [XmlElement(Order = 2)]
        [DefaultValue(INGOTS)]
        public bool Ingots { get; set; } = INGOTS;

        [ProtoMember(1)]
        [XmlElement(Order = 1)]
        [DefaultValue(ORES)]
        public bool Ores { get; set; } = ORES;

        [ProtoMember(5)]
        [XmlElement(Order = 5)]
        [DefaultValue(TOOLS)]
        public bool Tools { get; set; } = TOOLS;
    }
}