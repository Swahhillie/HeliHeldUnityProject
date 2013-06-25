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
		SpecialScore,
		LineGuide
	}
	
	private static Dictionary<string,Type> typeHelper= new Dictionary<string, Type>();
	private static bool init=false;
	
	public Type type;
	public string messageName;
	public Vector3 pos;
	public int specialScore;
	public float time;
	public GameObject go;
	public List<TriggeredObject> listeners;
	private string gameObjectName = "";
	public EventReaction(XmlNode node = null)
	{
		if(!init)
		{
			foreach(Type t in System.Enum.GetValues(typeof(Type))){
				typeHelper.Add(t.ToString(), t);
			}
			init=true;
			
		}
		if(node != null){
			messageName = node["MessageName"].InnerText;
			type = typeHelper[node["Type"].InnerText];
			specialScore = int.Parse(node["SpecialScore"].InnerText, System.Globalization.CultureInfo.InvariantCulture);
			
			XmlNode timeNode = node["Time"];
			if(timeNode != null)
			{
				time = float.Parse(timeNode.InnerText, System.Globalization.CultureInfo.InvariantCulture);
			}
			XmlNode gameObjectNode = node["GameObject"];
			if(gameObjectNode != null)
			{
				gameObjectName = gameObjectNode.InnerText;
			}
		}
		
	}
	public void Activate(){
		if(gameObjectName != ""){
			go = GameObject.Find(gameObjectName);
			if(go == null){
				Debug.LogError("Could not find gameObject " + gameObjectName);
			}	
		}
		foreach(TriggeredObject obj in listeners){
			if(obj == null) continue;
			Debug.Log("Fired eventreaction: " + type + " --> " + obj.name);
			obj.OnTriggered(this);
		}
	}

}
