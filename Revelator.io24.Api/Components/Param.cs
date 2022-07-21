using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace Presonus.UC.Api.Components;

public enum ParamType
{
	TOGGLE, LIST, STRING, FLOAT, COLOR, INT
}
public class ParamUnits : List<string>
{
	ParamUnits(params string[] values) : base(values) { }

	public static ParamUnits YES_NO = new("No", "Yes");
	public static ParamUnits ANALOG_SOURCE = new();
	public static ParamUnits DIGITAL_SOURCE = new();
	public static ParamUnits SD_SOURCE = new();
	public static ParamUnits EMPTYPARAMLIST = new();
	public static ParamUnits GAIN = new();
	public static ParamUnits FREQ = new();
	public static ParamUnits PERCENT = new();
	public static ParamUnits MS = new();
	public static ParamUnits CH_GAIN = new();
	public static ParamUnits PAN = new();
	public static ParamUnits AVBNUM = new();
	public static ParamUnits USBNUM = new();
	public static ParamUnits SIGNALTYPELIST = new("Pink Noise", "White Noise", "Sine");
	public static ParamUnits SOLOMODELIST = new("Latch", "Radio", "CR");
	public static ParamUnits STAGEBOXMODELIST = new("Stand-alone", "Monitor Mixer", "Stagebox");
	public static ParamUnits PANMODELIST = new("Stereo", "LCR");
	public static ParamUnits SAMPLERATELIST = new("44.1 kHz", "48 kHz");
	public static ParamUnits ON_OFF = new("Off", "On");
	public static ParamUnits LOWMEDHIGH = new("Low", "Med", "High");
	public static ParamUnits AUXMUTEMODELIST = new("Unlinked", "Main Mutes Auxes", "All Aux Mute Link", "Global Mute Link");
	public static ParamUnits ASSIGNABLESTEREOSOURCES = new(
		"Main L/R",
		"Flex Mix 1/2",
		"Flex Mix 3/4",
		"Flex Mix 5/6",
		"Flex Mix 7/8",
		"Flex Mix 9/10",
		"Flex Mix 11/12",
		"Flex Mix 13/14",
		"Flex Mix 15/16",
		"Subgroups A/B",
		"Subgroups C/D"
		);
	public static ParamUnits EARMIXROUTELIST = new(
		"None",
		"AVB Send 1-8",
		"AVB Send 9-16",
		"AVB Send 17-24",
		"AVB Send 25-32",
		"AVB Send 33-40",
		"AVB Send 41-48",
		"AVB Send 49-56",
		"AVB Send 57-64");
	public static ParamUnits AVBCLOCKSOURCELIST = new("Internal", "Network Stream");
	public static ParamUnits AVBCLOCKSTATUSLIST = new("No Source", "No Sync", "Sync");
	public static ParamUnits STAGEBOXTYPELIST = new("Stagebox 8in", "Mixer", "Stagebox 16in");
	public static ParamUnits NETWORKLATENCYLIST = new("-select-", "0.28 ms", "0.50 ms", "0.75 ms", "1.00 ms", "1.25 ms", "1.50 ms", "1.75 ms", "2.00 ms");
	public static ParamUnits ALLPARAMLIST = new("All");
	public static ParamUnits NONEPARAMLIST = new("None");
	public static ParamUnits IPASSIGNMODE = new("Dynamic", "Static Self Assigned", "Static Manual Assigned");
	public static ParamUnits BLUETOOTHLINKSTATELIST = new("Power Off", "Standby", "Connected", "Pairing");
	public static ParamUnits INSERTSLOTLIST = new("None", "Insert 1", "Insert 2", "Insert 3", "Insert 4");
	public static ParamUnits PREPOSTLIST = new("Pre", "Post");
	public static ParamUnits INPUTSOURCELIST = new("Analog", "Network", "USB", "SD Card");
	public static ParamUnits PREAMPMODELIST = new("Pre", "Gain");
	public static ParamUnits DIGITALSENDSOURCELIST = new("Analog", "Digital");
	public static ParamUnits BUSMODELIST = new("Aux", "Subgroup", "Matrix");
	public static ParamUnits AUXMODELIST = new("Pre1", "Pre2", "Post");
	public static ParamUnits BUSSOURCELIST = new("Mixer", "Network");
	public static ParamUnits SDCARDTYPELIST = new("Unknown", "SD", "SDHC");
	public static ParamUnits SDFORMATTYPELIST = new("Quick", "Erase", "Overwrite");
	public static ParamUnits FOLDERHIERARCHYLIST = new("Artist-Performance-Location", "Location-Performance-Artist");
	public static ParamUnits SESSIONLOADSTATUSLIST = new("No Session", "Busy", "Session Loaded", "Sample Rate Mismatch", "Load Failed");

}
public enum ParamCurve
{
	LINEAR, EXP, FADER, LOG
}



public class Param
{
	public Param(ParameterBase sender, string name = null, ParamType type = default, float def = default, ParamUnits units = null, int min = default, int max = default, int mid = default, ParamCurve curve = default, int steps = default)
	{
		this.name = name;
		this.type = type;
		this.def = def;
		this.units = units;
		this.min = min;
		this.max = max;
		this.mid = mid;
		this.curve = curve;
		this.steps = steps;
		this.parentPath = sender.path;
	}

	public string parentPath;
	public string name;
	public ParamType type;
	public float def;
	public ParamUnits units;
	public int min;
	public int max;
	public int mid;
	public ParamCurve curve;
	public int steps;
	public object Value { get => GetValue(); set => SetValue(value); }

	private object GetValue()
	{
		return 0;
	}

	private void SetValue(object value, [CallerMemberName] string fieldName = "")
	{

	}
}


