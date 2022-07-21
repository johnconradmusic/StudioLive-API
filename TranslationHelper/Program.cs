using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Presonus.StudioLive32.Api.Console;
using Presonus.UC.Api.Components;

public class Program
{
	[STAThread]
	static void Main(string[] args)
	{
		//NewMethod();
		XmlDocument doc = new XmlDocument();
		doc.Load("c:\\dev\\model.xml");
		//HandleStringLists(doc.GetElementsByTagName("StringList"));
		HandleParamLists(doc.GetElementsByTagName("ParamList"));

		Console.ReadKey();
	}

	public class Param
	{
		public string? id;
		public string? name;
		public string? type;
		public string? def;
		public string? units;
		public int? min;
		public int? max;
		public int? mid;
		public string? curve;
		public int? steps;
	}

	private static void HandleParamLists(XmlNodeList lists)
	{
		string filepath = "C:\\Dev\\test.cs";
		foreach (XmlNode node1 in lists)
		{
			if (node1.NodeType == XmlNodeType.Comment) continue;
			var list = node1.ChildNodes;
			var classId = node1.Attributes["id"]?.InnerText;
			File.AppendAllLines(filepath, new[] { $"public class {classId} : ConsoleControlGroup, INotifyPropertyChanged {{" });

			/////class starts here, need to declare members without initliaizng them.
			//property change members first.

			File.AppendAllLines(filepath, new[] { "protected override void OnPropertyChanged(PropertyChangedEventArgs eventArgs) => PropertyChanged?.Invoke(this, eventArgs);" });
			File.AppendAllLines(filepath, new[] { "public event PropertyChangedEventHandler PropertyChanged;" });
			foreach (XmlNode node in list)
			{
				if (node.NodeType == XmlNodeType.Comment) continue;

				string id = node.Attributes["id"]?.InnerText;
				string name = node.Attributes["name"]?.InnerText;
				string type = node.Attributes["type"]?.InnerText;
				string def = node.Attributes["def"]?.InnerText;
				string units = node.Attributes["units"]?.InnerText;
				string min = node.Attributes["min"]?.InnerText;
				string max = node.Attributes["max"]?.InnerText;
				string mid = node.Attributes["mid"]?.InnerText;
				string curve = node.Attributes["curve"]?.InnerText;
				string steps = node.Attributes["steps"]?.InnerText;

				//ConsoleControl control = null;
				ParamCurve curveParam;
				ParamCurve.TryParse(curve, out curveParam);
				switch (type)
				{
					case "float":

						File.AppendAllLines(filepath, new[] { $"public ConsoleFloatDial {id} {{ get; set; }}" });

						break;
					case "int":

						File.AppendAllLines(filepath, new[] { $"public ConsoleIntDial {id} {{ get; set; }}" });

						break;
					case "toggle":

						File.AppendAllLines(filepath, new[] { $"public ConsoleToggleButton {id} {{ get; set; }}" });

						break;
					case "list":

						File.AppendAllLines(filepath, new[] { $"public ConsoleListControl {id} {{ get; set; }}" });

						break;
					case "color":
						break;
					case "string":
						File.AppendAllLines(filepath, new[] { $"public ConsoleStringField {id} {{ get; set; }}" });

						break;
				}
			}
			//declarations are done.. next build the constructor so we can initialize the fields;
			string ctor = $"public {classId}(){{";
			File.AppendAllLines(filepath, new[] { ctor });
			foreach (XmlNode node in list)
			{
				if (node.NodeType == XmlNodeType.Comment) continue;

				string id = node.Attributes["id"]?.InnerText;
				string name = node.Attributes["name"]?.InnerText;
				string type = node.Attributes["type"]?.InnerText;
				string def = node.Attributes["def"]?.InnerText + "f";
				string units = node.Attributes["units"]?.InnerText;
				string min = node.Attributes["min"]?.InnerText;
				string max = node.Attributes["max"]?.InnerText;
				string mid = node.Attributes["mid"]?.InnerText;
				string curve = node.Attributes["curve"]?.InnerText;
				string steps = node.Attributes["steps"]?.InnerText;

				//ConsoleControl control = null;
				ParamCurve curveParam;
				ParamCurve.TryParse(curve, out curveParam);
				switch (type)
				{
					case "float":

						File.AppendAllLines(filepath, new[] { $"{id} = new ConsoleFloatDial(){{Parent = this, Address = \"{id}\", Curve = ParamCurve.{curveParam.ToString()},Def = {def},Max = {max},Mid = {mid},Min = {min},Name = \"{name}\"}};"});

						break;
					case "int":

						File.AppendAllLines(filepath, new[] { $"{id} = new ConsoleIntDial(){{Parent = this, Address = \"{id}\", Curve = ParamCurve.{curveParam.ToString()},Def = {def},Max = {max},Mid = {mid},Min = {min},Name = \"{name}\"}};" });

						break;
					case "toggle":
						File.AppendAllLines(filepath, new[] { $"{id} = new ConsoleToggleButton(){{Parent = this, Address = \"{id}\", Name = \"{name}\"}};" });

						break;
					case "list":
						break;
					case "color":
						break;
					case "string":
						break;
				}
			}
			File.AppendAllLines(filepath, new[] { "}}" });
		}
		Console.ReadKey();
	}

	private static void HandleStringLists(XmlNodeList lists)
	{
		foreach (XmlNode node in lists)
		{
			if (node.NodeType == XmlNodeType.Comment) continue;

			var list = node.ChildNodes;
			foreach (XmlNode entry in list)
			{
				if (entry.NodeType == XmlNodeType.Comment) continue;
				string id = node.Attributes["id"]?.InnerText;
				string name = node.Attributes["name"]?.InnerText;
				string type = node.Attributes["type"]?.InnerText;
				string def = node.Attributes["def"]?.InnerText;
				string units = node.Attributes["units"]?.InnerText;
				string min = node.Attributes["min"]?.InnerText;
				string max = node.Attributes["max"]?.InnerText;
				string mid = node.Attributes["mid"]?.InnerText;
				string curve = node.Attributes["curve"]?.InnerText;
				string steps = node.Attributes["steps"]?.InnerText;
				//Console.WriteLine(node.Attributes["id"].Value + " ---- " + entry.Attributes["text"].Value);
			}
		}
	}

	private static void NewMethod()
	{
		XmlDocument doc = new XmlDocument();
		doc.Load("c:\\dev\\model.xml");
		StringBuilder stringBuilder = new StringBuilder();
		foreach (XmlNode node in doc.DocumentElement.ChildNodes)
		{
			string id = node.Attributes["id"]?.InnerText;
			string name = node.Attributes["name"]?.InnerText;
			string type = node.Attributes["type"]?.InnerText;
			string def = node.Attributes["def"]?.InnerText;
			string units = node.Attributes["units"]?.InnerText;
			string min = node.Attributes["min"]?.InnerText;
			string max = node.Attributes["max"]?.InnerText;
			string mid = node.Attributes["mid"]?.InnerText;
			string curve = node.Attributes["curve"]?.InnerText;
			string steps = node.Attributes["steps"]?.InnerText;
			stringBuilder.Append("public Param " + id + " = new (");
			if (name != null) stringBuilder.Append("name: " + "\"" + name + "\",");
			if (type != null) stringBuilder.Append("type: ParamType." + type.ToUpper());
			if (def != null) stringBuilder.Append(", def: " + def);
			if (def != null && def.Contains(".")) stringBuilder.Append("f");
			if (units != null) stringBuilder.Append(", units: " + "ParamUnits." + units.ToUpper());
			if (min != null) stringBuilder.Append(", min: " + min);
			if (max != null) stringBuilder.Append(", max: " + max);
			if (mid != null) stringBuilder.Append(", mid: " + mid);
			if (curve != null) stringBuilder.Append(", curve: " + "ParamCurve." + curve.ToUpper());
			if (steps != null) stringBuilder.Append(", steps: " + steps);

			if (stringBuilder[stringBuilder.Length - 1] == ',') stringBuilder.Remove(stringBuilder.Length - 1, 1);
			stringBuilder.Append(");");
			stringBuilder.Append("\n");


			//System.Console.WriteLine(stringBuilder.ToString());
		}

		System.Windows.Forms.Clipboard.SetText(stringBuilder.ToString());
	}
}