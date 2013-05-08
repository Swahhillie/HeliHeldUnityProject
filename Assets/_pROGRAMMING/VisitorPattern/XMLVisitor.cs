using UnityEngine;
using System.Collections;
using System.Xml;

public class XMLVisitor : Visitor
{
	
	private XmlDocument _writeTarget;
	private XmlNode _activeObject; //object the current components should be added to
	
	public XMLVisitor (XmlDocument target)
	{
		_writeTarget = target;
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
			
			
			triggerXml.AppendChild (radiusXml);
			triggerXml.AppendChild (typeXml);
			
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
	}
	
	override public void Visit (Castaway v)
	{
		Debug.Log ("Visiting a Castaway");
		XmlNode castawayXml = _writeTarget.CreateNode (XmlNodeType.Element, "Castaway", null);
		XmlNode spawnXml = _writeTarget.CreateNode (XmlNodeType.Element, "Spawn", null);
		spawnXml.InnerText = v.spawn.ToString ();
		castawayXml.AppendChild (spawnXml);
		
		XmlNode prefabNameXml = _writeTarget.CreateNode (XmlNodeType.Element, "PrefabName", null);
		prefabNameXml.InnerText = v.prefabName;
		castawayXml.AppendChild (prefabNameXml);
		
		_activeObject.AppendChild (castawayXml);
	}
	
	override public void Visit (Ship v)
	{
		Debug.Log ("Visiting a Ship");
		XmlNode shipXml = _writeTarget.CreateNode (XmlNodeType.Element, "Ship", null);
		XmlNode spawnXml = _writeTarget.CreateNode (XmlNodeType.Element, "Spawn", null);
		spawnXml.InnerText = v.spawn.ToString ();
		shipXml.AppendChild (spawnXml);
		
		XmlNode prefabNameXml = _writeTarget.CreateNode (XmlNodeType.Element, "PrefabName", null);
		prefabNameXml.InnerText = v.prefabName;
		shipXml.AppendChild (prefabNameXml);
		
		_activeObject.AppendChild (shipXml);
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
		
		_writeTarget.GetElementsByTagName ("Level") [0].AppendChild (_activeObject);
				
	}

	public XmlNode GetXmlResult ()
	{
		return _writeTarget;
	}
	
}
