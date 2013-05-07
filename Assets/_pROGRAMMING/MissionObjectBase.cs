using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class MissionObjectBase : MonoBehaviour, ITriggeredObject {


	protected float maxlifetime=0;
	protected float lifetime=0;
	protected bool spawned=false;
	protected bool saved=false;
	protected MissionObject _type;
	//protected Dictionary<string,Reaction> evt = new Dictionary<string,Reaction>();
	//protected Dictionary<int,List<string>> action = new Dictionary<int, List<string>>();
	
	private void Awake(){
		_type = MissionObject.None;
		AwakeConcrete();
	}	
	protected abstract void AwakeConcrete();
	public virtual void SetVariables( int spawn, float aLifetime)
	{

		maxlifetime=aLifetime;
		lifetime=maxlifetime;

	
	}


	virtual public void OnTriggered(EventReaction evr, TriggerType triggerType){
		Debug.Log(name + " trigger was activated by " + triggerType + " with event " + evr.type);
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
	private void Update(){
		UpdateConcrete();
	}
	protected abstract void UpdateConcrete();
	
	public MissionObject type{
		get{return _type;}
	}
	
}
