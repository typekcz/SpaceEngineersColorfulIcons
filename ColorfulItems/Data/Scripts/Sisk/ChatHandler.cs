using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.ModAPI;
using Sisk.ColorfulIcons.Data;
using Sisk.ColorfulIcons.Extensions;
using Sisk.ColorfulIcons.Localization;
using Sisk.Utils.CommandHandler;

namespace Sisk.ColorfulIcons {
    public class ChatHandler {
        private readonly CommandHandler _commandHandler;

        private readonly Dictionary<Option, Type> _options = new Dictionary<Option, Type> {
            { Option.Blocks, typeof(bool) },
            { Option.Components, typeof(bool) },
            { Option.Ingots, typeof(bool) },
            { Option.Ores, typeof(bool) },
            { Option.Tools, typeof(bool) }
        };

        public ChatHandler() {
            _commandHandler = new CommandHandler(Mod.NAME) { Prefix = $"/{Mod.Acronym}" };
            _commandHandler.Register(new Command { Name = "Enable", Description = ModText.Description_CI_Enable.GetString(), Execute = OnEnableOptionCommand });
            _commandHandler.Register(new Command { Name = "Disable", Description = ModText.Description_CI_Disable.GetString(), Execute = OnDisableOptionCommand });
            _commandHandler.Register(new Command { Name = "Help", Description = ModText.Description_CI_Help.GetString(), Execute = _commandHandler.ShowHelp });
            _commandHandler.Register(new Command { Name = "List", Description = ModText.Description_CI_List.GetString(), Execute = OnListOptionsCommand });

            MyAPIGateway.Utilities.MessageEntered += OnMessageEntered;
        }

        /// <summary>
        ///     Close the network message handler.
        /// </summary>
        public void Close() {
            MyAPIGateway.Utilities.MessageEntered -= OnMessageEntered;
        }

        /// <summary>
        ///     Called on Disable option command.
        /// </summary>
        /// <param name="arguments">The arguments that should contain the option name.</param>
        private void OnDisableOptionCommand(string arguments) {
            Option result;
            if (TryGetOption(arguments, out result)) {
                if (_options[result] == typeof(bool)) {
                    Mod.Static.SetOption(result, false);
                } else {
                    MyAPIGateway.Utilities.ShowMessage(Mod.NAME, ModText.Error_CI_OnlyBooleanAllowed.GetString());
                }
            } else {
                MyAPIGateway.Utilities.ShowMessage(Mod.NAME, ModText.Error_CI_UnknownOption.GetString(arguments));
            }
        }

        /// <summary>
        ///     Called on Enable command received.
        /// </summary>
        /// <param name="arguments">The arguments that should contain the option name.</param>
        private void OnEnableOptionCommand(string arguments) {
            Option result;
            if (TryGetOption(arguments, out result)) {
                if (_options[result] == typeof(bool)) {
                    Mod.Static.SetOption(result, true);
                } else {
                    MyAPIGateway.Utilities.ShowMessage(Mod.NAME, ModText.Error_CI_OnlyBooleanAllowed.GetString());
                }
            } else {
                MyAPIGateway.Utilities.ShowMessage(Mod.NAME, ModText.Error_CI_UnknownOption.GetString(arguments));
            }
        }

        /// <summary>
        ///     Called on List command received.
        /// </summary>
        /// <param name="arguments">Arguments are ignored in this handler.</param>
        private void OnListOptionsCommand(string arguments) {
            var sb = new StringBuilder("Option | Alias | Type").AppendLine();
            foreach (var pair in _options) {
                var option = pair.Key;
                var type = pair.Value;
                sb.Append(option).Append(" | ");
                sb.Append($"<{type.Name}>").AppendLine();
            }

            MyAPIGateway.Utilities.ShowMessage(Mod.NAME, sb.ToString());
        }

        /// <summary>
        ///     Message event handler.
        /// </summary>
        /// <param name="messageText">The received message text.</param>
        /// <param name="sendToOthers">Indicates if message should be send to others.</param>
        private void OnMessageEntered(string messageText, ref bool sendToOthers) {
            if (_commandHandler.TryHandle(messageText.Trim())) {
                sendToOthers = false;
            }
        }

        /// <summary>
        ///     Try to resolve given string to an option.
        /// </summary>
        /// <param name="arguments">The string that gets checked.</param>
        /// <param name="result">The Option returned.</param>
        /// <returns>Returns true if an option is resolved.</returns>
        private bool TryGetOption(string arguments, out Option result) {
            if (Enum.TryParse(arguments, true, out result)) {
                return true;
            }

            return false;
        }
    }
}