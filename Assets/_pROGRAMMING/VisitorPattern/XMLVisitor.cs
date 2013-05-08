using UnityEngine;
using System.Collections;
using System.Xml;

public class XMLVisitor : Visitor {
	
	private XmlDocument _writeTarget;
	private XmlNode _activeObject; //object the current components should be added to
	
	public XMLVisitor(XmlDocument target){
		_writeTarget = target;
	}
	override public void Visit(Trigger v){
		Debug.Log("Visiting a trigger");
		
		v.CreateXml(ref _writeTarget, ref _activeObject);
		
	}
	
	override public void Visit(Castaway v){
		Debug.Log("Visiting a Castaway");
		XmlNode castawayXml = _writeTarget.CreateNode(XmlNodeType.Element, "Castaway", null);
		XmlNode spawnXml = _writeTarget.CreateNode(XmlNodeType.Element, "Spawn", null);
		spawnXml.InnerText = v.spawn.ToString();
		castawayXml.AppendChild(spawnXml);
		
		XmlNode prefabNameXml = _writeTarget.CreateNode(XmlNodeType.Element, "PrefabName", null);
		prefabNameXml.InnerText = v.prefabName;
		castawayXml.AppendChild(prefabNameXml);
		
		_activeObject.AppendChild(castawayXml);
	}
	
	override public void Visit(Ship v){
		Debug.Log("Visiting a Ship");
		XmlNode shipXml = _writeTarget.CreateNode(XmlNodeType.Element, "Ship", null);
		XmlNode spawnXml = _writeTarget.CreateNode(XmlNodeType.Element, "Spawn", null);
		spawnXml.InnerText = v.spawn.ToString();
		shipXml.AppendChild(spawnXml);
		
		XmlNode prefabNameXml = _writeTarget.CreateNode(XmlNodeType.Element, "PrefabName", null);
		prefabNameXml.InnerText = v.prefabName;
		shipXml.AppendChild(prefabNameXml);
		
		_activeObject.AppendChild(shipXml);
	}
	
	
	public void OpenNewObject(GameObject go){
		Debug.Log("Saving new object with name " + go.name);
	//create tthe new node
		_activeObject = _writeTarget.CreateNode(XmlNodeType.Element, "Object", null);
		
		//properties of objects
		//name
		XmlNode nameXml = _writeTarget.CreateNode(XmlNodeType.Element, "Name", null);
		nameXml.InnerText = go.name;
		_activeObject.AppendChild(nameXml);
		//position
		XmlNode posXml = _writeTarget.CreateNode(XmlNodeType.Element, "Pos", null);
		posXml.InnerText = go.transform.position.ToString();
		_activeObject.AppendChild(posXml);
		//rotation
		XmlNode rotXml = _writeTarget.CreateNode(XmlNodeType.Element, "Rot", null);
		rotXml.InnerText = go.transform.eulerAngles.ToString();
		_activeObject.AppendChild(rotXml);
		
		_writeTarget.GetElementsByTagName("Level")[0].AppendChild(_activeObject);
				
	}
	public XmlNode GetXmlResult(){
		return _writeTarget;
	}
	
}
