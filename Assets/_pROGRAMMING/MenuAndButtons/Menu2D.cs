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
	public int savedCastaways;
	
	public int castawayScore;
	public int timeScore;
	public int specialScore;
	
	public int totalScore;
	public Award awardAchieved = Award.None;
	
	public int goldAwards;
	public int silverAwards;
	public int bronzeAwards;

	public void AddAward(Award award)
	{
		switch(award)
		{
		case Award.Gold: goldAwards++; break;
		case Award.Silver: silverAwards++;break;
		case Award.Bronze: bronzeAwards++;break;
		}
	}

	
}
public class Menu2D : MonoBehaviour {

	private List<Button2D> _buttons;
	
	public delegate void OnMouseMove(Menu2D sender, MouseClickEventArgs e);
	private event OnMouseMove MouseMoved;
	
	private Vector2 lastMousePosition;
		
	private bool popupOn = false;
	private GameStats gameStats;
	public KinectMouse kinectMouse;
	private Main main;
	public GameObject stopMenu;
	public GameObject instructionsObject;
	public GameObject totalScoreObject;
	public float instructionsUpDuration = 20.0f;
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
	
	public void StartOver(ButtonActivateEventArgs e)
	{
		Debug.Log("Starting over");
		if(popupOn) return;
		main.LastLevel();
	}
	public void StartNew(ButtonActivateEventArgs e)
	{
		Debug.Log("Starting new");
		if(popupOn) return;
		main.NextLevel();
	}
	public void StopPlaying(ButtonActivateEventArgs e)
	{
		//return to masterMenu
		main.ReturnToMaster();
		//Debug.Log("Probably shouldnt do this");
		//Application.Quit();
	}
	/// <summary>
	/// Opens the menu.
	/// </summary>
	/// <param name='e'>
	/// E.
	/// </param>
	public void OpenMenu(ButtonActivateEventArgs e)
	{
		//go to the main menu, used from the master scene
		main.LoadMenu();
	}
	/// <summary>
	/// Loads the first mission.
	/// </summary>
	/// <param name='e'>
	/// E.
	/// </param>
	public void LoadFirstMission(ButtonActivateEventArgs e)
	{
		main.LoadFirstLevel();
	}
	/// <summary>
	/// Opens the stop menu.
	/// </summary>
	/// <param name='e'>
	/// E.
	/// </param>
	public void OpenStopMenu(ButtonActivateEventArgs e)
	{
		//stopMenu.SetActive(true);	
		stopMenu.transform.position = new Vector3(.5f, 0.5f, 5.0f);
		popupOn = true;
	}
	/// <summary>
	/// Closes the stop menu.
	/// </summary>
	/// <param name='e'>
	/// E.
	/// </param>
	public void CloseStopMenu(ButtonActivateEventArgs e)
	{
		//stopMenu.SetActive(false);
		popupOn = false;
		stopMenu.transform.position = new Vector3(-500.0f, 0, 5.0f);
	}
	bool instructionsOpen = false;
	public void OpenInstruction(ButtonActivateEventArgs e)
	{
		instructionsOpen = true;

		instructionsObject.transform.position = new Vector3(0.5f, 1.0f, 1.0f);

		StartCoroutine(WaitAndExecute(() => CloseInstructions(null), instructionsUpDuration));
	 	
	}
	public void CloseInstructions(ButtonActivateEventArgs e)
	{
		instructionsOpen = false;
		instructionsObject.transform.position = new Vector3(-500.0f, 0, 1.0f);
	}
	
	private IEnumerator WaitAndExecute(System.Action action, float seconds){
		yield return new WaitForSeconds(seconds);
		action();
	}
	public void OpenTotalScore(ButtonActivateEventArgs e)
	{
		totalScoreObject.transform.position = new Vector3(0.5f, 0.5f, 1.0f);
		StartCoroutine(WaitAndExecute(() => HideTotalScore(null), 10.0f));
	}
	public void HideTotalScore(ButtonActivateEventArgs e)
	{
		totalScoreObject.transform.position = new Vector3(-10.0f, .5f, 1.0f);
	}
}
