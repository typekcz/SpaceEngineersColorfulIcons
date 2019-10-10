using System.ComponentModel;
using System.Xml.Serialization;
using ProtoBuf;

namespace Sisk.ColorfulIcons.Settings {
    [ProtoContract]
    public class ModSettings {
        private const bool BLOCKS = true;
        private const bool COMPONENTS = true;
        private const bool OLD_COMPONENTS = false;
        private const bool INGOTS = true;
        private const bool ORES = true;
        private const bool TOOLS = true;
        private const bool FIX_TOOL_COLORS = false;
        private const bool FORCE_OVERRIDE = false;

        [ProtoMember(1)]
        [XmlElement(Order = 1)]
        [DefaultValue(ORES)]
        public bool Ores { get; set; } = ORES;

        [ProtoMember(2)]
        [XmlElement(Order = 2)]
        [DefaultValue(INGOTS)]
        public bool Ingots { get; set; } = INGOTS;

        [ProtoMember(3)]
        [XmlElement(Order = 3)]
        [DefaultValue(COMPONENTS)]
        public bool Components { get; set; } = COMPONENTS;

        [ProtoMember(4)]
        [XmlElement(Order = 4)]
        [DefaultValue(OLD_COMPONENTS)]
        public bool OldComponents { get; set; } = OLD_COMPONENTS;

        [ProtoMember(5)]
        [XmlElement(Order = 5)]
        [DefaultValue(BLOCKS)]
        public bool Blocks { get; set; } = BLOCKS;

        [ProtoMember(6)]
        [XmlElement(Order = 6)]
        [DefaultValue(TOOLS)]
        public bool Tools { get; set; } = TOOLS;

        [ProtoMember(7)]
        [XmlElement(Order = 7)]
        [DefaultValue(FIX_TOOL_COLORS)]
        public bool FixToolColors { get; set; } = FIX_TOOL_COLORS;
        [ProtoMember(8)]
        [XmlElement(Order = 8)]
        [DefaultValue(FORCE_OVERRIDE)]
        public bool ForceOverride { get; set; } = FORCE_OVERRIDE;
    }
}