using UnityEngine;
using System.Collections;

public class MainPlaceholder : TriggeredObject {
	public void Awake(){
		Destroy(this.gameObject);
	}
	
	public override void OnTriggered (EventReaction eventReaction)
	{
		throw new System.NotImplementedException ();
	}
}
