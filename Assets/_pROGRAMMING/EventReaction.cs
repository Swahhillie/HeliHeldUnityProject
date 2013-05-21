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
		Disable,
<<<<<<< HEAD
		Count
=======
		StartTimer,
		Count,
<<<<<<< HEAD
		Animate
>>>>>>> added animator
=======
		Animate,
		Highlight
>>>>>>> EventReaction and Highlight update
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
			foreach(Type t in System.Enum.GetValues(typeof(Type))){
				typeHelper.Add(t.ToString(), t);
			}
			init=true;
			
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
