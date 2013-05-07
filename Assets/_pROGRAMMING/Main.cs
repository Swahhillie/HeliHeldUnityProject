using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
	
	private string _loadName = "MainMenu";
	private enum STATE{MENU,GAME};
	private int state;
	// Use this for initialization

	void Awake () 
	{
		DontDestroyOnLoad(this.gameObject);

	}
	
	public string loadName
	{
		get{return _loadName;}
		set{_loadName = value;}
	}
	
}
