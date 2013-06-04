using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Main : TriggeredObject {
	
	private string _loadName = "MainMenu";
	private enum State{Menu, Game};
	private State state;
	private ConfigLoader _configLoader = null;// = new ConfigLoader();
	// Use this for initialization
	public string gameScene = "LevelDesignDaniel";
	public List<string> levels;
	private int currentLevel = -1;
	
	void Awake () 
	{
		Debug.LogError("Main", this);
		if(FindObjectsOfType(typeof(Main)).Length >1)
		{
			Debug.Log("Found another main. deleting this");
			GameObject.Destroy(gameObject);
		}
		else{
			state = State.Game;
			configLoader = new ConfigLoader();
			DontDestroyOnLoad(this.gameObject);
		}
		if(levels.Count == 0)Debug.LogError ("Specify Levels!");
		
		

	}
	/// <summary>
	/// Raises the triggered event.
	/// </summary>
	/// <param name='eventReaction'>
	/// Event reaction.
	/// </param>
	public override void OnTriggered (EventReaction eventReaction)
	{
		
		if(eventReaction.type == EventReaction.Type.EndLevel)
		{
			state = State.Menu;
			Application.LoadLevel("MenuScene");	
		}
	}
	/// <summary>
	/// Gets the next level.
	/// </summary>
	/// <value>
	/// The next level.
	/// </value>
	private string nextLevel{
		get{
			if(currentLevel < levels.Count)
				return levels[currentLevel];
			else
			{
				currentLevel = 0;
				return levels[currentLevel];
			}
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
	public void NextLevel(){
		currentLevel++;
		StartCoroutine(SwitchSceneAndLoad(gameScene, levels[currentLevel]));
	}
	public void LastLevel(){
		StartCoroutine(SwitchSceneAndLoad(gameScene, levels[currentLevel]));
	}
		
	public IEnumerator SwitchSceneAndLoad(string scene, string level){
		Application.LoadLevel(scene);
		yield return null; //wait a frame so the scene can load
		configLoader.LoadLevel (level); //load the objects
		state = State.Game;
	}
}
