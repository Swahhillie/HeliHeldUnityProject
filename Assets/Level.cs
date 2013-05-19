using System.Xml;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System;

public class Level
{
	public Level (string name, XmlNode xml)
	{
		levelName = name;
		lvlXml = xml;
	}

	public string levelName;
	public XmlNode lvlXml;
	public List<GameObject> levelElements; //list of all gameobjects that will be active in this level.
	public GameObject lvlRoot;
	private bool _isLoaded = false;
	private int _castawayCount = 0;
	private int _shipCount = 0;

	public void LoadLevel () // make the level real, creates the gameobjects from the stored xml
	{
	
		
		levelElements = new List<GameObject> ();
		//parse the xml file and instantiate the gameobjects
		lvlRoot = new GameObject ("lvlRoot" + levelName);
		//get all the objects that need to be created
		XmlNodeList elements = lvlXml ["Objects"].ChildNodes;
		
		StringBuilder report = new StringBuilder ();
		foreach (XmlNode e in elements) {//looping over all <object>
			//create all the objects here
			GameObject go = new GameObject (e ["Name"].InnerText);
			
			go.transform.position = ConfigLoader.ParseVec3 (e ["Pos"].InnerText);
			go.transform.eulerAngles = ConfigLoader.ParseVec3 (e ["Rot"].InnerText);
			
			
			go.transform.parent = lvlRoot.transform;
			report.Append ("Created GameObject " + go.name + " at loc " + go.transform.position + " with rotation " + go.transform.eulerAngles);
			
			//check if this objects needs to have a trigger on it.
			
			report.Append ("\n\tcomponents[");
			
			
			//check if this should have a ship attached
			XmlNode shipXml = e ["Ship"];
			if (shipXml != null) {
				AddShip (shipXml, ref go);
				_shipCount ++;
				report.Append ("Ship,");
			}
			
			//check for a castaway
			XmlNode castaXml = e ["Castaway"];
			if (castaXml != null) {
				AddCastaway (castaXml, ref go);			
				_castawayCount ++;
				report.Append ("Castaway,");
			}
			
			
			//XmlNode triggerXml = e ["Trigger"];
			foreach (XmlNode triggerXml in e.SelectNodes("Trigger")) {
				if (triggerXml != null) {
					//there is a trigger xml node in this object. add it to the object.
					AddTrigger (triggerXml, ref go);	
					report.Append ("Trigger,");	
				}
			}
			levelElements.Add (go);
			report.Append ("]\n");
			
			
		}
		_isLoaded = true;
		Debug.Log (report.ToString ());
	}

	public int castawayCount {
		get{ return _castawayCount;}
	}

	public int shipCount {
		get{ return _shipCount;}
	}

	public void RemoveLevelElement (MissionObjectBase obj) //ships and castaways
	{
		
		if (obj.type == MissionObject.Ship) {
			_shipCount--;
		}
		if (obj.type == MissionObject.Castaway) {
			_castawayCount --;
		}
		levelElements.Remove(obj.gameObject);
	}

	public void UnLoadLevel ()
	{
		Debug.Log ("Unloading level " + levelName);
		for (int i = levelElements.Count -1; i >= 0; i --) {
			GameObject.Destroy (levelElements [i]);
		}
		_isLoaded = false;
		_castawayCount = 0;
		_shipCount = 0;
			
	}
	//---------------------Object building functions--------------------------------
	
	private static void AddTrigger (XmlNode triggerXml, ref GameObject go)
	{
		//Adding a trigger and its values to the gameobject
	
		Trigger tr = go.GetOrAddComponent<Trigger> ();
		
		TriggerType triggerType = ConfigLoader.triggerTypes [triggerXml ["Type"].InnerText];
		
		
		XmlNodeList reactionList = triggerXml.SelectNodes ("EventReaction");
		List<EventReaction> eventReactions = new List<EventReaction> ();
		
		//parse the reactions
		foreach (XmlNode reactionXml in reactionList) {
		
			EventReaction eventReaction = new EventReaction (reactionXml);
			
			eventReactions.Add (eventReaction);
			
			eventReaction.listeners = new List<TriggeredObject> (); //go.GetComponent<MissionObjectBase>();
			XmlNodeList listenerNamesList = reactionXml.SelectNodes ("Listeners/Listener");
			List<string> listenerNames = new List<string> ();
			foreach (XmlNode listenerNode in listenerNamesList) {
				listenerNames.Add (listenerNode.InnerText);
			}
			
			
			tr.StartCoroutine (ConfigLoader.FindListeners (eventReaction, listenerNames.ToArray ()));
			
		}
		float radius = 0;		
		if (triggerType == TriggerType.OnTriggerEnter || triggerType == TriggerType.OnTriggerExit) {
			radius = float.Parse (triggerXml ["Radius"].InnerText);
				
		}
		float time = float.Parse (triggerXml ["TimeToTrigger"].InnerText);	
		
		int repeatCount = int.Parse (triggerXml ["RepeatCount"].InnerText);
		
		int triggerCount = int.Parse (triggerXml ["CountToTrigger"].InnerText);
		
		tr.AddTriggerValue (eventReactions, triggerType, radius, time, triggerCount, repeatCount);
		
		
		
		
		
		//TriggerData triggerData = new TriggerData(blab blah blah);
		//tr.AddEvent(triggerData);
	
		
	}
	
	private static void AddShip (XmlNode shipXml, ref GameObject go)
	{
		//add component ship here and set the proper values
		Ship c = AddMissionBase<Ship> (shipXml, go);
		if(UnityEngine.Random.value > .999)Debug.Log("I'm on a boat.");
		
	}

	private static void AddCastaway (XmlNode cawXml, ref GameObject go)
	{
		Castaway c = AddMissionBase<Castaway> (cawXml, go);
	}

	private static void AddBeacon (XmlNode bNode, ref GameObject go)
	{
		Beacon b = AddMissionBase<Beacon> (bNode, go);
	}

	private static T AddMissionBase<T> (XmlNode baseNode, GameObject go) where T : MissionObjectBase
	{
		
		//basic setup for all mission object bases
		T mib = go.GetOrAddComponent<T> ();
		mib.spawn = (MissionObjectBase.SpawnType)System.Enum.Parse (typeof(MissionObjectBase.SpawnType), baseNode ["Spawn"].InnerText);
		
		string prefabName = baseNode ["PrefabName"].InnerText;
		mib.prefabName = prefabName;
		GameObject model = GameObject.Instantiate (Resources.Load (prefabName)) as GameObject;
		model.transform.parent = go.transform;
		model.transform.localPosition = Vector3.zero;
		mib.Sleep2FramesAndDisable();
		return mib;
	}
	
	public bool isLoaded
	{
		get{return _isLoaded;}
	}


	

}
