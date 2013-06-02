using UnityEngine;
using System.Collections;

public class Main : TriggeredObject {
	
	private string _loadName = "MainMenu";
	private enum State{Menu, Game};
	private State state;
	private ConfigLoader _configLoader;// = new ConfigLoader();
	// Use this for initialization

	void Awake () 
	{
		if(FindObjectsOfType(typeof(Main)).Length > 1)
		{
			Debug.Log("Found another main. deleting this");
			GameObject.Destroy(gameObject);
		}
		else{
			state = State.Game;
			configLoader = new ConfigLoader();
			DontDestroyOnLoad(this.gameObject);
		}
		

	}
	public override void OnTriggered (EventReaction eventReaction)
	{
		if(eventReaction.type == EventReaction.Type.EndLevel)
		{
			Application.LoadLevel("MenuScene");	
		}
	}
	public string loadName
	{
		get{return _loadName;}
		set{_loadName = value;}
	}
	public ConfigLoader configLoader{
		get{return _configLoader;}
		private set{_configLoader = value;}
	}
}
