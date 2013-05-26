using UnityEngine;
using System.Collections;
using System.Xml;

public class XMLVisitor : Visitor
{
	
	private XmlDocument _writeTarget;
	private XmlNode _activeObject; //object the current components should be added to
	
	private XmlNode _savingTo;
	private XmlNode _levelsNode;
	private XmlNode _messagesNode;
	private XmlNode _settingsNode;
	private XmlNode _menusNode;
	public static string outputFile = "levelOutput.xml";
	public static string configFile = "config.xml";
	public static bool useExsitingConfig = false;
	public static bool overridePreviousLevelWithName = false;
	
	public enum ToSave
	{
		Level,
		Menu,
		Messages
	}
	
	public XMLVisitor (string levelName, ToSave toSave)
	{
		if(useExsitingConfig){
			LoadExsitingFile (levelName, toSave);		
		}
		else{
			PrepareNewFile (levelName, toSave);
		}
		
	}

	private void PrepareNewFile (string levelName, ToSave toSave)
	{
		_writeTarget = new XmlDocument ();
		
		XmlDeclaration xmlDecl = _writeTarget.CreateXmlDeclaration ("1.0", null, null);
		
		XmlNode root = _writeTarget.CreateNode (XmlNodeType.Element, "Root", null);
		_writeTarget.AppendChild (root);
		//xmlDecl.InsertBefore(xmlDecl, root);
		
		switch (toSave) {
		case ToSave.Level:
			{
				
				_savingTo = _writeTarget.CreateNode (XmlNodeType.Element, "Level", "");
				XmlElement tc = (XmlElement)_savingTo;//
				tc.SetAttribute ("name", levelName);
				CreateAddNode(_writeTarget, "Objects", _savingTo);
				break;
			}
		case ToSave.Menu:
			{
				_savingTo = _writeTarget.CreateNode (XmlNodeType.Element, "Menu", "");
				XmlElement tc = (XmlElement)_savingTo;//
				tc.SetAttribute ("name", levelName);
				break;
			}
		case ToSave.Messages:
			_savingTo = _writeTarget.CreateNode (XmlNodeType.Element, "Messages", "");
			break;
		default:
			_savingTo = null;
			break;
		}
		
		root.AppendChild (_savingTo);
	
	}

	private void LoadExsitingFile (string levelName, ToSave toSave)
	{
		_writeTarget = new XmlDocument ();
		_writeTarget.Load (Application.dataPath + "\\" + configFile);
		
		//Debug.Log("Doc element = " + _writeTarget.DocumentElement.Name); // Heli Held
		XmlNode docElement = _writeTarget.DocumentElement;
		
		
		_levelsNode = docElement.SelectSingleNode ("Levels");
		ReportIfNull(_levelsNode, "Levels");
		
		_settingsNode = docElement.SelectSingleNode ("Settings");
		ReportIfNull(_settingsNode, "Settings");
		
		_messagesNode = docElement.SelectSingleNode ("Messages");
		ReportIfNull(_messagesNode, "Messages");
		
		_menusNode = docElement.SelectSingleNode ("Menus");
		ReportIfNull(_menusNode, "Menus");
		
		switch (toSave) {
		case ToSave.Level:
			{	
				//check to see if there is a level with this name
				_savingTo = _levelsNode.SelectSingleNode(string.Format("Level[@name='{0}']", levelName));
		
				if(_savingTo != null){
					
					// there is a level with this name
					
					if(overridePreviousLevelWithName)
					{
						//removing the old level
						Debug.LogWarning("Overriding a previously saved level");
						_levelsNode.RemoveChild(_savingTo);
						
					}
					else
					{	
						//let the old level sit there. will cause issues loading
						Debug.LogError("Creating a new level in the config file with an identical name!");
					}
				}
				//create the new level
				_savingTo = CreateAddNode (_writeTarget, "Level", _levelsNode);
				XmlElement tc = _savingTo as XmlElement;
				tc.SetAttribute ("name", levelName);
				
				CreateAddNode(_writeTarget, "Objects", _savingTo);
			}	
			break;
			
		case ToSave.Menu:
			{
				_savingTo = CreateAddNode (_writeTarget, "Menu", _menusNode);
				XmlElement tc = _savingTo as XmlElement;
				tc.SetAttribute ("name", levelName);
			}	
			break;
		case ToSave.Messages:
			docElement.RemoveChild (_messagesNode);
			_messagesNode = CreateAddNode (_writeTarget, "Messages", _writeTarget.DocumentElement);
			_savingTo = _messagesNode;
			break;
		}
	}
	private void ReportIfNull(XmlNode node, string name)
	{
		if(node == null) Debug.Log(string.Format("{0} node is null, the document is not complete", name));
	}
	override public void Visit (Trigger v)
	{
		Debug.Log ("Visiting a trigger");
		
		
		foreach (TriggerValue t in v.triggers) {
			XmlNode triggerXml = _writeTarget.CreateNode (XmlNodeType.Element, "Trigger", null);
			_activeObject.AppendChild (triggerXml);
			
			
			
			foreach (EventReaction evr in t.eventReactions) {
				XmlNode reactionXml = _writeTarget.CreateNode (XmlNodeType.Element, "EventReaction", null);
				AddEventReaction (ref reactionXml, evr);
				triggerXml.AppendChild (reactionXml);
			}
			
			
			XmlNode radiusXml = _writeTarget.CreateNode (XmlNodeType.Element, "Radius", null);
			radiusXml.InnerText = t.radius.ToString ();
			XmlNode typeXml = _writeTarget.CreateNode (XmlNodeType.Element, "Type", null);
			typeXml.InnerText = t.type.ToString ();
			
			//saving the repeat count and the trigger count
			XmlNode repeatCountXml = _writeTarget.CreateNode (XmlNodeType.Element, "RepeatCount", null);
			repeatCountXml.InnerText = t.maxRepeatCount.ToString ();
			XmlNode timeToTriggerXml = _writeTarget.CreateNode (XmlNodeType.Element, "TimeToTrigger", null);
			timeToTriggerXml.InnerText = t.timeToTrigger.ToString ();
			XmlNode triggerCountXml = _writeTarget.CreateNode (XmlNodeType.Element, "CountToTrigger", null);
			triggerCountXml.InnerText = t.countToTrigger.ToString ();
			
			
			triggerXml.AppendChild (radiusXml);
			triggerXml.AppendChild (typeXml);
			triggerXml.AppendChild (repeatCountXml);
			triggerXml.AppendChild (timeToTriggerXml);
			triggerXml.AppendChild (triggerCountXml);
			
		}
	}
	
	private void AddEventReaction (ref XmlNode evrXml, EventReaction evr)
	{
		
		XmlNode typeXml = _writeTarget.CreateNode (XmlNodeType.Element, "Type", null);
		typeXml.InnerText = evr.type.ToString ();
		evrXml.AppendChild (typeXml);
		XmlNode posXml = _writeTarget.CreateNode (XmlNodeType.Element, "Pos", null);
		posXml.InnerText = evr.pos.ToString ("G4");
		evrXml.AppendChild (posXml);
		XmlNode messageNameXml = _writeTarget.CreateNode (XmlNodeType.Element, "MessageName", null);
		messageNameXml.InnerText = evr.messageName;
		evrXml.AppendChild (messageNameXml);
		
		XmlNode listenersXml = _writeTarget.CreateNode (XmlNodeType.Element, "Listeners", null);
		
		foreach (TriggeredObject listener in evr.listeners) {
			if (listener != null) {
				XmlNode listenerXml = _writeTarget.CreateNode (XmlNodeType.Element, "Listener", null);
				listenerXml.InnerXml = listener.gameObject.name;
				listenersXml.AppendChild (listenerXml);
			}
		}
		evrXml.AppendChild (listenersXml);
	}
	
	override public void Visit (Castaway v)
	{
		Debug.Log ("Visiting a Castaway");
				
		CreateMissionObjectBaseXml ("Castaway", v);
		
		
	}
	
	override public void Visit (Ship v)
	{
		Debug.Log ("Visiting a Ship");
		
	
		CreateMissionObjectBaseXml ("Ship", v);
		
		
		
	}

	override public void Visit (Beacon beacon)
	{
		Debug.Log ("Visiting a Beacon");
		CreateMissionObjectBaseXml ("Beacon", beacon);	
	}
	
	private void CreateMissionObjectBaseXml (string name, MissionObjectBase mb)
	{
		XmlNode target = _writeTarget.CreateNode (XmlNodeType.Element, name, null);
		
		XmlNode spawnXml = _writeTarget.CreateNode (XmlNodeType.Element, "Spawn", null);
		spawnXml.InnerText = mb.spawn.ToString ();
		target.AppendChild (spawnXml);
		
		XmlNode prefabNameXml = _writeTarget.CreateNode (XmlNodeType.Element, "PrefabName", null);
		prefabNameXml.InnerText = mb.prefab.name;
		target.AppendChild (prefabNameXml);
		
		_activeObject.AppendChild (target);
	}

	public void OpenNewObject (GameObject go)
	{
		Debug.Log ("Saving new object with name " + go.name);
		//create tthe new node
		_activeObject = _writeTarget.CreateNode (XmlNodeType.Element, "Object", null);
		
		//properties of objects
		//name
		XmlNode nameXml = _writeTarget.CreateNode (XmlNodeType.Element, "Name", null);
		nameXml.InnerText = go.name;
		_activeObject.AppendChild (nameXml);
		//position
		XmlNode posXml = _writeTarget.CreateNode (XmlNodeType.Element, "Pos", null);
		posXml.InnerText = go.transform.position.ToString ("G4");
		_activeObject.AppendChild (posXml);
		//rotation
		XmlNode rotXml = _writeTarget.CreateNode (XmlNodeType.Element, "Rot", null);
		rotXml.InnerText = go.transform.eulerAngles.ToString ("G4");
		_activeObject.AppendChild (rotXml);
		
		//_writeTarget.GetElementsByTagName ("Objects") [0].AppendChild (_activeObject);
		_savingTo.SelectSingleNode ("Objects").AppendChild (_activeObject);
				
	}

	override public void Visit (Button3D v)
	{
		GameObject go = v.gameObject;
		
		_activeObject = _writeTarget.CreateNode (XmlNodeType.Element, "Button", null);
		
		//type
		XmlNode typeXml = _writeTarget.CreateNode (XmlNodeType.Element, "Type", null);
		typeXml.InnerText = v.type.ToString ();
		_activeObject.AppendChild (typeXml);
		
		//name
		
		XmlNode nameXml = _writeTarget.CreateNode (XmlNodeType.Element, "Name", null);
		nameXml.InnerText = go.name;
		_activeObject.AppendChild (nameXml);
		
		//label
		XmlNode labelXml = _writeTarget.CreateNode (XmlNodeType.Element, "Label", null);
		labelXml.InnerText = v.textMesh.text;
		_activeObject.AppendChild (labelXml);
		//position
		XmlNode posXml = _writeTarget.CreateNode (XmlNodeType.Element, "Pos", null);
		posXml.InnerText = go.transform.position.ToString ();
		_activeObject.AppendChild (posXml);
		//rotation
		XmlNode rotXml = _writeTarget.CreateNode (XmlNodeType.Element, "Rot", null);
		rotXml.InnerText = go.transform.eulerAngles.ToString ();
		_activeObject.AppendChild (rotXml);
		
		XmlNode functionXml = _writeTarget.CreateNode (XmlNodeType.Element, "Function", null);
		functionXml.InnerText = v.command;
		_activeObject.AppendChild (functionXml);
		
		
		
		//_writeTarget.GetElementsByTagName ("Menu") [0].AppendChild (_activeObject);
		_savingTo.AppendChild (_activeObject);
		
	}

	override public void Visit (Message message)
	{
		_activeObject = _writeTarget.CreateNode (XmlNodeType.Element, "Message", null);
		//name
		CreateAddNode (_writeTarget, "Name", _activeObject).InnerText = message.name;
		//text
		CreateAddNode (_writeTarget, "Text", _activeObject).InnerText = message.text;
		//audio
		CreateAddNode (_writeTarget, "Audio", _activeObject).InnerText = message.audio != null ? message.audio.name : "";
		
//_writeTarget.GetElementsByTagName ("Messages") [0].AppendChild (_activeObject);
		_messagesNode.AppendChild (_activeObject);
	}

	private static XmlNode CreateAddNode (XmlDocument doc, string name, XmlNode parent)
	{
		XmlNode node = doc.CreateNode (XmlNodeType.Element, name, null);
		parent.AppendChild (node);
		return node;
	}

	public string Write ()
	{
		//wrint to the output file. note that the asset database does not update automatically, minimize / reload unity first
		XmlTextWriter writer = new XmlTextWriter (Application.dataPath + "\\" + outputFile, System.Text.Encoding.ASCII);
		writer.Formatting = Formatting.Indented;
		//doc.Save(Application.dataPath + "\\" +outputFile);
		_writeTarget.Save (writer);
		writer.Close ();
		return Application.dataPath + "\\" + outputFile;
	}
		
	public XmlNode GetXmlResult ()
	{
		return _writeTarget;
	}
	
}
