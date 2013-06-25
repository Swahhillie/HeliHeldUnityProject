using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

[System.Serializable]
public class LevelScene
{
	public string level;
	public string scene;
	
	
	public LevelScene(string scene, string level)
	{
		this.scene = scene;
		this.level = level;
	}
	public override string ToString ()
	{
		return string.Format("{0} + {1}", scene, level);
	}
}

public class Main : TriggeredObject {
	public string masterScene = "DennisMasterScene";
	public string menuScene = "DennisMenuScene";
	private enum State{Menu, Game};
	private State state;
	// Use this for initialization
	public string gameScene = "Playtest";
	private List<LevelScene> levels;
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
			scoreManager = ScoreManager.Instance;
			scoreManager.main = this;
			
			
			string levelsString = "";
			string scenesString = "";
			
			if(ConfigLoader.GetValue("levels", ref levelsString) && ConfigLoader.GetValue("scenes", ref scenesString))
			{
				levels = new List<LevelScene>();
				var levelsToCombine = levelsString.Split(',');
				var scenesToCombine = scenesString.Split(',');
				
				if(levelsToCombine.Length != scenesToCombine.Length) Debug.LogError("Levels do not match scenes");
				for(int i =0 ; i< levelsToCombine.Length; i++)
				{
					levels.Add(new LevelScene(levelsToCombine[i], scenesToCombine[i]));
					
				}
			}
			else
			{
				Debug.LogError("There are no levels and scenes defined in Config.xml");
			}
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
		StartCoroutine(SwitchSceneAndLoad(levels[currentLevel].scene, levels[currentLevel].level));
	}
	public void LastLevel(){
		StartCoroutine(SwitchSceneAndLoad(levels[currentLevel].scene, levels[currentLevel].level));
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
