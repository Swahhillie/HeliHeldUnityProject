using UnityEngine;
using System.Collections;
using System.Xml;

public class XMLVisitor : Visitor
{
	
	private XmlDocument _writeTarget;
	private XmlNode _activeObject; //object the current components should be added to
	
	private string levelBase = "levelBase.xml";
	private string outputFile = "levelOutput.xml";
	
	public XMLVisitor (XmlDocument target)
	{
		_writeTarget = target;
	}
	public XMLVisitor(string levelName, bool level = true){
		
		_writeTarget = new XmlDocument ();
		_writeTarget.Load (Application.dataPath + "\\" + levelBase);
		
		XmlNode root = _writeTarget.DocumentElement;
		
		XmlNode testChild;
		if(level)testChild = _writeTarget.CreateNode (XmlNodeType.Element, "Level", "");
		else testChild = _writeTarget.CreateNode (XmlNodeType.Element, "Menu", "");
	
		XmlElement tc = (XmlElement)testChild;//
		tc.SetAttribute ("name", levelName);
		
		root.AppendChild (testChild);
		
		if(level){
			XmlNode objectsXml = _writeTarget.CreateNode (XmlNodeType.Element, "Objects", "");
			testChild.AppendChild(objectsXml);	
		}
	}

	override public void Visit (Trigger v)
	{
		Debug.Log ("Visiting a trigger");
		
		
		foreach (TriggerValue t in v.triggers)
		{
			XmlNode triggerXml = _writeTarget.CreateNode (XmlNodeType.Element, "Trigger", null);
			_activeObject.AppendChild (triggerXml);
			
			
			
			foreach(EventReaction evr in t.eventReactions){
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
			timeToTriggerXml.InnerText = t.timeToTrigger.ToString();
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
		posXml.InnerText = evr.pos.ToString ();
		evrXml.AppendChild (posXml);
		XmlNode messageNameXml = _writeTarget.CreateNode (XmlNodeType.Element, "MessageName", null);
		messageNameXml.InnerText = evr.messageName;
		evrXml.AppendChild (messageNameXml);
		
		XmlNode listenersXml = _writeTarget.CreateNode(XmlNodeType.Element, "Listeners", null);
		
		foreach(TriggeredObject listener in evr.listeners){
			XmlNode listenerXml = _writeTarget.CreateNode(XmlNodeType.Element, "Listener", null);
			listenerXml.InnerXml = listener.gameObject.name;
			listenersXml.AppendChild(listenerXml);
		}
		evrXml.AppendChild(listenersXml);
	}
	
	override public void Visit (Castaway v)
	{
		Debug.Log ("Visiting a Castaway");
				
		CreateMissionObjectBaseXml("Castaway", v);
		
		
	}
	
	override public void Visit (Ship v)
	{
		Debug.Log ("Visiting a Ship");
		
	
		CreateMissionObjectBaseXml("Ship", v);
		
		
		
	}
	override public void Visit(Beacon beacon)
	{
		Debug.Log("Visiting a Beacon");
		CreateMissionObjectBaseXml("Beacon", beacon);	
	}
	
	private void CreateMissionObjectBaseXml(string name , MissionObjectBase mb)
	{
		XmlNode target = _writeTarget.CreateNode (XmlNodeType.Element, name, null);
		
		XmlNode spawnXml = _writeTarget.CreateNode (XmlNodeType.Element, "Spawn", null);
		spawnXml.InnerText = mb.spawn.ToString ();
		target.AppendChild (spawnXml);
		
		XmlNode prefabNameXml = _writeTarget.CreateNode (XmlNodeType.Element, "PrefabName", null);
		prefabNameXml.InnerText = mb.prefabName;
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
		posXml.InnerText = go.transform.position.ToString ();
		_activeObject.AppendChild (posXml);
		//rotation
		XmlNode rotXml = _writeTarget.CreateNode (XmlNodeType.Element, "Rot", null);
		rotXml.InnerText = go.transform.eulerAngles.ToString ();
		_activeObject.AppendChild (rotXml);
		
		_writeTarget.GetElementsByTagName ("Objects") [0].AppendChild (_activeObject);
				
	}
	override public void Visit(Button3D v){
		GameObject go  = v.gameObject;
		
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
		
		XmlNode functionXml = _writeTarget.CreateNode(XmlNodeType.Element, "Function", null);
		functionXml.InnerText = v.command;
		_activeObject.AppendChild (functionXml);
		
		
		
		_writeTarget.GetElementsByTagName ("Menu") [0].AppendChild (_activeObject);
		
	}
	public string Write(){
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
