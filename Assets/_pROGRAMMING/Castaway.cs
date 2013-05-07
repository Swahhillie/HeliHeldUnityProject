using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Castaway : MissionObjectBase
{
	override protected void AwakeConcrete(){
		_type = MissionObject.Castaway;
	}
	override protected void UpdateConcrete(){
		//update for mission object base must be implemented here. not in Update()	
	}
	override public void OnTriggered(EventReaction evr, TriggerType triggerType){
		
	}
}
