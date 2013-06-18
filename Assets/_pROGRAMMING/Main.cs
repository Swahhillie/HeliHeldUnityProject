using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Main : TriggeredObject {
	public string masterScene = "DennisMasterScene";
	public string menuScene = "DennisMenuScene";
	private enum State{Menu, Game};
	private State state;
	// Use this for initialization
	public string gameScene = "Playtest";
	public List<string> levels;
	private int currentLevel = -1;
	
	public ScoreManager scoreManager;
	
	void Awake () 
	{
		if(FindObjectsOfType(typeof(Main)).Length >1)
		{
			Debug.Log("Found another main. deleting this");
			GameObject.Destroy(gameObject);
		}
		else{
			state = State.Game;
			DontDestroyOnLoad(this.gameObject);
			if(levels.Count == 0)Debug.LogError ("Specify Levels!");
			scoreManager = ScoreManager.Instance;
			scoreManager.main = this;
		}
		
		
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
			LoadMenu();	
			
		}
		if(eventReaction.type == EventReaction.Type.SpecialScore){
			scoreManager.AddSpecialScore(eventReaction.specialScore);
		}
	}

	public void NextLevel(){
		if(currentLevel<levels.Count)
		{
			currentLevel++;
		}
		StartCoroutine(SwitchSceneAndLoad(gameScene, levels[currentLevel]));
	}
	public void LastLevel(){
		StartCoroutine(SwitchSceneAndLoad(gameScene, levels[currentLevel]));
	}
		
	public IEnumerator SwitchSceneAndLoad(string scene, string level){
		
		Application.LoadLevel(scene);
		yield return null; //wait a frame so the scene can load
		ConfigLoader.Instance.LoadLevel (level); //load the objects
		state = State.Game;
	}
	
	public void ExitToMainMenu()
	{
		Application.LoadLevel(menuScene);
	}
	public void ReturnToMaster()
	{
		currentLevel = -1;
		
		if(Application.loadedLevelName != masterScene)
			Application.LoadLevel(masterScene);
	}
	/// <summary>
	/// Loads the menu.
	/// </summary>
	public void LoadMenu()
	{
		state = State.Menu;
		scoreManager.LevelComplete(ConfigLoader.Instance.activeLevel);
		Application.LoadLevel(menuScene);
	}
	/// <summary>
	/// Loads the first level.
	/// </summary>
	public void LoadFirstLevel()
	{
		currentLevel = 0;
		LastLevel();
	}
}
