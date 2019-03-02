using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandbox.Definitions;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;

namespace Sisk.ColorfulIconsOld {
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
    public class Mod : MySessionComponentBase {
        public const string NAME = "Colorful Icons Old";
        private const string FILE_NAME = "definitions.txt";

        private readonly Dictionary<MyDefinitionBase, string> _replacedIcons = new Dictionary<MyDefinitionBase, string>();

        /// <summary>
        ///     Mod name to acronym.
        /// </summary>
        public static string Acronym => string.Concat(NAME.Where(char.IsUpper));

        /// <inheritdoc />
        public override void LoadData() {
            if (!MyAPIGateway.Multiplayer.MultiplayerActive || !MyAPIGateway.Utilities.IsDedicated) {
                MyLog.Default.WriteLineAndConsole("ColorfulIconsOld: Load Data - START");
                try {
                    ModifyDefinitions();
                } catch (Exception exception) {
                    MyLog.Default.WriteLineAndConsole("ColorfulIconsOld: " + exception);
                }
                MyLog.Default.WriteLineAndConsole("ColorfulIconsOld: Load Data - END");
            }
        }

        /// <inheritdoc />
        protected override void UnloadData() {
            if (!MyAPIGateway.Multiplayer.MultiplayerActive || !MyAPIGateway.Utilities.IsDedicated) {
                MyLog.Default.WriteLineAndConsole("ColorfulIconsOld: Unload Data - START");
                RevertIcons();
                _replacedIcons.Clear();
                MyLog.Default.WriteLineAndConsole("ColorfulIconsOld: Unload Data - END");
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
        ///     Creates a file with vanilla definition ids.
        /// </summary>
        private void CreateDefinitionFile() {
            var definitions = MyDefinitionManager.Static.GetAllDefinitions();
            var blueprintDefinitions = MyDefinitionManager.Static.GetBlueprintDefinitions();
            var allDefinitions = definitions.Concat(blueprintDefinitions);

            var definitionIds = new Dictionary<string, string>();
            foreach (var definition in allDefinitions) {
                if (definition.Context.IsBaseGame) {
                    if (definition.Icons != null && definition.Icons.Any() && (definition is MyCubeBlockDefinition || definition is MyPhysicalItemDefinition || definition is MyBlockBlueprintDefinition)) {
                        definitionIds.Add(definition.Id.ToString(), definition.Icons[0]);
                    }
                }
            }

            using (var writer = MyAPIGateway.Utilities.WriteBinaryFileInWorldStorage(FILE_NAME, typeof(Mod))) {
                var sb = new StringBuilder();
                foreach (var pair in definitionIds) {
                    sb.AppendLine($"{{\"{pair.Key}\",\"{pair.Value.Replace(@"\", "/")}\"}},");
                }

                var bytes = Encoding.UTF8.GetBytes(sb.ToString());
                writer.Write(bytes);
            }
        }

        /// <summary>
        ///     Change block, blueprint and item icons.
        /// </summary>
        private void ModifyDefinitions() {
            MyLog.Default.WriteLineAndConsole("ColorfulIconsOld: Change Icons - Start");
            var definitions = MyDefinitionManager.Static.GetAllDefinitions();
            var blueprintDefinitions = MyDefinitionManager.Static.GetBlueprintDefinitions();

            foreach (var blueprint in blueprintDefinitions) {
                string iconPath;
                if (blueprint?.Id != null && Config.Icons.TryGetValue(blueprint.Id.ToString(), out iconPath)) {
                    ChangeIcon(blueprint, iconPath);
                }
            }

            foreach (var definition in definitions) {
                string iconPath;
                if (definition?.Id != null && Config.Icons.TryGetValue(definition.Id.ToString(), out iconPath)) {
                    if (definition is MyCubeBlockDefinition || definition is MyPhysicalItemDefinition) {
                        ChangeIcon(definition, iconPath);
                    }
                }
            }

            MyLog.Default.WriteLineAndConsole("ColorfulIconsOld: Change Icons - END");
        }

        /// <summary>
        ///     Revert icons to default.
        /// </summary>
        private void RevertIcons() {
            MyLog.Default.WriteLineAndConsole("ColorfulIconsOld: Revert Icons - END");
            foreach (var definition in _replacedIcons.Keys) {
                definition.Icons[0] = _replacedIcons[definition];
                MyLog.Default.WriteLineAndConsole($"|-> {definition.Id} > {definition.Icons[0]}");
            }

            MyLog.Default.WriteLineAndConsole("ColorfulIcons: Revert Icons - END");
        }
    }
}
