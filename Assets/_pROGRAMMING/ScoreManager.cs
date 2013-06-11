using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Score manager singleton.
/// </summary>
[System.Serializable]
public class ScoreManager : System.Object
{

	private static ScoreManager instance;
	private Level activeLevel = null;
	public GameStats gameStats = new GameStats ();
	public Main main;
	public float scoreCountDuration = 5.0f;
	/// <summary>
	/// Gets the instance.
	/// </summary>
	/// <value>
	/// The instance.
	/// </value>
	public static ScoreManager Instance {
		
		get {
			if (instance == null)
			{
				instance = new ScoreManager ();
			}
			return instance;
			
		}
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="ScoreManager"/> class.
	/// </summary>
	private ScoreManager ()
	{
		//attach load events to configloader
		Debug.Log ("Added listeners to ConfigLoader");
		ConfigLoader.Instance.LoadedLevel += BindEvents;
	}
	/// <summary>
	/// Adds the special score.
	/// </summary>
	/// <param name='amount'>
	/// The Amount to be added.
	/// </param>
	public void AddSpecialScore (int amount)
	{
		gameStats.specialScore += amount;
	}
	
	public GameStats.Award MedalCalculate()
	{
		if(activeLevel == null)return GameStats.Award.None;

		int levelScore = gameStats.castawayScore + gameStats.specialScore;
		GameStats.Award award = GameStats.Award.None;
		if (levelScore > activeLevel.bronzeAchievementScore)
		{
			if (levelScore > activeLevel.silverAchievementScore)
			{
				if (levelScore > activeLevel.goldAchievementScore)
				{
					award = GameStats.Award.Gold;
				} 
				else
				{
					award = GameStats.Award.Silver;
				}
			} 
			else
			{
				award = GameStats.Award.Bronze;
			}
		} 
		else
		{
			award = GameStats.Award.None;
		}
		return award;
	}
	
	/// <summary>
	/// Unbinds the events.
	/// </summary>
	/// <param name='level'>
	/// Level.
	/// </param>
	public void LevelComplete (Level level)
	{
		Debug.Log("ENDED A LEVEL, CALCULATING MEDAL");
		
		int levelScore = gameStats.castawayScore + gameStats.specialScore;
		//lerping the castaway and special score fields
		main.StartCoroutine(BringFieldScoreUp((obj) => gameStats.castawayScore = obj, 0, gameStats.castawayScore));
		main.StartCoroutine(BringFieldScoreUp((obj) => gameStats.specialScore = obj, 0, gameStats.specialScore));
		level.SavedCastaway -= AddScore;
		gameStats.timeScore = Mathf.FloorToInt(Time.time - level.levelLoadTime);
		
		gameStats.awardAchieved = MedalCalculate();
		//find out what award the player scored
		
		main.StartCoroutine(BringFieldScoreUp((obj) => gameStats.totalScore = obj, gameStats.totalScore, gameStats.totalScore + levelScore));
		
		Debug.Log (string.Format (
@"Level Report:
Time taken = {0}
Castaway Score = {1}
Award achieved = {2}
Total score = {3}",
		new object[]{gameStats.timeScore, gameStats.castawayScore, gameStats.awardAchieved, gameStats.totalScore}));
		
		activeLevel = null;
	}
	/// <summary>
	/// Brings the field score up.
	/// </summary>
	/// <returns>
	/// The field score up.
	/// </returns>
	/// <param name='assignement'>
	/// Assignement action to set the target field.
	/// </param>
	/// <param name='start'>
	/// Start value.
	/// </param>
	/// <param name='goal'>
	/// Goal value.
	/// </param>
	private IEnumerator BringFieldScoreUp(System.Action<int> assignement, int start, int goal){
		Debug.Log("Bringing up a field from " + start + " to " + goal);
		float alpha = 0;
		float startTime = Time.time;
		do
		{
			
			float elapsed = Time.time - startTime;
			alpha = elapsed / scoreCountDuration;
			assignement(Mathf.RoundToInt(Mathf.Lerp(start, goal, alpha)));
			//gameStats.totalScore = Mathf.RoundToInt(Mathf.Lerp(start, goal, alpha));
			yield return null;
		}while(alpha <= 1.0f);
		assignement(goal);
		
	}
	/// <summary>
	/// Called when a new level is loaded.
	/// </summary>
	/// <param name='level'>
	/// Level that has just been loaded.
	/// </param>
	private void BindEvents (Level level)
	{
		if (activeLevel != null)
		{
			Debug.LogError ("Active level must be null");
		}
		activeLevel = level;
		level.SavedCastaway += AddScore;
		WipeLevelGameStats();
	}
	/// <summary>
	/// Wipes the level game stats.
	/// </summary>
	private void WipeLevelGameStats(){
		Debug.Log("Wiping level specific game stats");
		gameStats.castawayScore = 0;
		gameStats.savedCastaways = 0;
		gameStats.specialScore = 0;
		gameStats.awardAchieved = GameStats.Award.None;
	}
	/// <summary>
	/// Adds the score to the level game stats.
	/// </summary>
	/// <param name='level'>
	/// Level.
	/// </param>
	/// <param name='castaway'>
	/// Castaway to pull the score from.
	/// </param
	/// >
	private void AddScore (Level level, Castaway castaway)
	{
		Debug.Log ("Player scored " + castaway + " points");
		gameStats.castawayScore += castaway.scoreValue;
		gameStats.savedCastaways ++;
	}
}
