using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreManager {

	private static ScoreManager instance;
	
	public ScoreManager Instance{
		get{
			if(instance == null){
				instance = new ScoreManager();
			}
			return instance;
			
		}
	}
}
