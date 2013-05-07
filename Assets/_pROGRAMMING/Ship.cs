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

}
