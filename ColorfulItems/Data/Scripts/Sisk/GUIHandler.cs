using Draygo.API;
using Sisk.ColorfulIcons.Data;
using Sisk.ColorfulIcons.Extensions;
using Sisk.ColorfulIcons.Localization;
using VRage.Utils;

namespace Sisk.ColorfulIcons {
    public class GuiHandler {
        private static readonly MyStringId Off = MyStringId.GetOrCompute("HudInfoOff");
        private static readonly MyStringId On = MyStringId.GetOrCompute("HudInfoOn");
        private bool _hudApiRegistered = false;
        private HudAPIv2.MenuItem _blocks;
        private HudAPIv2.MenuItem _components;
        private HudAPIv2.MenuItem _oldComponents;
        private HudAPIv2 _hudApi;
        private HudAPIv2.MenuItem _ingots;
        private HudAPIv2.MenuRootCategory _menu;
        private HudAPIv2.MenuItem _ores;
        private HudAPIv2.MenuItem _tools;
        private HudAPIv2.MenuItem _fixToolColors;

        public GuiHandler() {
            _hudApi = new HudAPIv2(OnHudApiRegistered);
        }

        /// <summary>
        ///     Translate boolean to on or off localized.
        /// </summary>
        /// <param name="value">The boolean value.</param>
        /// <returns>Returns on localized when boolean is true else it will return off localized.</returns>
        private static string ToEnabledDisabledString(bool value) {
            return value ? On.GetString() : Off.GetString();
        }

        /// <summary>
        ///     Closes the hud api object.
        /// </summary>
        public void Close() {
            if (_hudApi != null) {
                _hudApi.Close();
                _hudApi = null;
                _hudApiRegistered = false;
            }
        }

        /// <summary>
        ///     Set a given option to given value.
        /// </summary>
        /// <param name="option">Which option should be set.</param>
        /// <param name="value">The value for given option.</param>
        public void UpdateMenuItemText(Option option, bool value) {
            if(!_hudApiRegistered)
                return;
            HudAPIv2.MenuItem menuItem;
            MyStringId text;
            switch (option) {
                case Option.Blocks:
                    menuItem = _blocks;
                    text = ModText.MenuItemText_CI_ReplaceBlocks;
                    break;
                case Option.Components:
                    menuItem = _components;
                    text = ModText.MenuItemText_CI_ReplaceComponents;
                    break;
                case Option.OldComponents:
                    menuItem = _oldComponents;
                    text = ModText.MenuItemText_CI_ReplaceComponentsOld;
                    break;
                case Option.Ingots:
                    menuItem = _ingots;
                    text = ModText.MenuItemText_CI_ReplaceIngots;
                    break;
                case Option.Ores:
                    menuItem = _ores;
                    text = ModText.MenuItemText_CI_ReplaceOres;
                    break;
                case Option.Tools:
                    menuItem = _tools;
                    text = ModText.MenuItemText_CI_ReplaceTools;
                    break;
                case Option.FixToolColors:
                    menuItem = _fixToolColors;
                    text = ModText.MenuItemText_CI_FixToolColors;
                    break;
                default:
                    MyLog.Default.WriteLine($"Unknown option '{nameof(option)}'");

                    return;
            }

            menuItem.Text = text.GetString(ToEnabledDisabledString(value));
        }

        /// <summary>
        ///     Initialize menu when hud api is registered.
        /// </summary>
        private void OnHudApiRegistered() {
            _menu = new HudAPIv2.MenuRootCategory(Mod.NAME, HudAPIv2.MenuRootCategory.MenuFlag.PlayerMenu, ModText.MenuRootCategoryHeader_CI.GetString());
            _blocks = new HudAPIv2.MenuItem(ModText.MenuItemText_CI_ReplaceBlocks.GetString(ToEnabledDisabledString(Mod.Static.Settings.Blocks)), _menu, OnReplaceBlocksChanged);
            _components = new HudAPIv2.MenuItem(ModText.MenuItemText_CI_ReplaceComponents.GetString(ToEnabledDisabledString(Mod.Static.Settings.Components)), _menu, OnReplaceComponentsChanged);
            _oldComponents = new HudAPIv2.MenuItem(ModText.MenuItemText_CI_ReplaceComponentsOld.GetString(ToEnabledDisabledString(Mod.Static.Settings.OldComponents)), _menu, OnReplaceOldComponentsChanged);
            _ingots = new HudAPIv2.MenuItem(ModText.MenuItemText_CI_ReplaceIngots.GetString(ToEnabledDisabledString(Mod.Static.Settings.Ingots)), _menu, OnReplaceIngotsChanged);
            _ores = new HudAPIv2.MenuItem(ModText.MenuItemText_CI_ReplaceOres.GetString(ToEnabledDisabledString(Mod.Static.Settings.Ores)), _menu, OnReplaceOresChanged);
            _tools = new HudAPIv2.MenuItem(ModText.MenuItemText_CI_ReplaceTools.GetString(ToEnabledDisabledString(Mod.Static.Settings.Tools)), _menu, OnReplaceToolsChanged);
            _fixToolColors = new HudAPIv2.MenuItem(ModText.MenuItemText_CI_FixToolColors.GetString(ToEnabledDisabledString(Mod.Static.Settings.FixToolColors)), _menu, OnReplaceFixToolColorsChanged);
            _hudApiRegistered = true;
        }

        /// <summary>
        ///     Invert Blocks option.
        /// </summary>
        private void OnReplaceBlocksChanged() {
            Mod.Static.SetOption(Option.Blocks, !Mod.Static.Settings.Blocks, false);
        }

        /// <summary>
        ///     Invert components option.
        /// </summary>
        private void OnReplaceComponentsChanged() {
            Mod.Static.SetOption(Option.Components, !Mod.Static.Settings.Components, false);
        }

        /// <summary>
        ///     Invert old components option.
        /// </summary>
        private void OnReplaceOldComponentsChanged() {
            Mod.Static.SetOption(Option.OldComponents, !Mod.Static.Settings.OldComponents, false);
        }

        /// <summary>
        ///     Invert ingots option.
        /// </summary>
        private void OnReplaceIngotsChanged() {
            Mod.Static.SetOption(Option.Ingots, !Mod.Static.Settings.Ingots, false);
        }

        /// <summary>
        ///     Invert ores option.
        /// </summary>
        private void OnReplaceOresChanged() {
            Mod.Static.SetOption(Option.Ores, !Mod.Static.Settings.Ores, false);
        }

        /// <summary>
        ///     Invert tools option.
        /// </summary>
        private void OnReplaceToolsChanged() {
            Mod.Static.SetOption(Option.Tools, !Mod.Static.Settings.Tools, false);
        }

        /// <summary>
        ///     Invert fix tools color option.
        /// </summary>
        private void OnReplaceFixToolColorsChanged() {
            Mod.Static.SetOption(Option.FixToolColors, !Mod.Static.Settings.FixToolColors, false);
        }
    }
}