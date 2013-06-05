using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class ScoreManager : System.Object{

	private static ScoreManager instance;
	private Level activeLevel = null;
	
	private int levelScore = 0;
	
	public GameStats gameStats = new GameStats();
	
	public static ScoreManager Instance{
		
		get{
			if(instance == null){
				instance = new ScoreManager();
			}
			return instance;
			
		}
	}
	
	private ScoreManager(){
		//attach load events to configloader
		Debug.Log("Added listeners to ConfigLoader");
		ConfigLoader.Instance.UnloadingLevel += UnbindEvents;
		ConfigLoader.Instance.LoadedLevel += BindEvents;
	}
	private void UnbindEvents(Level level){
		level.SavedCastaway -= AddScore;
		gameStats.timeScore = level.TimeTaken;
		activeLevel = null;
	}
	private void BindEvents(Level level){
		if(activeLevel != null)Debug.LogError("Active level must be null");
		activeLevel = level;
		level.SavedCastaway += AddScore;
	}
	private void AddScore(Level level, Castaway castaway){
		Debug.Log("Player scored "+ castaway + " points");
		gameStats.castawayScore += castaway.scoreValue;
	}
}