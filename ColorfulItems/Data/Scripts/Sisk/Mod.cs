using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandbox.Definitions;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;

namespace Sisk.ColorfulIcons {
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
	public class Mod : MySessionComponentBase {
		public const string NAME = "Colorful Icons";
		private const string FILE_NAME = "definitions.txt";
		private readonly CommandHandler _commandHandler = new CommandHandler();

		private readonly Dictionary<MyDefinitionBase, string> _replacedIcons = new Dictionary<MyDefinitionBase, string>();

		/// <summary>
		///     Mod name to acronym.
		/// </summary>
		public static string Acronym => string.Concat(NAME.Where(char.IsUpper));

		/// <inheritdoc />
		public override void LoadData() {
			if (!MyAPIGateway.Multiplayer.MultiplayerActive || !MyAPIGateway.Utilities.IsDedicated) {
				MyLog.Default.WriteLineAndConsole("ColorfulIcons: Load Data - START");
				try {
					ModifyDefinitions();
					CreateCommands();
				} catch(Exception exception){
					MyLog.Default.WriteLineAndConsole("ColorfulIcons: " + exception.ToString());
				}
				MyAPIGateway.Utilities.MessageEntered += OnMessageEntered;

				MyLog.Default.WriteLineAndConsole("ColorfulIcons: Load Data - END");
			}
		}

		/// <inheritdoc />
		protected override void UnloadData() {
			if (!MyAPIGateway.Multiplayer.MultiplayerActive || !MyAPIGateway.Utilities.IsDedicated) {
				MyLog.Default.WriteLineAndConsole("ColorfulIcons: Unload Data - START");
				MyAPIGateway.Utilities.MessageEntered -= OnMessageEntered;
				RevertIcons();
				_replacedIcons.Clear();
				MyLog.Default.WriteLineAndConsole("ColorfulIcons: Unload Data - END");
			}
		}

		/// <summary>
		///     Change icon for specified definition.
		/// </summary>
		/// <param name="definition">The definition where the icon should be changed.</param>
		private void ChangeIcon(MyDefinitionBase definition, string iconPath) {
			if (definition?.Icons != null && definition.Icons.Any()) {
				if (!definition.Icons[0].StartsWith(ModContext.ModPath)) {
					if (!_replacedIcons.ContainsKey(definition)) {
						_replacedIcons.Add(definition, definition.Icons[0]);
					}

					definition.Icons[0] = $"{ModContext.ModPath}\\{iconPath}";
					MyLog.Default.WriteLineAndConsole($"|-> {definition.Id} > {definition.Icons[0]}");
				}
			}
		}

		/// <summary>
		///     Create commands.
		/// </summary>
		private void CreateCommands() {
			_commandHandler.Prefix = $"/{Acronym}";
			_commandHandler.Register(new Command { Name = "Generate", Description = "Generate a list of definition id's when loaded in a vanilla world", Execute = OnGenerateCommand });
		}

		/// <summary>
		///     Creates a file with vanilla definition ids.
		/// </summary>
		private void CreateDefinitionFile() {
			var definitions = MyDefinitionManager.Static.GetAllDefinitions();
			var blueprintDefinitions = MyDefinitionManager.Static.GetBlueprintDefinitions();
			var allDefinitions = definitions.Concat(blueprintDefinitions);

			var definitionsIds = (from definition in allDefinitions where definition.Context.IsBaseGame where definition is MyCubeBlockDefinition || definition is MyPhysicalItemDefinition || definition is MyBlockBlueprintDefinition select definition.Id.ToString()).ToList();
			using (var writer = MyAPIGateway.Utilities.WriteBinaryFileInWorldStorage(FILE_NAME, typeof(Mod))) {
				var sb = new StringBuilder();
				foreach (var definitionsId in definitionsIds) {
					sb.AppendLine($"\"{definitionsId}\",");
				}

				var bytes = Encoding.UTF8.GetBytes(sb.ToString());
				writer.Write(bytes);
			}
		}

		/// <summary>
		///     Change block, blueprint and item icons.
		/// </summary>
		private void ModifyDefinitions() {
			MyLog.Default.WriteLineAndConsole("ColorfulIcons: Change Icons - Start");
			var definitions = MyDefinitionManager.Static.GetAllDefinitions();
			var blueprintDefinitions = MyDefinitionManager.Static.GetBlueprintDefinitions();

			foreach (var blueprint in blueprintDefinitions) {
				if (blueprint?.Id != null) {
					string iconPath = null;
					if(Config.icons.TryGetValue(blueprint.Id.ToString(), out iconPath)){
						ChangeIcon(blueprint, iconPath);
					}
				}
			}

			foreach (var definition in definitions) {
				if (definition?.Id != null) {
					if (definition is MyCubeBlockDefinition || definition is MyPhysicalItemDefinition) {
						string iconPath = null;
						if(Config.icons.TryGetValue(definition.Id.ToString(), out iconPath)){
							ChangeIcon(definition, iconPath);
						}
					}
				}
			}

			MyLog.Default.WriteLineAndConsole("ColorfulIcons: Change Icons - END");
		}

		/// <summary>
		///     Executed when "Generate" command in chat received.
		/// </summary>
		/// <param name="arguments"></param>
		private void OnGenerateCommand(string arguments) {
			if (MyAPIGateway.Session.Mods.Any(x => x.Name != ModContext.ModId)) {
				MyAPIGateway.Utilities.ShowMessage(NAME, $"You should execute this command in a world without other mods than \"{NAME}\".");
			} else {
				CreateDefinitionFile();
				MyAPIGateway.Utilities.ShowMessage(NAME, $"Definition file generated. You can find it in \"{MyAPIGateway.Session.CurrentPath}\\Storage\\{ModContext.ModId}\"");
			}
		}

		private void OnMessageEntered(string messagetext, ref bool sendtoothers) {
			if (_commandHandler.TryHandle(messagetext.Trim())) {
				sendtoothers = false;
			}
		}

		/// <summary>
		///     Revert icons to default.
		/// </summary>
		private void RevertIcons() {
			MyLog.Default.WriteLineAndConsole("ColorfulIcons: Revert Icons - END");
			foreach (var definition in _replacedIcons.Keys) {
				definition.Icons[0] = _replacedIcons[definition];
				MyLog.Default.WriteLineAndConsole($"|-> {definition.Id} > {definition.Icons[0]}");
			}

			MyLog.Default.WriteLineAndConsole("ColorfulIcons: Revert Icons - END");
		}
	}
}
