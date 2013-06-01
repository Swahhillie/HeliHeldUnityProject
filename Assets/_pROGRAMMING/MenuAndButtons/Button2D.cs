using UnityEngine;
using System.Collections;
using System.Reflection;

[RequireComponent(typeof(GUITexture))]
public class Button2D : MonoBehaviour
{

	
	private bool _isMouseIn = false;
	private float timeSinceEnter = 0;
	public float activationTime = 2.0f;
	
	public delegate void OnButtonActivate (Button2D sender, ButtonActivateEventArgs e);

	public event OnButtonActivate ButtonActivated;
	
	public Texture2D defaultTexture;
	public Texture2D hoverTexture;
	
	public MethodInfo activateFunction;
	
	/// <summary>
	/// Awake this instance, warn if there are unset variables.
	/// </summary>
	public void Awake()
	{
		if(defaultTexture == null)Debug.LogError("There is no default texture set");
		if(hoverTexture == null)Debug.LogError("There is no hover texture set");
		if(activateFunction == null){
			activateFunction = typeof(Menu2D).GetMethod("DefaultButton");
			Debug.LogWarning("Using default button function for " + name, this);
		} 
		//update the texture so it displays the correct one from the start.
		SwapTexture();
	}

	public void MouseMoveListener (Menu2D sender, MouseClickEventArgs e)
	{
		if (guiTexture.GetScreenRect ().Contains (e.position)) {
			IsMouseIn = true;
			
		} else {
			IsMouseIn = false;
		}
	}
	private void Update()
	{
		if (IsMouseIn) {
			timeSinceEnter += Time.deltaTime;
			if (timeSinceEnter > activationTime) {
				ActivateButton ();
				timeSinceEnter = 0;	
			}
		}
		else{
			timeSinceEnter = 0;
		}
	}
	private void ActivateButton ()
	{
		Debug.Log("Button was activated ", gameObject);
		ButtonActivateEventArgs activateEvent = new ButtonActivateEventArgs();
		activateEvent.Function = activateFunction;
		ButtonActivated (this, activateEvent);
	}
	
	private bool IsMouseIn {
		get{ return  _isMouseIn;}
		set {
			if (_isMouseIn != value) {
				_isMouseIn = value;
				SwapTexture ();
			}
		}
	}
	private void SwapTexture()
	{
		if(IsMouseIn){
			guiTexture.texture = hoverTexture;
		}
		else if(!IsMouseIn){
			guiTexture.texture = defaultTexture;
		}
		
		Rect r = new Rect(guiTexture.pixelInset.x, guiTexture.pixelInset.y, guiTexture.texture.width, guiTexture.texture.height);
		guiTexture.pixelInset = r;
	}
//	public ButtonActivateEventArgs activateEvent{
//		get{return _activateEvent;}
//		set{_activateEvent = value;}
//	}
}
