using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Castaway : MissionObjectBase
{
	
	public int scoreValue = 0;
	
	override protected void AwakeConcrete(){
		_type = MissionObject.Castaway;
		_rescuable = true;
	}
	override protected void UpdateConcrete(){
		//update for mission object base must be implemented here. not in Update()	
	}
	
	override public void AcceptVisitor(Visitor v){
		v.Visit(this);
	}
	override public void OnTriggered(EventReaction evr){
		base.OnTriggered(evr);
		
	}
}
