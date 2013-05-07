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
	}
	
	override public void Visit(Castaway v){
		Debug.Log("Visiting a Castaway");
	}
	
	override public void Visit(Ship v){
		Debug.Log("Visiting a Ship");
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
