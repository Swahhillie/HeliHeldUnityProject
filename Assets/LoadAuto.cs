using UnityEngine;
using System.Collections;

public class LoadAuto : MonoBehaviour {

	ConfigLoader loader;
	
	public enum ToLoad
	{
		Level,
		Menu
	}
	public ToLoad toLoad = ToLoad.Level;
	public string toLoadName;
	
	void Start () {
		loader = GetComponent<ConfigLoader>();
		if(toLoad == ToLoad.Level)
			loader.LoadLevel(toLoadName);
		else if(toLoad == ToLoad.Menu)
			loader.LoadMenu(toLoadName);
	}

}
