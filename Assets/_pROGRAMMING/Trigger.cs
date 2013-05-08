using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
[System.Serializable()]
public class TriggerValue
{
	public EventReaction eventReaction;
	public float radius;
	public TriggerType type;
	public float timeRemaining;
	public List<MissionObjectBase> listeners;
	
	public TriggerValue (EventReaction eventReaction, TriggerType type, List<MissionObjectBase> listeners, float radius, float time)
	{
		this.eventReaction = eventReaction;
		this.radius = radius;
		this.type = type;
		this.timeRemaining = time;
		this.listeners = listeners;
	}
	public void Activate(){
		foreach(MissionObjectBase obj in listeners)
			obj.OnTriggered(this.eventReaction, this.type);
	}
//	public string eventName {
//		get{ return _eventName;}
//	}
//	
//	public float radius {
//		get{ return _radius;}
//	}
//	
//	public TriggerType type {
//		get{ return _type;}
//	}
//	
//	public float timeRemaining {
//		get{ return _timeRemaining;}
//		set{ _timeRemaining = value;}
//
//	}

}

public class Trigger : MonoBehaviour, IVisitable
{
	//private List<SpecMission> specmissions = new List<SpecMission>();
	public List<TriggerValue> triggers = new List<TriggerValue>(); // used to define multiple triggers in one component
	
	public delegate void TriggerCallback(TriggerValue t);
	
	public void AddTriggerValue (EventReaction eventReaction, TriggerType type, List<MissionObjectBase> missionObjects, float radius = 0, float time = 0)//,List<SpecMission> Missions)
	{
		//Debug.Log(aName+aRadius+aType+aTime);
		TriggerValue t = new TriggerValue (eventReaction, type, missionObjects, radius, time);


		if (type == TriggerType.OnTriggerEnter || type == TriggerType.OnTriggerExit) {
			if (t.radius == 0)
				Debug.LogError ("Created a collider trigger with 0 radius");
			CapsuleCollider cc = this.gameObject.GetOrAddComponent <CapsuleCollider> ();
			cc.isTrigger = true;
			
			
			float h = 0;
			if (ConfigLoader.GetValue ("capsuleHeight", ref h))
				Debug.Log ("Height is set to " + cc.height);
			else
				Debug.LogError ("FAIL");
			
			cc.height = h;
			cc.radius = radius;
		}
		triggers.Add (t);
	}
	public void AcceptVisitor(Visitor v){
		v.Visit(this);
	}
	void Update ()
	{
		TriggerValue trigger = triggers.Find (t => t.type == TriggerType.OnOutOfLive);
		if (trigger != null) {
			if (trigger.timeRemaining <= 0) {
				TriggerActivate(trigger);
				triggers.Remove (trigger);
			}
			trigger.timeRemaining -= Time.deltaTime;
		}
		
	}
	public void CreateXml(ref XmlDocument doc, ref XmlNode objNode){
		foreach(TriggerValue t in triggers){
			XmlNode triggerXml = doc.CreateNode(XmlNodeType.Element, "Trigger",null);
			objNode.AppendChild(triggerXml);
			
			XmlNode reactionXml = doc.CreateNode(XmlNodeType.Element, "EventReaction", null);
			reactionXml.InnerText = t.eventReaction.ToString();
			XmlNode radiusXml = doc.CreateNode(XmlNodeType.Element, "Radius", null);
			radiusXml.InnerText =	t.radius.ToString();
			XmlNode typeXml = doc.CreateNode(XmlNodeType.Element, "Type", null);
			typeXml.InnerText = t.type.ToString();
			
			triggerXml.AppendChild(reactionXml);
			triggerXml.AppendChild(radiusXml);
			triggerXml.AppendChild(typeXml);
		}
	}
	void OnTriggerEnter ()
	{
		
		//trigger enter and exit should be ignored if its not the player
		TriggerValue trigger = triggers.Find (t => t.type == TriggerType.OnOutOfLive);
		if (trigger != null) {
			TriggerActivate(trigger);
			triggers.Remove (trigger);
		}
	
	}

	void OnTriggerExit ()
	{
		//trigger enter and exit should be ignored if its not the player, use unity physics settings
		for (int i = triggers.Count -1; i >= 0; i--) {
			TriggerValue t = triggers [i];
			if (t.type == TriggerType.OnTriggerExit) {
				TriggerActivate(t);
				triggers.RemoveAt (i);
					
			}
				
		}
	}

	void OnDestroy ()
	{
		for (int i = triggers.Count -1; i >= 0; i--) {
			TriggerValue t = triggers [i];
			if (t.type == TriggerType.OnDeath) { //this should probably be changed to onDestroy, because even saved castaways are eventually destroyed
				TriggerActivate(t);
				triggers.RemoveAt (i);
					
			}
				
		}	
	}
	private void TriggerActivate(TriggerValue trigger){
		//called if a trigger is activated, calls all the functions that are listening
		trigger.Activate();
	}
}
