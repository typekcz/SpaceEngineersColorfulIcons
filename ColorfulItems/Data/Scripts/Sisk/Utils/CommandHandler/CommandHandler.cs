using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandbox.ModAPI;

namespace Sisk.Utils.CommandHandler {
    public class CommandHandler {
        private readonly Dictionary<string, Command> _commands = new Dictionary<string, Command>();
        private readonly string _modName;

        /// <summary>
        ///     Initialize a new instance of <see cref="CommandHandler" />.
        /// </summary>
        /// <param name="modName"></param>
        public CommandHandler(string modName) {
            _modName = modName;
        }

        /// <summary>
        ///     A prefix needed to trigger registered commands.
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        ///     Register an <see cref="Command" />.
        /// </summary>
        /// <param name="command">The <see cref="Command" /> to register.</param>
        public void Register(Command command) {
            if (command == null) {
                throw new ArgumentNullException(nameof(command));
            }

            if (string.IsNullOrWhiteSpace(command.Name)) {
                throw new Exception($"Invalid command name: '{command.Name}'");
            }

            if (_commands.ContainsKey(command.Name)) {
                throw new Exception($"Command already registered: {command.Name}");
            }

            _commands.Add(command.Name.ToLower(), command);
        }

        /// <summary>
        ///     Show an help window with all listed commands.
        /// </summary>
        /// <param name="arguments"></param>
        public void ShowHelp(string arguments) {
            var sb = new StringBuilder().AppendLine("Usage: /ss [command]");
            var maxArgLen = _commands.Keys.Max(x => x.Length);
            var outputFormat = $"  {{0, -{maxArgLen + 2}}}{{1}}";
            foreach (var command in _commands.Values) {
                sb.AppendFormat(outputFormat, command.Name, command.Description);
                sb.AppendLine();
            }

            MyAPIGateway.Utilities.ShowMessage(_modName, sb.ToString());
            //MyAPIGateway.Utilities.ShowMissionScreen(_modName, "", "Help", sb.ToString(), okButtonCaption: "Exit");
        }

        /// <summary>
        ///     Try to find and execute a command for given string.
        /// </summary>
        /// <param name="argument">The command string.</param>
        /// <returns>Returns true if a command is found.</returns>
        public bool TryHandle(string argument) {
            string commandName;
            string parameter;

            if (!string.IsNullOrWhiteSpace(Prefix)) {
                if (argument.Length <= Prefix.Length) {
                    return false;
                }

                var prefix = argument.Substring(0, Prefix.Length).ToLower();
                if (prefix != Prefix.ToLower()) {
                    return false;
                }

                argument = argument.Remove(0, Prefix.Length).Trim();
            }

            var commandEnd = argument.IndexOf(" ", StringComparison.InvariantCultureIgnoreCase);

            if (commandEnd == -1) {
                commandName = argument.Trim();
                parameter = "";
            } else {
                commandName = argument.Substring(0, commandEnd);
                parameter = argument.Substring(commandEnd).Trim();
            }

            Command command;
            if (_commands.TryGetValue(commandName.ToLower(), out command)) {
                command.Execute(parameter);
                return true;
            }

            return false;
        }
    }
}
