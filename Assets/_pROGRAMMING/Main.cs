using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Main : TriggeredObject {
	
	public string menuScene = "MenuSceneDavid";
	private enum State{Menu, Game};
	private State state;
	// Use this for initialization
	public string gameScene = "LevelDesignDaniel";
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
			state = State.Menu;
			scoreManager.LevelComplete(ConfigLoader.Instance.activeLevel);
			Application.LoadLevel(menuScene);	
			
		}
		if(eventReaction.type == EventReaction.Type.SpecialScore){
			scoreManager.AddSpecialScore(eventReaction.specialScore);
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
		ConfigLoader.Instance.LoadLevel (level); //load the objects
		state = State.Game;
	}
	
	public void ExitToMainMenu()
	{
		currentLevel = -1;
		
		if(Application.loadedLevelName != menuScene)
			Application.LoadLevel(menuScene);
	}
}
