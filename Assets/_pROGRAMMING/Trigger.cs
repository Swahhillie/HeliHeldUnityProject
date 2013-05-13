using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

[System.Serializable()]
public class TriggerValue
{
	
	
	public TriggerType type;
	public float radius;
	public float timeToTrigger;
	private float _timeRemaining;
	public int maxRepeatCount = 1; // how many times should this trigger go off
	private int currentRepeatCount = 0; // howw many times has the trigger gone off
	public int countToTrigger = 1; // how many times does this have to be set off before it activates	
	private int currentTriggeredCount = 0; //how many times has the trigger been added to.
	
	public List<EventReaction> eventReactions;
	
	public TriggerValue (List<EventReaction> eventReactions, TriggerType type, float radius, float time, int triggerCount, int repeatCount)
	{
		this.eventReactions = eventReactions;
		this.radius = radius;
		this.type = type;
		this.timeRemaining = time;
		this.timeToTrigger = time;
		this.countToTrigger = triggerCount;
		this.maxRepeatCount = repeatCount;
	}

	public void Activate ()
	{
		
		if (isRunning) {
			if (type == TriggerType.Counting) {
				if (currentTriggeredCount < countToTrigger) {
					currentTriggeredCount ++;
					if (currentTriggeredCount == countToTrigger) {
						//activate the evrs
					
						//reset the counter, if the trigger has a repeat count it should start counting again
						currentTriggeredCount = 0;
					
					} else {
						//one has been added to the trigger count
						return;
					}
				}
			}
		
			foreach (EventReaction evr in eventReactions) {
				evr.Activate ();
			}
			currentRepeatCount ++;
		}
	}

	public void Update ()
	{
		if (isRunning) {
			_timeRemaining -= Time.deltaTime;
			if (_timeRemaining < 0) {//activate the trigger and reset it.
				Activate ();
				_timeRemaining = timeToTrigger;
			}
		}
	}

	public float timeRemaining {
		set{ _timeRemaining = value;}
	}

	public bool isRunning {
		get{ return currentRepeatCount < maxRepeatCount;}
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

public class Trigger : TriggeredObject, IVisitable
{
	//private List<SpecMission> specmissions = new List<SpecMission>();
	public List<TriggerValue> triggers = new List<TriggerValue> (); // used to define multiple triggers in one component
	
	public delegate void TriggerCallback (TriggerValue t);
	
	public void Start ()
	{
		Debug.Log ("Changing " + this.name + " to save layer");
		gameObject.layer = LayerMask.NameToLayer ("SaveLayer");
	}
	
	public void AddTriggerValue (List<EventReaction> eventReactions, TriggerType type, float radius = 0, float time = 0, int triggerCount = 1, int repeatCount = 1)//,List<SpecMission> Missions)
	{
		//Debug.Log(aName+aRadius+aType+aTime);
		TriggerValue t = new TriggerValue (eventReactions, type, radius, time, triggerCount, repeatCount);


		if (type == TriggerType.OnTriggerEnter || type == TriggerType.OnTriggerExit) {
			if (t.radius == 0)
				Debug.LogError ("Created a collider trigger with 0 radius");
			CapsuleCollider cc = this.gameObject.GetOrAddComponent <CapsuleCollider> ();
			//adding a rigibody to the trigger
			Rigidbody rb = this.gameObject.GetOrAddComponent<Rigidbody> ();
			rb.constraints = RigidbodyConstraints.FreezeAll;
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

	public void OnDrawGizmos ()
	{
		//draw the radius of all triggers
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
		Gizmos.color = new Color(0, 1, 0, 1);
=======
>>>>>>> Editor Utility to save to xml file
=======
		Gizmos.color = new Color(0, 1, 0, 1);
>>>>>>> Small updates
		triggers.FindAll(x=>x.type == TriggerType.OnTriggerEnter || x.type == TriggerType.OnTriggerExit)
			.ForEach(x=> Gizmos.DrawWireSphere(transform.position, x.radius));
=======
		Gizmos.color = new Color (0, 1, 0, 1);
		triggers.FindAll (x => x.type == TriggerType.OnTriggerEnter || x.type == TriggerType.OnTriggerExit)
			.ForEach (x => Gizmos.DrawWireSphere (transform.position, x.radius));
>>>>>>> Changes to make triggers more flexible
	}

	public void AcceptVisitor (Visitor v)
	{
		v.Visit (this);
	}

	void Update ()
	{
		triggers.FindAll (t => t.type == TriggerType.OnOutOfLive).ForEach (x => x.Update ());
		
	}
	
	void OnTriggerEnter (Collider collider)
	{
		Debug.Log ("Trigger set of by " + collider.name);
		if (collider.CompareTag ("Player")) {
			Debug.Log ("Player triggered ", this.gameObject);
			//trigger enter and exit should be ignored if its not the player
			
			triggers.FindAll (t => t.type == TriggerType.OnTriggerEnter).ForEach (x => TriggerActivate (x));
		}
			
	}

	void OnTriggerExit (Collider collider)
	{
		
		if (collider.CompareTag ("Player")) {
			//trigger enter and exit should be ignored if its not the player, use unity physics settings
			triggers.FindAll (t => t.type == TriggerType.OnTriggerExit).ForEach (x => TriggerActivate (x));
		}
		
	}

	void OnDestroy ()
	{
		triggers.FindAll (t => t.type == TriggerType.OnDeath).ForEach (x => TriggerActivate (x));
		
		/*for (int i = triggers.Count -1; i >= 0; i--) {
			TriggerValue t = triggers [i];
			if (t.type == TriggerType.OnDeath) { //this should probably be changed to onDestroy, because even saved castaways are eventually destroyed
				TriggerActivate(t);
				triggers.RemoveAt (i);
					
			}
				
		}
		*/	
	}

	public void OnRescue ()
	{
		//called by a mission object base when it is rescued
		triggers.FindAll (t => t.type == TriggerType.OnRescued).ForEach (x => TriggerActivate (x));
	}

	private void TriggerActivate (TriggerValue trigger)
	{
		//called if a trigger is activated, calls all the functions that are listening
		trigger.Activate ();
	}
	
	override public void OnTriggered (EventReaction evr)
	{
		Debug.Log ("Triggered was triggered, this is usefull for counting");
		switch (evr.type) {
		case EventReaction.Type.Count:
			triggers.FindAll (t => t.type == TriggerType.Counting).ForEach (x => TriggerActivate (x));
			break;
			
		default:
			Debug.Log ("Trigger cannot handle eventtype " + evr.type);
			break;	
		}
	}
}
