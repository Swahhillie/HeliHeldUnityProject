using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class MissionObjectBase : TriggeredObject, IVisitable
{


	protected float maxlifetime = 0;
	protected float lifetime = 0;
	protected bool spawned = false;
	protected bool saved = false;
	public enum SpawnType
	{
		Start = 0,
		Later = 1
	}
	public SpawnType spawn = SpawnType.Start;
	protected MissionObject _type;
	public GameObject prefab;
	protected bool _rescuable = false;
	//protected Dictionary<string,Reaction> evt = new Dictionary<string,Reaction>();
	//protected Dictionary<int,List<string>> action = new Dictionary<int, List<string>>();
	
	private void Awake ()
	{
		_type = MissionObject.None;
		AwakeConcrete ();
	}

	public void Start ()
	{
		Debug.Log ("Changing " + this.name + " to save layer", this.gameObject);
		gameObject.layer = LayerMask.NameToLayer ("SaveLayer");
	}
		
	protected abstract void AwakeConcrete ();

	public virtual void SetVariables (int spawn, float aLifetime)
	{

		maxlifetime = aLifetime;
		lifetime = maxlifetime;

	
	}

	override public void OnTriggered (EventReaction evr)
	{
		//override for behaviour
		Debug.Log (gameObject.name + " received eventReaction of type : " + evr.type);
		switch (evr.type) {
		case EventReaction.Type.Spawn:
			Debug.LogError("Spawn is not yet used", this);
			//GameObject go = (GameObject)Instantiate(Resources.Load(evr.messageName), evr.pos, Quaternion.identity);
			//ConfigLoader.instance.activeLevel.AddTemporary(go);
			break;
		case EventReaction.Type.Destroy:
				
			GameObject.Destroy (gameObject);
			break;
		case EventReaction.Type.Displace:
			transform.position = evr.pos;
			break;
		case EventReaction.Type.Enable:
			gameObject.SetActive (true);
			break;
		case EventReaction.Type.Disable:
			gameObject.SetActive (false);
			break;
		default:
			Debug.Log ("Mission Object base does not implement " + evr.type + " eventreaction");
			break;
		}
	}

	public bool AttemptRescue ()
	{
		//Attempt rescue is called by the helicopter if it is in position and the player makes a rescue attempt.
		
		bool rescueSuccess = false;
		//some functionality here to determine if the rescue should fail or succeed
		rescueSuccess = true;
		
		//if this object has a trigger attached to it that goes off on rescue. Make sure that it is called.
		Trigger t = gameObject.GetComponent<Trigger> ();
		if (t != null) {
			t.OnRescue ();
		}
		ConfigLoader.instance.activeLevel.RemoveLevelElement(this.gameObject);
		GameObject.Destroy(this.gameObject);
		return rescueSuccess;
	}
	public bool rescuable{
		get{return _rescuable;}
	}
/*	
	protected virtual void CreateModel(string toLoad = "")
	{
		if(toLoad == ""){
			Debug.LogError("Loading nothing");
			return;
		}
		GameObject model = (GameObject)GameObject.Instantiate(Resources.Load(toLoad),pos,rot);
		model.name=name;
		spawned=true;
		model.transform.parent=this.gameObject.transform;
		int key=0;
		List<string> eventlist;
		if(action.TryGetValue(key,out eventlist))
		{
			for(int x=eventlist.Count-1;x>=0;--x)
			{
				Events.FireEvent(eventlist[x]);
			}
		}
	}
	*/
	/*
	private void Update () 
	{
		if(maxlifetime>0&&spawned)
		{
			if(lifetime>0)
			{
				lifetime-=Time.deltaTime;
			}
			if(lifetime<=0)
			{
				int key = 3;
				List<string> eventlist;
				if(action.TryGetValue(key,out eventlist))
				{
					for(int x=eventlist.Count-1;x>=0;--x)
					{
						Events.FireEvent(eventlist[x]);
					}
				}
				Destroy(this.gameObject);
			}
			else{
				UpdateConcrete ();	
			}
			//do something
		}
		
	}
	*/
	private void Update ()
	{
		UpdateConcrete ();
	}

	protected abstract void UpdateConcrete ();
	
	public MissionObject type {
		get{ return _type;}
	}

	abstract public void AcceptVisitor (Visitor v);

	public IEnumerator Sleep2FramesAndDisable ()
	{	
		//frame 1: objects are created
		//frame 2: objects are linked
		//frame 3: some objects are disabled
		
		yield return null; //sleep over frame 1
		yield return null; //sleep over frame 2
		
		//disable
		if (spawn == MissionObjectBase.SpawnType.Start) {
				
		} else {
			gameObject.SetActive (false);
		}
		
	}
	private void OnDestroy()
	{
		Debug.Log("OnDestroy Mission object ---> " + name, gameObject);
	}
}
