namespace Sisk.ColorfulIcons {
    public class Command {
        public delegate void CommandEventHandler(string arguments);

        /// <summary>
        ///     Description displayed in help window.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     The action that gets executed if this command is triggered.
        /// </summary>
        public CommandEventHandler Execute { get; set; }

        /// <summary>
        ///     The name of this command.
        /// </summary>
        public string Name { get; set; }
    }
}
