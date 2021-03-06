using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ship : MissionObjectBase 
{
	override protected void AwakeConcrete(){
		_type = MissionObject.Ship;
	}
	override protected void UpdateConcrete(){
		//update for mission object base must be implemented here. not in Update()	
	}
	override public void AcceptVisitor(Visitor v){
		v.Visit(this);
	}
	override public void OnTriggered(EventReaction evr){
	
		base.OnTriggered(evr);
		
		if(evr.type == EventReaction.Type.Say){
			Debug.Log("Ship says " + ConfigLoader.GetMessage(evr.messageName).text);
		}
	}
	

}
