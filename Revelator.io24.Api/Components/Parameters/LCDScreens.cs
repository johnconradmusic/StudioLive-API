using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presonus.UC.Api.Components.Parameters
{
	public class LCDScreens : ParameterBase
	{
		public Param channelOverviewPage = new("Channel Overview Page", ParamType.TOGGLE);
		public Param channelSettingsPage = new("Channel Settings Page", ParamType.TOGGLE);
		public Param channelTypePage = new("Channel Type Page", ParamType.TOGGLE);
		public Param channelInsertPage = new("Channel Insert Page", ParamType.TOGGLE);
		public Param eqPage = new("EQ Page", ParamType.TOGGLE);
		public Param gatePage = new("Gate Page", ParamType.TOGGLE);
		public Param compPage = new("Compressor Page", ParamType.TOGGLE);
		public Param limiterPage = new("Limiter Page", ParamType.TOGGLE);
		public Param auxSendsPage = new("Aux Sends Page", ParamType.TOGGLE);
		public Param userPage = new("User Page", ParamType.TOGGLE);
		public Param networkPage = new("Network Page", ParamType.TOGGLE);
		public Param advancedScenesPage = new("Scenes Page", ParamType.TOGGLE);
		public Param advancedSceneListEditorPage = new("Advanced Scene List Editor Page", ParamType.TOGGLE);
		public Param sceneSafePage = new("Scene Safe Page", ParamType.TOGGLE);
		public Param advancedSceneFiltersPage = new("Advanced Scene Filters Page", ParamType.TOGGLE);
		public Param startPage = new("Start Page", ParamType.TOGGLE);
		public Param subgroupPage = new("Subgroup Page", ParamType.TOGGLE);
		public Param systemPage = new("System Page", ParamType.TOGGLE);
		public Param softPowerConfirmationPage = new("Soft Power Confirmation Page", ParamType.TOGGLE);
		public Param mcuPage = new("MCU Page", ParamType.TOGGLE);
		public Param huiPage = new("HUI Page", ParamType.TOGGLE);
		public Param audioRoutingPage = new("Audio Routing Page", ParamType.TOGGLE);
		public Param utilsPage = new("Utilities Page", ParamType.TOGGLE);
		public Param obsRemotePage = new("OBS Remote Page", ParamType.TOGGLE);
		public Param stageBoxSetupPage = new("StageBox Setup Page", ParamType.TOGGLE);
		public Param earmixSetupPage = new("Earmix Setup Page", ParamType.TOGGLE);
		public Param digitalPatchingPage = new("Digital Patching Page", ParamType.TOGGLE);
		public Param digitalPatchingResetPage = new("Digital Reset Page", ParamType.TOGGLE);
		public Param inputAVBStreamsPage = new("Input AVB Streams Page", ParamType.TOGGLE);
		public Param inputSelectionPage = new("Input Selection Page", ParamType.TOGGLE);
		public Param signalGenPage = new("Signal Gen Page", ParamType.TOGGLE);
		public Param userProfilePage = new("User Profile Page", ParamType.TOGGLE);
		public Param userProfileLoginPage = new("User Profile Login Page", ParamType.TOGGLE);
		public Param userProfileLoginLoadScenePage = new("User Profile Login Load Scene Page", ParamType.TOGGLE);
		public Param userProfileSettingsPage = new("User Profile Settings Page", ParamType.TOGGLE);
		public Param userProfileAvatarPage = new("User Profile Avatar Page", ParamType.TOGGLE);
		public Param defaultProjectScenePage = new("Default Project Scene Page", ParamType.TOGGLE);
		public Param userProfilePasswordPage = new("User Profile Password Page", ParamType.TOGGLE);
		public Param userProfilePermissionsPage = new("User Profile Permissions Page", ParamType.TOGGLE);
		public Param auxMuteModePage = new("Aux Mute Mode Page", ParamType.TOGGLE);
		public Param manualIPEditPage = new("Static Manual IP Edit Page", ParamType.TOGGLE);
		public Param groupsPage = new("DCA Groups Page", ParamType.TOGGLE);
		public Param fxPage = new("FX Page", ParamType.TOGGLE);
		public Param insertPage = new("Insert Page", ParamType.TOGGLE);
		public Param fxrackPage = new("FX Rack Page", ParamType.TOGGLE);
		public Param btregPage = new("Bluetooth Reg Page", ParamType.TOGGLE);
		public Param userFunctionsPage = new("User Functions Page", ParamType.TOGGLE);
		public Param capturePage = new("Capture Page", ParamType.TOGGLE);
		public Param sdFormatPage = new("SD Card Format Page", ParamType.TOGGLE);
		public Param demo1 = new("Demo 1", ParamType.TOGGLE);
		public Param demo2 = new("Demo 2", ParamType.TOGGLE);
		public Param demo3 = new("Demo 3", ParamType.TOGGLE);
		public Param demo4 = new("Demo 4", ParamType.TOGGLE);
		public Param demo5 = new("Demo 5", ParamType.TOGGLE);
		public Param geqPage = new("GEQ", ParamType.TOGGLE);
		public Param geqEditPage = new("GEQ Edit", ParamType.TOGGLE);
		public Param soloEditPage = new("Solo Edit Page", ParamType.TOGGLE);
		public Param talkbackPage = new("Talkback Edit Page", ParamType.TOGGLE);
		public Param monitorPage = new("Monitor Page", ParamType.TOGGLE);
		public Param fatchPresetsPage = new("Channel Presets Page", ParamType.TOGGLE);
		public Param channelFilterPage = new("Channel Filter Page", ParamType.TOGGLE);
		public Param geqPresetsPage = new("GEQ Presets Page", ParamType.TOGGLE);
		public Param fxPresetsPage = new("FX Presets Page", ParamType.TOGGLE);
		public Param insertsPresetsPage = new("Inserts Presets Page", ParamType.TOGGLE);
		public Param firmwareUpdatePage = new("Firmware Update Page", ParamType.TOGGLE);
		public Param permissionsPage = new("Permissions Page", ParamType.TOGGLE);
		public Param servicePage = new("Service Page", ParamType.TOGGLE);
		public Param lcdTestPage = new("LCD Test Page", ParamType.TOGGLE);
		public Param tapeEditPage = new("Tape Edit Page", ParamType.TOGGLE);
		public Param userLayerEditPage = new("User Layer Edit Page", ParamType.TOGGLE);
		public Param workflowOptionsPage = new("Workflow Options Page", ParamType.TOGGLE);
		public Param calibrateTouchscreenPage = new("Calibrate Touchscreen Page", ParamType.TOGGLE);
		public Param recordArmPage = new("Record Arm Page", ParamType.TOGGLE);
		public Param dawChannelOverview = new("DAW Channel Overview", ParamType.TOGGLE);
		public Param dawChannelSettings = new("DAW Channel Settings", ParamType.TOGGLE);
		public Param dawGenericParameters = new("DAW Generic Parameters", ParamType.TOGGLE);
		public Param dawTransport = new("DAW Transport", ParamType.TOGGLE);
		public Param dawChannelEditor = new("DAW Channel Editor", ParamType.TOGGLE);
		public Param dawMacroMapping = new("DAW Macro Mapping", ParamType.TOGGLE);
		public Param boot = new("Boot Screen", ParamType.TOGGLE);
		public Param progress = new("Progress Screen", ParamType.TOGGLE);

		public LCDScreens(string path) : base(path)
		{
		}
	}
}
