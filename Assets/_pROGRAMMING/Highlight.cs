using UnityEngine;
using System.Collections;

public class Highlight : TriggeredObject {

	public override void OnTriggered (EventReaction eventReaction)
	{
		if(eventReaction.type == EventReaction.Type.Say)
		{
			this.gameObject.renderer.material.shader = Shader.Find("Outline/Rimlight");
			
			
		}
	}
}
