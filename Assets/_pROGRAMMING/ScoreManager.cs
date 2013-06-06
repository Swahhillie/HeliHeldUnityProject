using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ScoreManager : System.Object
{

	private static ScoreManager instance;
	private Level activeLevel = null;
	private int levelScore = 0;
	public GameStats gameStats = new GameStats ();
	
	public static ScoreManager Instance {
		
		get {
			if (instance == null) {
				instance = new ScoreManager ();
			}
			return instance;
			
		}
	}
	
	private ScoreManager ()
	{
		//attach load events to configloader
		Debug.Log ("Added listeners to ConfigLoader");
		ConfigLoader.Instance.UnloadingLevel += UnbindEvents;
		ConfigLoader.Instance.LoadedLevel += BindEvents;
	}
	/// <summary>
	/// Adds the special score.
	/// </summary>
	/// <param name='amount'>
	/// The Amount to be added.
	/// </param>
	private void AddSpecialScore(int amount){
		gameStats.achievementScore += amount;
	}

	private void UnbindEvents (Level level)
	{
		level.SavedCastaway -= AddScore;
		gameStats.timeScore = Time.time - level.levelLoadTime;
		
		//find out what award the player scored
		if (levelScore > level.bronzeAchievementScore) {
			if (levelScore > level.silverAchievementScore) {
				if (levelScore > level.goldAchievementScore) {
					gameStats.awardAchieved = GameStats.Award.Gold;
				} else
					gameStats.awardAchieved = GameStats.Award.Silver;
			} else
				gameStats.awardAchieved = GameStats.Award.Bronze;
		} else
			gameStats.awardAchieved = GameStats.Award.None;
		
		Debug.Log (string.Format (
@"Level Report:
Time taken = {0}
Castaway Score = {1}
Award achieved = {2}",
		new object[]{gameStats.timeScore, gameStats.castawayScore, gameStats.awardAchieved}));
		
		activeLevel = null;
	}

	private void BindEvents (Level level)
	{
		if (activeLevel != null)
			Debug.LogError ("Active level must be null");
		activeLevel = level;
		level.SavedCastaway += AddScore;
	}

	private void AddScore (Level level, Castaway castaway)
	{
		Debug.Log ("Player scored " + castaway + " points");
		gameStats.castawayScore += castaway.scoreValue;
	}
}
