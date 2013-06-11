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
		StartTimer,
		Count,
		Animate,
		Highlight_Activate,
		Highlight_Deactivate,
		EndLevel,
		SpecialScore
	}
	
	private static Dictionary<string,Type> typeHelper= new Dictionary<string, Type>();
	private static bool init=false;
	
	public Type type;
	public string messageName;
	public Vector3 pos;
	public int specialScore;
	public float time;
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
		specialScore = int.Parse(node["SpecialScore"].InnerText, System.Globalization.CultureInfo.InvariantCulture);
		
		XmlNode timeNode = node["Time"];
		if(timeNode != null)
		{
			time = float.Parse(timeNode.InnerText, System.Globalization.CultureInfo.InvariantCulture);
		}
	}
	public void Activate(){
		foreach(TriggeredObject obj in listeners){
			Debug.Log("Fired eventreaction: " + type + " --> " + obj.name);
			obj.OnTriggered(this);
		}
	}

}
