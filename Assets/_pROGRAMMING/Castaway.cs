using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Castaway : MissionObjectBase
{
	
	public int scoreValue = 0;
	public GameObject ScoreText;

	
	override protected void AwakeConcrete(){
		_type = MissionObject.Castaway;
		_rescuable = true;
	}
	override protected void UpdateConcrete(){
		//update for mission object base must be implemented here. not in Update()	
	}
	
	public override bool AttemptRescue ()
	{
		bool success  = base.AttemptRescue ();
		if(success)
		{
			ScoreText = (GameObject)Instantiate(Resources.Load("ScoreText"), this.transform.position, Camera.mainCamera.transform.rotation);
			if(ScoreText!=null)
			{
				ScoreText.GetComponent<Point>().points = scoreValue;
			}
		}
		return (success);
	}
	
	override public void AcceptVisitor(Visitor v){
		v.Visit(this);
	}
	override public void OnTriggered(EventReaction evr){
		base.OnTriggered(evr);
		
	}
}
