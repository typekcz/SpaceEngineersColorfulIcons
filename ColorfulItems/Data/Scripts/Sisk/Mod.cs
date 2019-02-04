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

		/// <summary>
		///     Replace this with the content of a new created the definitions.txt.
		/// </summary>
		private readonly string[] _definitionIds = {
			"MyObjectBuilder_AmmoMagazine/NATO_5p56x45mm",
			"MyObjectBuilder_AmmoMagazine/NATO_25x184mm",
			"MyObjectBuilder_AmmoMagazine/Missile200mm",
			"MyObjectBuilder_Component/Construction",
			"MyObjectBuilder_Component/MetalGrid",
			"MyObjectBuilder_Component/InteriorPlate",
			"MyObjectBuilder_Component/SteelPlate",
			"MyObjectBuilder_Component/Girder",
			"MyObjectBuilder_Component/SmallTube",
			"MyObjectBuilder_Component/LargeTube",
			"MyObjectBuilder_Component/Motor",
			"MyObjectBuilder_Component/Display",
			"MyObjectBuilder_Component/BulletproofGlass",
			"MyObjectBuilder_Component/Superconductor",
			"MyObjectBuilder_Component/Computer",
			"MyObjectBuilder_Component/Reactor",
			"MyObjectBuilder_Component/Thrust",
			"MyObjectBuilder_Component/GravityGenerator",
			"MyObjectBuilder_Component/Medical",
			"MyObjectBuilder_Component/RadioCommunication",
			"MyObjectBuilder_Component/Detector",
			"MyObjectBuilder_Component/Explosives",
			"MyObjectBuilder_Component/SolarCell",
			"MyObjectBuilder_Component/PowerCell",
			"MyObjectBuilder_Component/Canvas",
			"MyObjectBuilder_PhysicalGunObject/GoodAIRewardPunishmentTool",
			"MyObjectBuilder_Ore/Stone",
			"MyObjectBuilder_Ore/Iron",
			"MyObjectBuilder_Ore/Nickel",
			"MyObjectBuilder_Ore/Cobalt",
			"MyObjectBuilder_Ore/Magnesium",
			"MyObjectBuilder_Ore/Silicon",
			"MyObjectBuilder_Ore/Silver",
			"MyObjectBuilder_Ore/Gold",
			"MyObjectBuilder_Ore/Platinum",
			"MyObjectBuilder_Ore/Uranium",
			"MyObjectBuilder_Ingot/Stone",
			"MyObjectBuilder_Ingot/Iron",
			"MyObjectBuilder_Ingot/Nickel",
			"MyObjectBuilder_Ingot/Cobalt",
			"MyObjectBuilder_Ingot/Magnesium",
			"MyObjectBuilder_Ingot/Silicon",
			"MyObjectBuilder_Ingot/Silver",
			"MyObjectBuilder_Ingot/Gold",
			"MyObjectBuilder_Ingot/Platinum",
			"MyObjectBuilder_Ingot/Uranium",
			"MyObjectBuilder_PhysicalGunObject/AutomaticRifleItem",
			"MyObjectBuilder_PhysicalGunObject/PreciseAutomaticRifleItem",
			"MyObjectBuilder_PhysicalGunObject/RapidFireAutomaticRifleItem",
			"MyObjectBuilder_PhysicalGunObject/UltimateAutomaticRifleItem",
			"MyObjectBuilder_OxygenContainerObject/OxygenBottle",
			"MyObjectBuilder_GasContainerObject/HydrogenBottle",
			"MyObjectBuilder_PhysicalGunObject/WelderItem",
			"MyObjectBuilder_PhysicalGunObject/Welder2Item",
			"MyObjectBuilder_PhysicalGunObject/Welder3Item",
			"MyObjectBuilder_PhysicalGunObject/Welder4Item",
			"MyObjectBuilder_PhysicalGunObject/AngleGrinderItem",
			"MyObjectBuilder_PhysicalGunObject/AngleGrinder2Item",
			"MyObjectBuilder_PhysicalGunObject/AngleGrinder3Item",
			"MyObjectBuilder_PhysicalGunObject/AngleGrinder4Item",
			"MyObjectBuilder_PhysicalGunObject/HandDrillItem",
			"MyObjectBuilder_PhysicalGunObject/HandDrill2Item",
			"MyObjectBuilder_PhysicalGunObject/HandDrill3Item",
			"MyObjectBuilder_PhysicalGunObject/HandDrill4Item",
			"MyObjectBuilder_Ore/Scrap",
			"MyObjectBuilder_Ingot/Scrap",
			"MyObjectBuilder_Ore/Ice",
			"MyObjectBuilder_BatteryBlock/LargeBlockBatteryBlock",
			"MyObjectBuilder_BatteryBlock/SmallBlockBatteryBlock",
			"MyObjectBuilder_CubeBlock/LargeBlockArmorBlock",
			"MyObjectBuilder_CubeBlock/LargeBlockArmorSlope",
			"MyObjectBuilder_CubeBlock/LargeBlockArmorCorner",
			"MyObjectBuilder_CubeBlock/LargeBlockArmorCornerInv",
			"MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorBlock",
			"MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorSlope",
			"MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorCorner",
			"MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorCornerInv",
			"MyObjectBuilder_CubeBlock/SmallBlockArmorBlock",
			"MyObjectBuilder_CubeBlock/SmallBlockArmorSlope",
			"MyObjectBuilder_CubeBlock/SmallBlockArmorCorner",
			"MyObjectBuilder_CubeBlock/SmallBlockArmorCornerInv",
			"MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorBlock",
			"MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorSlope",
			"MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorCorner",
			"MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorCornerInv",
			"MyObjectBuilder_CubeBlock/LargeHalfArmorBlock",
			"MyObjectBuilder_CubeBlock/LargeHeavyHalfArmorBlock",
			"MyObjectBuilder_CubeBlock/LargeHalfSlopeArmorBlock",
			"MyObjectBuilder_CubeBlock/LargeHeavyHalfSlopeArmorBlock",
			"MyObjectBuilder_CubeBlock/HalfArmorBlock",
			"MyObjectBuilder_CubeBlock/HeavyHalfArmorBlock",
			"MyObjectBuilder_CubeBlock/HalfSlopeArmorBlock",
			"MyObjectBuilder_CubeBlock/HeavyHalfSlopeArmorBlock",
			"MyObjectBuilder_CubeBlock/LargeBlockArmorRoundSlope",
			"MyObjectBuilder_CubeBlock/LargeBlockArmorRoundCorner",
			"MyObjectBuilder_CubeBlock/LargeBlockArmorRoundCornerInv",
			"MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorRoundSlope",
			"MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorRoundCorner",
			"MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorRoundCornerInv",
			"MyObjectBuilder_CubeBlock/SmallBlockArmorRoundSlope",
			"MyObjectBuilder_CubeBlock/SmallBlockArmorRoundCorner",
			"MyObjectBuilder_CubeBlock/SmallBlockArmorRoundCornerInv",
			"MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorRoundSlope",
			"MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorRoundCorner",
			"MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorRoundCornerInv",
			"MyObjectBuilder_CubeBlock/LargeBlockArmorSlope2Base",
			"MyObjectBuilder_CubeBlock/LargeBlockArmorSlope2Tip",
			"MyObjectBuilder_CubeBlock/LargeBlockArmorCorner2Base",
			"MyObjectBuilder_CubeBlock/LargeBlockArmorCorner2Tip",
			"MyObjectBuilder_CubeBlock/LargeBlockArmorInvCorner2Base",
			"MyObjectBuilder_CubeBlock/LargeBlockArmorInvCorner2Tip",
			"MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorSlope2Base",
			"MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorSlope2Tip",
			"MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorCorner2Base",
			"MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorCorner2Tip",
			"MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorInvCorner2Base",
			"MyObjectBuilder_CubeBlock/LargeHeavyBlockArmorInvCorner2Tip",
			"MyObjectBuilder_CubeBlock/SmallBlockArmorSlope2Base",
			"MyObjectBuilder_CubeBlock/SmallBlockArmorSlope2Tip",
			"MyObjectBuilder_CubeBlock/SmallBlockArmorCorner2Base",
			"MyObjectBuilder_CubeBlock/SmallBlockArmorCorner2Tip",
			"MyObjectBuilder_CubeBlock/SmallBlockArmorInvCorner2Base",
			"MyObjectBuilder_CubeBlock/SmallBlockArmorInvCorner2Tip",
			"MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorSlope2Base",
			"MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorSlope2Tip",
			"MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorCorner2Base",
			"MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorCorner2Tip",
			"MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorInvCorner2Base",
			"MyObjectBuilder_CubeBlock/SmallHeavyBlockArmorInvCorner2Tip",
			"MyObjectBuilder_TerminalBlock/ControlPanel",
			"MyObjectBuilder_MyProgrammableBlock/SmallProgrammableBlock",
			"MyObjectBuilder_TerminalBlock/SmallControlPanel",
			"MyObjectBuilder_LargeGatlingTurret/(null)",
			"MyObjectBuilder_LargeGatlingTurret/SmallGatlingTurret",
			"MyObjectBuilder_LargeMissileTurret/(null)",
			"MyObjectBuilder_LargeMissileTurret/SmallMissileTurret",
			"MyObjectBuilder_InteriorTurret/LargeInteriorTurret",
			"MyObjectBuilder_Passage/(null)",
			"MyObjectBuilder_Door/(null)",
			"MyObjectBuilder_RadioAntenna/LargeBlockRadioAntenna",
			"MyObjectBuilder_Beacon/LargeBlockBeacon",
			"MyObjectBuilder_Beacon/SmallBlockBeacon",
			"MyObjectBuilder_ReflectorLight/LargeBlockFrontLight",
			"MyObjectBuilder_InteriorLight/SmallLight",
			"MyObjectBuilder_InteriorLight/SmallBlockSmallLight",
			"MyObjectBuilder_InteriorLight/LargeBlockLight_1corner",
			"MyObjectBuilder_InteriorLight/LargeBlockLight_2corner",
			"MyObjectBuilder_InteriorLight/SmallBlockLight_1corner",
			"MyObjectBuilder_InteriorLight/SmallBlockLight_2corner",
			"MyObjectBuilder_CubeBlock/LargeWindowSquare",
			"MyObjectBuilder_CubeBlock/LargeWindowEdge",
			"MyObjectBuilder_CubeBlock/LargeStairs",
			"MyObjectBuilder_CubeBlock/LargeRamp",
			"MyObjectBuilder_CubeBlock/LargeSteelCatwalk",
			"MyObjectBuilder_CubeBlock/LargeSteelCatwalk2Sides",
			"MyObjectBuilder_CubeBlock/LargeSteelCatwalkCorner",
			"MyObjectBuilder_CubeBlock/LargeSteelCatwalkPlate",
			"MyObjectBuilder_CubeBlock/LargeCoverWall",
			"MyObjectBuilder_CubeBlock/LargeCoverWallHalf",
			"MyObjectBuilder_Warhead/LargeWarhead",
			"MyObjectBuilder_Warhead/SmallWarhead",
			"MyObjectBuilder_Decoy/LargeDecoy",
			"MyObjectBuilder_Decoy/SmallDecoy",
			"MyObjectBuilder_CubeBlock/LargeBlockInteriorWall",
			"MyObjectBuilder_CubeBlock/LargeInteriorPillar",
			"MyObjectBuilder_LandingGear/LargeBlockLandingGear",
			"MyObjectBuilder_Projector/LargeProjector",
			"MyObjectBuilder_Projector/SmallProjector",
			"MyObjectBuilder_Refinery/LargeRefinery",
			"MyObjectBuilder_Refinery/Blast Furnace",
			"MyObjectBuilder_OxygenGenerator/(null)",
			"MyObjectBuilder_Assembler/LargeAssembler",
			"MyObjectBuilder_OreDetector/LargeOreDetector",
			"MyObjectBuilder_MedicalRoom/LargeMedicalRoom",
			"MyObjectBuilder_GravityGenerator/(null)",
			"MyObjectBuilder_GravityGeneratorSphere/(null)",
			"MyObjectBuilder_JumpDrive/LargeJumpDrive",
			"MyObjectBuilder_Cockpit/LargeBlockCockpit",
			"MyObjectBuilder_Cockpit/LargeBlockCockpitSeat",
			"MyObjectBuilder_Cockpit/SmallBlockCockpit",
			"MyObjectBuilder_Cockpit/DBSmallBlockFighterCockpit",
			"MyObjectBuilder_Cockpit/CockpitOpen",
			"MyObjectBuilder_Cockpit/PassengerSeatLarge",
			"MyObjectBuilder_Cockpit/PassengerSeatSmall",
			"MyObjectBuilder_CryoChamber/LargeBlockCryoChamber",
			"MyObjectBuilder_LandingGear/SmallBlockLandingGear",
			"MyObjectBuilder_ReflectorLight/SmallBlockFrontLight",
			"MyObjectBuilder_SmallMissileLauncher/(null)",
			"MyObjectBuilder_SmallMissileLauncher/LargeMissileLauncher",
			"MyObjectBuilder_SmallMissileLauncherReload/SmallRocketLauncherReload",
			"MyObjectBuilder_SmallGatlingGun/(null)",
			"MyObjectBuilder_Drill/SmallBlockDrill",
			"MyObjectBuilder_Drill/LargeBlockDrill",
			"MyObjectBuilder_OreDetector/SmallBlockOreDetector",
			"MyObjectBuilder_SensorBlock/SmallBlockSensor",
			"MyObjectBuilder_SensorBlock/LargeBlockSensor",
			"MyObjectBuilder_SoundBlock/SmallBlockSoundBlock",
			"MyObjectBuilder_SoundBlock/LargeBlockSoundBlock",
			"MyObjectBuilder_TextPanel/SmallTextPanel",
			"MyObjectBuilder_TextPanel/SmallLCDPanelWide",
			"MyObjectBuilder_TextPanel/SmallLCDPanel",
			"MyObjectBuilder_TextPanel/LargeBlockCorner_LCD_1",
			"MyObjectBuilder_TextPanel/LargeBlockCorner_LCD_2",
			"MyObjectBuilder_TextPanel/LargeBlockCorner_LCD_Flat_1",
			"MyObjectBuilder_TextPanel/LargeBlockCorner_LCD_Flat_2",
			"MyObjectBuilder_TextPanel/SmallBlockCorner_LCD_1",
			"MyObjectBuilder_TextPanel/SmallBlockCorner_LCD_2",
			"MyObjectBuilder_TextPanel/SmallBlockCorner_LCD_Flat_1",
			"MyObjectBuilder_TextPanel/SmallBlockCorner_LCD_Flat_2",
			"MyObjectBuilder_OxygenTank/OxygenTankSmall",
			"MyObjectBuilder_OxygenGenerator/OxygenGeneratorSmall",
			"MyObjectBuilder_TextPanel/LargeTextPanel",
			"MyObjectBuilder_TextPanel/LargeLCDPanel",
			"MyObjectBuilder_TextPanel/LargeLCDPanelWide",
			"MyObjectBuilder_RadioAntenna/SmallBlockRadioAntenna",
			"MyObjectBuilder_RemoteControl/LargeBlockRemoteControl",
			"MyObjectBuilder_RemoteControl/SmallBlockRemoteControl",
			"MyObjectBuilder_AirVent/(null)",
			"MyObjectBuilder_AirVent/SmallAirVent",
			"MyObjectBuilder_OxygenTank/(null)",
			"MyObjectBuilder_OxygenTank/LargeHydrogenTank",
			"MyObjectBuilder_OxygenTank/SmallHydrogenTank",
			"MyObjectBuilder_UpgradeModule/LargeProductivityModule",
			"MyObjectBuilder_UpgradeModule/LargeEffectivenessModule",
			"MyObjectBuilder_UpgradeModule/LargeEnergyModule",
			"MyObjectBuilder_CargoContainer/SmallBlockSmallContainer",
			"MyObjectBuilder_CargoContainer/SmallBlockMediumContainer",
			"MyObjectBuilder_CargoContainer/SmallBlockLargeContainer",
			"MyObjectBuilder_CargoContainer/LargeBlockSmallContainer",
			"MyObjectBuilder_CargoContainer/LargeBlockLargeContainer",
			"MyObjectBuilder_Thrust/SmallBlockSmallThrust",
			"MyObjectBuilder_Thrust/SmallBlockLargeThrust",
			"MyObjectBuilder_Thrust/LargeBlockSmallThrust",
			"MyObjectBuilder_Thrust/LargeBlockLargeThrust",
			"MyObjectBuilder_Thrust/LargeBlockLargeHydrogenThrust",
			"MyObjectBuilder_Thrust/LargeBlockSmallHydrogenThrust",
			"MyObjectBuilder_Thrust/SmallBlockLargeHydrogenThrust",
			"MyObjectBuilder_Thrust/SmallBlockSmallHydrogenThrust",
			"MyObjectBuilder_Thrust/LargeBlockLargeAtmosphericThrust",
			"MyObjectBuilder_Thrust/LargeBlockSmallAtmosphericThrust",
			"MyObjectBuilder_Thrust/SmallBlockLargeAtmosphericThrust",
			"MyObjectBuilder_Thrust/SmallBlockSmallAtmosphericThrust",
			"MyObjectBuilder_CameraBlock/SmallCameraBlock",
			"MyObjectBuilder_CameraBlock/LargeCameraBlock",
			"MyObjectBuilder_Gyro/LargeBlockGyro",
			"MyObjectBuilder_Gyro/SmallBlockGyro",
			"MyObjectBuilder_Reactor/SmallBlockSmallGenerator",
			"MyObjectBuilder_Reactor/SmallBlockLargeGenerator",
			"MyObjectBuilder_Reactor/LargeBlockSmallGenerator",
			"MyObjectBuilder_Reactor/LargeBlockLargeGenerator",
			"MyObjectBuilder_PistonBase/LargePistonBase",
			"MyObjectBuilder_ExtendedPistonBase/LargePistonBase",
			"MyObjectBuilder_PistonTop/LargePistonTop",
			"MyObjectBuilder_PistonBase/SmallPistonBase",
			"MyObjectBuilder_ExtendedPistonBase/SmallPistonBase",
			"MyObjectBuilder_PistonTop/SmallPistonTop",
			"MyObjectBuilder_MotorStator/LargeStator",
			"MyObjectBuilder_MotorSuspension/Suspension3x3",
			"MyObjectBuilder_MotorSuspension/Suspension5x5",
			"MyObjectBuilder_MotorSuspension/Suspension1x1",
			"MyObjectBuilder_MotorSuspension/SmallSuspension3x3",
			"MyObjectBuilder_MotorSuspension/SmallSuspension5x5",
			"MyObjectBuilder_MotorSuspension/SmallSuspension1x1",
			"MyObjectBuilder_MotorSuspension/Suspension3x3mirrored",
			"MyObjectBuilder_MotorSuspension/Suspension5x5mirrored",
			"MyObjectBuilder_MotorSuspension/Suspension1x1mirrored",
			"MyObjectBuilder_MotorSuspension/SmallSuspension3x3mirrored",
			"MyObjectBuilder_MotorSuspension/SmallSuspension5x5mirrored",
			"MyObjectBuilder_MotorSuspension/SmallSuspension1x1mirrored",
			"MyObjectBuilder_MotorRotor/LargeRotor",
			"MyObjectBuilder_MotorStator/SmallStator",
			"MyObjectBuilder_MotorRotor/SmallRotor",
			"MyObjectBuilder_MotorAdvancedStator/LargeAdvancedStator",
			"MyObjectBuilder_MotorAdvancedRotor/LargeAdvancedRotor",
			"MyObjectBuilder_MotorAdvancedStator/SmallAdvancedStator",
			"MyObjectBuilder_MotorAdvancedRotor/SmallAdvancedRotor",
			"MyObjectBuilder_ButtonPanel/ButtonPanelLarge",
			"MyObjectBuilder_ButtonPanel/ButtonPanelSmall",
			"MyObjectBuilder_TimerBlock/TimerBlockLarge",
			"MyObjectBuilder_TimerBlock/TimerBlockSmall",
			"MyObjectBuilder_SolarPanel/LargeBlockSolarPanel",
			"MyObjectBuilder_SolarPanel/SmallBlockSolarPanel",
			"MyObjectBuilder_OxygenFarm/LargeBlockOxygenFarm",
			"MyObjectBuilder_CubeBlock/Window1x2Slope",
			"MyObjectBuilder_CubeBlock/Window1x2Inv",
			"MyObjectBuilder_CubeBlock/Window1x2Face",
			"MyObjectBuilder_CubeBlock/Window1x2SideLeft",
			"MyObjectBuilder_CubeBlock/Window1x2SideLeftInv",
			"MyObjectBuilder_CubeBlock/Window1x2SideRight",
			"MyObjectBuilder_CubeBlock/Window1x2SideRightInv",
			"MyObjectBuilder_CubeBlock/Window1x1Slope",
			"MyObjectBuilder_CubeBlock/Window1x1Face",
			"MyObjectBuilder_CubeBlock/Window1x1Side",
			"MyObjectBuilder_CubeBlock/Window1x1SideInv",
			"MyObjectBuilder_CubeBlock/Window1x1Inv",
			"MyObjectBuilder_CubeBlock/Window1x2Flat",
			"MyObjectBuilder_CubeBlock/Window1x2FlatInv",
			"MyObjectBuilder_CubeBlock/Window1x1Flat",
			"MyObjectBuilder_CubeBlock/Window1x1FlatInv",
			"MyObjectBuilder_CubeBlock/Window3x3Flat",
			"MyObjectBuilder_CubeBlock/Window3x3FlatInv",
			"MyObjectBuilder_CubeBlock/Window2x3Flat",
			"MyObjectBuilder_CubeBlock/Window2x3FlatInv",
			"MyObjectBuilder_Conveyor/SmallBlockConveyor",
			"MyObjectBuilder_Conveyor/LargeBlockConveyor",
			"MyObjectBuilder_Collector/Collector",
			"MyObjectBuilder_Collector/CollectorSmall",
			"MyObjectBuilder_ShipConnector/Connector",
			"MyObjectBuilder_ShipConnector/ConnectorSmall",
			"MyObjectBuilder_ShipConnector/ConnectorMedium",
			"MyObjectBuilder_ConveyorConnector/ConveyorTube",
			"MyObjectBuilder_ConveyorConnector/ConveyorTubeSmall",
			"MyObjectBuilder_ConveyorConnector/ConveyorTubeMedium",
			"MyObjectBuilder_ConveyorConnector/ConveyorFrameMedium",
			"MyObjectBuilder_ConveyorConnector/ConveyorTubeCurved",
			"MyObjectBuilder_ConveyorConnector/ConveyorTubeSmallCurved",
			"MyObjectBuilder_ConveyorConnector/ConveyorTubeCurvedMedium",
			"MyObjectBuilder_Conveyor/SmallShipConveyorHub",
			"MyObjectBuilder_ConveyorSorter/LargeBlockConveyorSorter",
			"MyObjectBuilder_ConveyorSorter/MediumBlockConveyorSorter",
			"MyObjectBuilder_ConveyorSorter/SmallBlockConveyorSorter",
			"MyObjectBuilder_VirtualMass/VirtualMassLarge",
			"MyObjectBuilder_VirtualMass/VirtualMassSmall",
			"MyObjectBuilder_SpaceBall/SpaceBallLarge",
			"MyObjectBuilder_SpaceBall/SpaceBallSmall",
			"MyObjectBuilder_Wheel/SmallRealWheel1x1",
			"MyObjectBuilder_Wheel/SmallRealWheel",
			"MyObjectBuilder_Wheel/SmallRealWheel5x5",
			"MyObjectBuilder_Wheel/RealWheel1x1",
			"MyObjectBuilder_Wheel/RealWheel",
			"MyObjectBuilder_Wheel/RealWheel5x5",
			"MyObjectBuilder_Wheel/SmallRealWheel1x1mirrored",
			"MyObjectBuilder_Wheel/SmallRealWheelmirrored",
			"MyObjectBuilder_Wheel/SmallRealWheel5x5mirrored",
			"MyObjectBuilder_Wheel/RealWheel1x1mirrored",
			"MyObjectBuilder_Wheel/RealWheelmirrored",
			"MyObjectBuilder_Wheel/RealWheel5x5mirrored",
			"MyObjectBuilder_Wheel/Wheel1x1",
			"MyObjectBuilder_Wheel/SmallWheel1x1",
			"MyObjectBuilder_Wheel/Wheel3x3",
			"MyObjectBuilder_Wheel/SmallWheel3x3",
			"MyObjectBuilder_Wheel/Wheel5x5",
			"MyObjectBuilder_Wheel/SmallWheel5x5",
			"MyObjectBuilder_ShipGrinder/LargeShipGrinder",
			"MyObjectBuilder_ShipGrinder/SmallShipGrinder",
			"MyObjectBuilder_ShipWelder/LargeShipWelder",
			"MyObjectBuilder_ShipWelder/SmallShipWelder",
			"MyObjectBuilder_MergeBlock/LargeShipMergeBlock",
			"MyObjectBuilder_MergeBlock/SmallShipMergeBlock",
			"MyObjectBuilder_CubeBlock/ArmorCenter",
			"MyObjectBuilder_MyProgrammableBlock/LargeProgrammableBlock",
			"MyObjectBuilder_CubeBlock/ArmorCorner",
			"MyObjectBuilder_CubeBlock/ArmorInvCorner",
			"MyObjectBuilder_CubeBlock/ArmorSide",
			"MyObjectBuilder_CubeBlock/SmallArmorCenter",
			"MyObjectBuilder_CubeBlock/SmallArmorCorner",
			"MyObjectBuilder_CubeBlock/SmallArmorInvCorner",
			"MyObjectBuilder_CubeBlock/SmallArmorSide",
			"MyObjectBuilder_LaserAntenna/LargeBlockLaserAntenna",
			"MyObjectBuilder_LaserAntenna/SmallBlockLaserAntenna",
			"MyObjectBuilder_AirtightHangarDoor/(null)",
			"MyObjectBuilder_AirtightSlideDoor/LargeBlockSlideDoor",
			"MyObjectBuilder_Parachute/LgParachute",
			"MyObjectBuilder_Parachute/SmParachute",
			"MyObjectBuilder_WindTurbine/LargeBlockWindTurbine",
			"MyObjectBuilder_HydrogenEngine/LargeHydrogenEngine",
			"MyObjectBuilder_HydrogenEngine/SmallHydrogenEngine",
			"MyObjectBuilder_BatteryBlock/SmallBlockSmallBatteryBlock",
			"MyObjectBuilder_SurvivalKit/SurvivalKitLarge",
			"MyObjectBuilder_SurvivalKit/SurvivalKit",
			"MyObjectBuilder_Assembler/BasicAssembler"
		};

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
		private void ChangeIcon(MyDefinitionBase definition) {
			if (definition?.Icons != null && definition.Icons.Any()) {
				if (!definition.Icons[0].StartsWith(ModContext.ModPath)) {
					if (!_replacedIcons.ContainsKey(definition)) {
						_replacedIcons.Add(definition, definition.Icons[0]);
					}

					definition.Icons[0] = $"{ModContext.ModPath}\\{definition.Icons[0]}";
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
				if (blueprint?.Id != null && Enumerable.Contains(_definitionIds, blueprint.Id.ToString())) {
					ChangeIcon(blueprint);
				}
			}

			foreach (var definition in definitions) {
				if (definition?.Id != null && Enumerable.Contains(_definitionIds, definition.Id.ToString())) {
					if (definition is MyCubeBlockDefinition || definition is MyPhysicalItemDefinition) {
						ChangeIcon(definition);
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
