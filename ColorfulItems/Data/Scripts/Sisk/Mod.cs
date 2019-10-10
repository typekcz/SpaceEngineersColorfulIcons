using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sandbox.Definitions;
using Sandbox.ModAPI;
using Sisk.ColorfulIcons.Data;
using Sisk.ColorfulIcons.Extensions;
using Sisk.ColorfulIcons.Localization;
using Sisk.ColorfulIcons.Settings;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;

namespace Sisk.ColorfulIcons {
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
    public class Mod : MySessionComponentBase {
        public const string NAME = "Colorful Icons";
        private const string SETTINGS_FILE = "ColorfulIcons.xml";

        private readonly Dictionary<MyDefinitionBase, string> _replacedIcons = new Dictionary<MyDefinitionBase, string>();
        private readonly Dictionary<MyPhysicalItemDefinition, string> _replacedModels = new Dictionary<MyPhysicalItemDefinition, string>();
        private DictionaryValuesReader<MyDefinitionId, MyBlueprintDefinitionBase>? _blueprintDefinitions;
        private ChatHandler _chatHandler;

        private DictionaryValuesReader<MyDefinitionId, MyDefinitionBase>? _definitions;
        private GuiHandler _guiHandler;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Mod" /> session component.
        /// </summary>
        public Mod() {
            Static = this;
        }

        /// <summary>
        ///     Mod name to acronym.
        /// </summary>
        public static string Acronym => string.Concat(NAME.Where(char.IsUpper));

        private DictionaryValuesReader<MyDefinitionId, MyBlueprintDefinitionBase> BlueprintDefinitions => _blueprintDefinitions ?? (_blueprintDefinitions = MyDefinitionManager.Static.GetBlueprintDefinitions()).Value;
        private DictionaryValuesReader<MyDefinitionId, MyDefinitionBase> Definitions => _definitions ?? (_definitions = MyDefinitionManager.Static.GetAllDefinitions()).Value;

        /// <summary>
        ///     Language used to localize this mod.
        /// </summary>
        public MyLanguagesEnum? Language { get; private set; }

        /// <summary>
        ///     The Mod Settings.
        /// </summary>
        public ModSettings Settings { get; private set; }

        /// <summary>
        ///     The static instance.
        /// </summary>
        public static Mod Static { get; private set; }

        /// <summary>
        ///     Shows a result message in chat window.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="option"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        public static void ShowResultMessage<TValue>(Option option, TValue value, Result result) {
            if (MyAPIGateway.Multiplayer.MultiplayerActive && MyAPIGateway.Utilities.IsDedicated) {
                return;
            }

            switch (result) {
                case Result.Error:
                    MyAPIGateway.Utilities.ShowMessage(NAME, ModText.Error_CI_SetOption.GetString(option, value));
                    break;
                case Result.Success:
                    MyAPIGateway.Utilities.ShowMessage(NAME, ModText.Message_CI_SetOptionSuccess.GetString(option, value));
                    break;
            }
        }

        /// <inheritdoc />
        public override void LoadData() {
            if (!MyAPIGateway.Multiplayer.MultiplayerActive || !MyAPIGateway.Utilities.IsDedicated) {
                LoadLocalization();
                LoadSettings();
                ModifyDefinitions();

                MyAPIGateway.Gui.GuiControlRemoved += OnGuiControlRemoved;
                _chatHandler = new ChatHandler();
                _guiHandler = new GuiHandler();
            }
        }

        /// <inheritdoc />
        protected override void UnloadData() {
            if (!MyAPIGateway.Multiplayer.MultiplayerActive || !MyAPIGateway.Utilities.IsDedicated) {
                MyAPIGateway.Gui.GuiControlRemoved -= OnGuiControlRemoved;

                if (_guiHandler != null) {
                    _guiHandler.Close();
                    _guiHandler = null;
                }

                if (_chatHandler != null) {
                    _chatHandler.Close();
                    _chatHandler = null;
                }

                RevertDefinitions();
            }

            Static = null;
        }

        /// <summary>
        ///     Will check if given option is enabled in settings.
        /// </summary>
        /// <param name="option">The option which will be used to check.</param>
        /// <returns>Returns true when given option is enabled.</returns>
        public bool IsOptionEnabled(Option option) {
            switch (option) {
                case Option.Blocks:
                    return Settings.Blocks;
                case Option.Components:
                    return Settings.Components;
                case Option.OldComponents:
                    return Settings.OldComponents;
                case Option.Ingots:
                    return Settings.Ingots;
                case Option.Ores:
                    return Settings.Ores;
                case Option.Tools:
                    return Settings.Tools;
                case Option.FixToolColors:
                    return Settings.FixToolColors;
                case Option.ForceOverride:
                    return Settings.ForceOverride;
                default:
                    return false;
            }
        }

        /// <summary>
        ///     Set a given option to given value.
        /// </summary>
        /// <typeparam name="TValue">The value type.</typeparam>
        /// <param name="option">Which option should be set.</param>
        /// <param name="value">The value for given option.</param>
        /// <param name="showResultInChat">Show result in chat box when this is true.</param>
        public void SetOption<TValue>(Option option, TValue value, bool showResultInChat = true) {
            switch (option) {
                case Option.Blocks:
                    Settings.Blocks = (bool) (object) value;
                    break;
                case Option.Components:
                    Settings.Components = (bool) (object) value;
                    break;
                case Option.OldComponents:
                    Settings.OldComponents = (bool) (object) value;
                    break;
                case Option.Ingots:
                    Settings.Ingots = (bool) (object) value;
                    break;
                case Option.Ores:
                    Settings.Ores = (bool) (object) value;
                    break;
                case Option.Tools:
                    Settings.Tools = (bool) (object) value;
                    break;
                case Option.FixToolColors:
                    Settings.FixToolColors = (bool) (object) value;
                    break;
                case Option.ForceOverride:
                    Settings.ForceOverride = (bool) (object) value;
                    break;
                default:
                    MyLog.Default.WriteLine($"Unknown option '{nameof(option)}'");

                    return;
            }

            if (showResultInChat) {
                ShowResultMessage(option, value, Result.Success);
            }

            if (_guiHandler != null && _guiHandler.IsRegistered) {
                _guiHandler.UpdateMenuItemText(option, (bool) (object) value);
            }

            SaveSettings();
            RevertDefinitions();
            ModifyDefinitions();
        }

        /// <summary>
        ///     Change icon for specified definition.
        /// </summary>
        /// <param name="definition">The definition where the icon should be changed.</param>
        /// <param name="iconPath">The path to the icon relative to mod path.</param>
        private void ChangeIcon(MyDefinitionBase definition, string iconPath) {
            if (definition?.Icons != null && definition.Icons.Any()) {
                if(!IsOptionEnabled(Option.ForceOverride) && Path.IsPathRooted(definition.Icons[0]))
                    return;
                if (!definition.Icons[0].StartsWith(ModContext.ModPath) && !_replacedIcons.ContainsKey(definition)) {
                    _replacedIcons.Add(definition, definition.Icons[0]);
                }

                definition.Icons[0] = $"{ModContext.ModPath}\\{iconPath}";
                MyLog.Default.WriteLineAndConsole($"{definition.Id} -> {ModContext.ModPath}\\{iconPath}");
            }
        }

        /// <summary>
        ///     Change model for specified definition.
        /// </summary>
        /// <param name="definition">The definition where the model should be changed.</param>
        /// <param name="modelPath">The path to the model relative to game path.</param>
        private void ChangeModel(MyPhysicalItemDefinition definition, string modelPath) {
            if (definition?.Model != null) {
                if (!_replacedModels.ContainsKey(definition)) {
                    _replacedModels.Add(definition, definition.Model);
                }

                definition.Model = modelPath;
                MyLog.Default.WriteLineAndConsole($"{definition.Id} -> {modelPath}");
            }
        }

        /// <summary>
        ///     Load localizations for this mod.
        /// </summary>
        private void LoadLocalization() {
            var path = Path.Combine(ModContext.ModPathData, "Localization");
            var supportedLanguages = new HashSet<MyLanguagesEnum>();
            MyTexts.LoadSupportedLanguages(path, supportedLanguages);

            var currentLanguage = supportedLanguages.Contains(MyAPIGateway.Session.Config.Language) ? MyAPIGateway.Session.Config.Language : MyLanguagesEnum.English;
            if (Language != null && Language == currentLanguage) {
                return;
            }

            Language = currentLanguage;
            var languageDescription = MyTexts.Languages.Where(x => x.Key == currentLanguage).Select(x => x.Value).FirstOrDefault();
            if (languageDescription != null) {
                var cultureName = string.IsNullOrWhiteSpace(languageDescription.CultureName) ? null : languageDescription.CultureName;
                var subcultureName = string.IsNullOrWhiteSpace(languageDescription.SubcultureName) ? null : languageDescription.SubcultureName;

                MyTexts.LoadTexts(path, cultureName, subcultureName);
            }
        }

        /// <summary>
        ///     Load mod settings.
        /// </summary>
        private void LoadSettings() {
            ModSettings settings = null;
            try {
                if (MyAPIGateway.Utilities.FileExistsInGlobalStorage(SETTINGS_FILE)) {
                    using (var reader = MyAPIGateway.Utilities.ReadFileInGlobalStorage(SETTINGS_FILE)) {
                        settings = MyAPIGateway.Utilities.SerializeFromXML<ModSettings>(reader.ReadToEnd());
                    }
                }
            } catch (Exception exception) {
                MyLog.Default.WriteLine(exception);
            }

            if (settings == null) {
                settings = new ModSettings();
            }

            Settings = settings;
        }

        /// <summary>
        ///     Change block, blueprint and item icons.
        /// </summary>
        private void ModifyDefinitions() {
            MyLog.Default.WriteLine("ColorfulIcons.ModifyDefinitions - START");
            var definitions = Definitions;
            var blueprintDefinitions = BlueprintDefinitions;

            foreach (var pair in Config.Icons.Where(x => IsOptionEnabled(x.Key))) {
                var option = pair.Key;
                var dictionary = pair.Value;

                ModifyDefinitions(dictionary, ref definitions, ref blueprintDefinitions);

                if (option == Option.Tools && IsOptionEnabled(Option.FixToolColors)) {
                    foreach (var definitionPair in Config.FixTools) {
                        var definitionId = MyDefinitionId.Parse(definitionPair.Key);
                        var blueprintId = MyDefinitionId.Parse(definitionPair.Value.BlueprintId);
                        var modelPath = definitionPair.Value.Model;
                        var iconPath = definitionPair.Value.Icon;

                        MyBlueprintDefinitionBase blueprint;
                        if (blueprintDefinitions.TryGetValue(blueprintId, out blueprint)) {
                            ChangeIcon(blueprint, iconPath);
                        }

                        MyDefinitionBase definition;
                        if (definitions.TryGetValue(definitionId, out definition)) {
                            ChangeModel(definition as MyPhysicalItemDefinition, modelPath);
                            ChangeIcon(definition, iconPath);
                        }
                    }
                }

                if (option == Option.Components && IsOptionEnabled(Option.OldComponents)) {
                    ModifyDefinitions(Config.OldComponents, ref definitions, ref blueprintDefinitions);
                }
            }

            MyLog.Default.WriteLine("ColorfulIcons.ModifyDefinitions - END");
        }

        /// <summary>
        ///     Modifies all matching definitions.
        /// </summary>
        /// <param name="dictionary">A dictionary with definition ids and icon paths.</param>
        /// <param name="definitions">All block, component and tool definitions.</param>
        /// <param name="blueprintDefinitions">All blueprint definitions.</param>
        private void ModifyDefinitions(Dictionary<string, string> dictionary, ref DictionaryValuesReader<MyDefinitionId, MyDefinitionBase> definitions, ref DictionaryValuesReader<MyDefinitionId, MyBlueprintDefinitionBase> blueprintDefinitions) {
            foreach (var definitionPair in dictionary) {
                var definitionId = MyDefinitionId.Parse(definitionPair.Key);
                var iconPath = definitionPair.Value;

                if (definitionPair.Key.StartsWith("MyObjectBuilder_BlueprintDefinition/")) {
                    MyBlueprintDefinitionBase blueprint;
                    if (blueprintDefinitions.TryGetValue(definitionId, out blueprint)) {
                        ChangeIcon(blueprint, iconPath);
                    }
                } else {
                    MyDefinitionBase definition;
                    if (definitions.TryGetValue(definitionId, out definition)) {
                        ChangeIcon(definition, iconPath);
                    }
                }
            }
        }

        /// <summary>
        ///     Event triggered on gui control removed.
        ///     Used to detect if Option screen is closed and then to reload localization.
        /// </summary>
        /// <param name="obj"></param>
        private void OnGuiControlRemoved(object obj) {
            if (obj.ToString().EndsWith("ScreenOptionsSpace")) {
                LoadLocalization();
            }
        }

        /// <summary>
        ///     Revert definitions to default.
        /// </summary>
        private void RevertDefinitions() {
            MyLog.Default.WriteLine("ColorfulIcons.RevertDefinitions - START");
            foreach (var pair in _replacedIcons) {
                var definition = pair.Key;
                var iconPath = pair.Value;

                definition.Icons[0] = iconPath;
                MyLog.Default.WriteLineAndConsole($"{definition.Id} -> {iconPath}");
            }

            foreach (var pair in _replacedModels) {
                var definition = pair.Key;
                var modelPath = pair.Value;

                definition.Model = modelPath;
                MyLog.Default.WriteLineAndConsole($"{definition.Id} -> {modelPath}");
            }

            MyLog.Default.WriteLine("ColorfulIcons.RevertDefinitions - END");
        }

        /// <summary>
        ///     Save settings.
        /// </summary>
        private void SaveSettings() {
            try {
                using (var writer = MyAPIGateway.Utilities.WriteFileInGlobalStorage(SETTINGS_FILE)) {
                    writer.Write(MyAPIGateway.Utilities.SerializeToXML(Settings));
                }
            } catch (Exception exception) {
                MyLog.Default.WriteLine(exception);
            }
        }
    }
}