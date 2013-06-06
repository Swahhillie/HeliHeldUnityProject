using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
/// <summary>
/// Game stats object.
/// </summary>
public class GameStats : System.Object{
	
	
	public enum Award
	{
		None,
		Gold,
		Silver,
		Bronze
	}
	
	public float castawayScore;
	public float timeScore;
	public float achievementScore;
	
	public float totalScore;
	public Award awardAchieved = Award.None;
	
	public int goldAwards;
	public int silverAwards;
	public int bronzeAwards;

	

	
}
public class Menu2D : MonoBehaviour {

	private List<Button2D> _buttons;
	
	public delegate void OnMouseMove(Menu2D sender, MouseClickEventArgs e);
	private event OnMouseMove MouseMoved;
	
	private Vector2 lastMousePosition;
	
	
	private GameStats gameStats;
	public KinectMouse kinectMouse;
	private Main main;
	private void Start()
	{
		
		main = FindObjectOfType(typeof(Main)) as Main;
		
		kinectMouse = ((GameObject)Instantiate(Resources.Load("kinectMousePrefab"))).GetComponent<KinectMouse>();
		RegisterButtons();
		gameStats = ScoreManager.Instance.gameStats;
		
		
	}
	private void Update()
	{
		
		if(kinectMouse.position.sqrMagnitude != lastMousePosition.sqrMagnitude) // normal != operator doesnt work here.. fix that later
		{
			MouseClickEventArgs e = new MouseClickEventArgs();
			e.position = kinectMouse.position;
			lastMousePosition = kinectMouse.position;
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
		e.Function.Invoke(this,new object[]{e});
	}
	public void DefaultButton(ButtonActivateEventArgs e)
	{
		Debug.LogWarning("Default button was called because a button does not have a properly set activate event");
	}
	public void OpenShop(ButtonActivateEventArgs e){
		Debug.Log("Opening shop");
	}
	public void CloseShop(ButtonActivateEventArgs e)
	{
		Debug.Log("Closing shop");
	}

	public void StartOver(ButtonActivateEventArgs e)
	{
		Debug.Log("Starting over");
		main.LastLevel();
	}
	public void StartNew(ButtonActivateEventArgs e)
	{
		Debug.Log("Starting new");
		main.NextLevel();
	}
	public void StopPlaying(ButtonActivateEventArgs e)
	{
		//Debug.Log("Probably shouldnt do this");
		//Application.Quit();
	}
	
	
	
}
