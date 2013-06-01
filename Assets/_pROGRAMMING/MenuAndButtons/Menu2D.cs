using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Menu2D : MonoBehaviour {

	private List<Button2D> _buttons;
	
	public delegate void OnMouseMove(Menu2D sender, MouseClickEventArgs e);
	private event OnMouseMove MouseMoved;
	
	private Vector2 lastMousePosition;
	
	private void Start()
	{
		
		RegisterButtons();
		
	}
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
	private void RegisterButtons()
	{
		Debug.Log("Finding all active button2Ds");
		//find all buttons
		Button2D[] buttons = Object.FindObjectsOfType(typeof(Button2D)) as Button2D[];
		
		//add the found buttons to the list of buttons
		Debug.Log(string.Format("Found {0} buttons to add to menu", buttons.Length));
		
		_buttons = new List<Button2D>(buttons);
		
		Debug.Log("Making buttons register themself");
		//registere buttons. so they can listen to the events
		//_buttons.ForEach(x=>x.Register(this));	
		foreach(Button2D b in _buttons)
		{
			b.ButtonActivated += ButtonActivateListener;
			this.MouseMoved += b.MouseMoveListener;
		}
		
	}
	private void ButtonActivateListener(Button2D sender, ButtonActivateEventArgs e)
	{
		Debug.Log("Button was activated ", sender);
		e.Function.Invoke(this, null);
	}
	public void DefaultButton()
	{
		Debug.LogWarning("Default button was called because a button does not have a properly set activate event");
	}
	public void OpenShop(){
		Debug.Log("Opening shop");
	}
	public void CloseShop()
	{
		Debug.Log("Closing shop");
	}
	public void BeginGame()
	{
		Debug.Log("Beginning game");
	}
	public void StartOver()
	{
		Debug.Log("Starting over");
	}
	public void StartNew()
	{
		Debug.Log("Starting new");
	}
	
	
}
