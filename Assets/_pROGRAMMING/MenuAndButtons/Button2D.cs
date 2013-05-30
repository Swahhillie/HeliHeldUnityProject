using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUITexture))]
public class Button2D : MonoBehaviour {

	
	private bool isMouseIn = false;
	private float timeSinceEnter = 0;
	public float activationTime = 2.0f;
	
	public delegate void OnButtonActivate(Button2D sender, System.EventArgs e);
	public event OnButtonActivate ButtonActivated;
	
	public void Register(Menu2D menu)
	{
		menu.MouseMoved += MouseMoveListener;
		
	}
	private void MouseMoveListener(Menu2D sender, MouseClickEventArgs e)
	{
		if(guiTexture.GetScreenRect().Contains(e.position)){
			isMouseIn = true;
			timeSinceEnter += Time.deltaTime;
		}
		else{
			isMouseIn = false;
			timeSinceEnter = 0;
		}
		
		if(isMouseIn)
		{
			if(timeSinceEnter > activationTime)
			{
				ActivateButton();
			}
		}
	}
	private void ActivateButton()
	{
		ButtonActivated(this, new System.EventArgs());
	}
}
