using UnityEngine; 
using System.Collections;
 
public class Highlightable : TriggeredObject
{ 

	public float HighlightPower=0;
	public Color HighlightColor = Color.white;
	private Shader originalShader; 
	//private bool active; 
  
	void Awake ()
	{ 
		originalShader = this.gameObject.renderer.material.shader; 
	}
  
	public override void OnTriggered (EventReaction eventReaction)
	{ 
		if (eventReaction.type == EventReaction.Type.Highlight_Activate) 
		{ 
			renderer.material = (Material)Instantiate(renderer.material);
			renderer.material.shader = Shader.Find ("Rimlight");
			renderer.material.SetFloat("RimPower",HighlightPower);
			renderer.material.SetColor("RimColor",HighlightColor);
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