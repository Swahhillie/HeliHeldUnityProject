using UnityEngine;
using System.Collections;

public class MainPlaceholder : Main {
	public void Awake(){
		name = "Main Placeholder";
		Destroy(this.gameObject);
	}
	
	public override void OnTriggered (EventReaction eventReaction)
	{
		throw new System.NotImplementedException ();
	}
}
