using UnityEngine;
using System.Collections;

public class Animator : TriggeredObject {

	void Start()
	{
		Animation anim = this.gameObject.GetComponent<Animation>();
		anim.playAutomatically = false;
	}
	override public void OnTriggered(EventReaction evr)
	{
		if(evr.type==EventReaction.Type.Animate)
		{
			if(!animation.Play(evr.messageName))
			{
				Debug.LogError("No "+evr.messageName+"-animation for this Object",this);
			}
		}
	}
}
