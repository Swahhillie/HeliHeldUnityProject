using UnityEngine;
using System.Collections;

public class HudScore : MonoBehaviour 
{
	public GUIText Score;
	public GUIText CastawayCounter;

	public GUITexture bronze;
	public GUITexture silver;
	public GUITexture gold;
		
	// Use this for initialization
	void Start () 
	{
		bronze.enabled = false;
		silver.enabled = false;
		gold.enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		GameStats _gamestats = ScoreManager.Instance.gameStats;
		Score.text = _gamestats.castawayScore.ToString();
		CastawayCounter.text = _gamestats.savedCastaways.ToString();
		GameStats.Award award = ScoreManager.Instance.MedalCalculate();
		
		if(GameStats.Award.None != award)
		{
			bronze.enabled=true;
			if(GameStats.Award.Bronze != award)
			{
				silver.enabled=true;
				if(GameStats.Award.Silver != award)
				{
					gold.enabled=true;
				}
			}
		}
	}
}
