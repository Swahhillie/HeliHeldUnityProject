using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
	
	private string _loadName = "MainMenu";
	private enum STATE{MENU,GAME};
	private int state;
	// Use this for initialization

	void Awake () 
	{
		if(FindObjectsOfType(typeof(Main)).Length > 1)
		{
			Debug.Log("Found another main. deleting this");
			GameObject.Destroy(gameObject);
		}
		else{
			DontDestroyOnLoad(this.gameObject);
		}
		

	}
	
	public string loadName
	{
		get{return _loadName;}
		set{_loadName = value;}
	}
	
}
