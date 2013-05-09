using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
[System.SerializableAttribute()]
public class EventReaction 
{
	public enum Type{
		Spawn,
		Destroy,
		Say,
		Displace,
		Enable,
		Disable
	}
	
	private static Dictionary<string,Type> typeHelper= new Dictionary<string, Type>();
	private static bool init=false;
	
	public Type type;
	public string messageName;
	public Vector3 pos;
	public List<TriggeredObject> listeners;
	
	public EventReaction(XmlNode node)
	{
		if(!init)
		{
			init=true;
			typeHelper["Spawn"]=Type.Spawn;
			typeHelper["Destroy"]=Type.Destroy;	
			typeHelper["Say"]=Type.Say;	
			typeHelper["Displace"]=Type.Displace;
			typeHelper["Enable"]=Type.Enable;
			typeHelper["Disable"]=Type.Disable;
		}

		messageName = node["MessageName"].InnerText;
		type = typeHelper[node["Type"].InnerText];
	}
	public void Activate(){
		foreach(TriggeredObject obj in listeners){
			obj.OnTriggered(this);
		}
	}

}
