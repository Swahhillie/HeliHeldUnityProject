using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Menu2D : MonoBehaviour {

	private List<Button2D> _buttons;
	
	public delegate void OnMouseMove(Menu2D sender, MouseClickEventArgs e);
	public event OnMouseMove MouseMoved;
	
	private Vector2 lastMousePosition;
	
	private void Update()
	{
		
		if(Input.mousePosition.sqrMagnitude != lastMousePosition.sqrMagnitude) // normal != operator doesnt work here.. fix that later
		{
			MouseClickEventArgs e = new MouseClickEventArgs();
			e.position = Input.mousePosition;
			lastMousePosition = Input.mousePosition;
			MouseMoved(this, e);
		}
		
	}
	public void RegisterButtons()
	{
		Debug.Log("Finding all active button2Ds");
		//find all buttons
		Button2D[] buttons = Object.FindObjectsOfType(typeof(Button2D)) as Button2D[];
		
		//add the found buttons to the list of buttons
		Debug.Log(string.Format("Found {0} buttons to add to menu", buttons.Length));
		_buttons.AddRange(buttons);
		
		Debug.Log("Making buttons register themself");
		//registere buttons. so they can listen to the events
		//_buttons.ForEach(x=>x.Register(this));	
		foreach(Button2D b in _buttons)
		{
			b.Register(this);
			b.ButtonActivated += ButtonActivateListener;
		}
		
	}
	public void ButtonActivateListener(Button2D sender, System.EventArgs e)
	{
		Debug.Log("Button was activated ", sender);
	}
	
}
