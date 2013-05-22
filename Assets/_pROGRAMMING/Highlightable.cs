using UnityEngine; 
using System.Collections;
 
public class Highlightable : TriggeredObject
{ 
  
	private Shader originalShader; 
	//private bool active; 
  
	void Awake ()
	{ 
		originalShader = this.gameObject.renderer.material.shader; 
	}
  
	public override void OnTriggered (EventReaction eventReaction)
	{ 
		if (eventReaction.type == EventReaction.Type.Highlight_Activate) { 
			renderer.material.shader = Shader.Find ("Rimlight"); 
		} 
		if (eventReaction.type == EventReaction.Type.Highlight_Deactivate) { 
			renderer.material.shader = originalShader; 
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