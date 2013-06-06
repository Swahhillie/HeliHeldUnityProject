using UnityEngine;
using System.Collections;
[RequireComponent(typeof(GUITexture))]
public class AwardTexture : MonoBehaviour {
	
	public Texture2D noAward;
	public Texture2D goldAward;
	public Texture2D silverAward;
	public Texture2D bronzeAward;
	
	private void Start(){
		switch(ScoreManager.Instance.gameStats.awardAchieved){
			case GameStats.Award.None:
				guiTexture.texture = null;
				break;
			case GameStats.Award.Gold:
				guiTexture.texture = goldAward;
				break;
			case GameStats.Award.Silver:
				guiTexture.texture = silverAward;
				break;
			case GameStats.Award.Bronze:
				guiTexture.texture = bronzeAward;
				break;
		}
	}
}
