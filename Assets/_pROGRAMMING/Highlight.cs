using UnityEngine;
using System.Collections;

public class Highlight : TriggeredObject {
	
	private Shader originalShader;
	//private bool active;
	
	void Awake()
	{
		originalShader = this.gameObject.renderer.material.shader;
	}
	
	public override void OnTriggered (EventReaction eventReaction)
	{
		if(eventReaction.type == EventReaction.Type.Highlight_Activate)
		{
			this.gameObject.renderer.material.shader = Shader.Find("Rimlight");
		}
		if(eventReaction.type == EventReaction.Type.Highlight_Deactivate)
		{
			this.gameObject.renderer.material.shader = originalShader;
		}
	}
	
	//not used yet
	/*private IEnumerator switchShader()
	{
		if(active)
		{
			this.gameObject.renderer.material.shader = originalShader;
		}
		else
		{
			this.gameObject.renderer.material.shader = Shader.Find("Rimlight");
		}
	}*/
}
