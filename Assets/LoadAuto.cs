using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Main))]
public class LoadAuto : MonoBehaviour
{

	ConfigLoader loader;

	public enum ToLoad
	{
		Level,
		Menu
	}
	public bool loadAtStart = false;
	public ToLoad toLoad = ToLoad.Level;
	public string toLoadName;
	public string scene;
	
	void Start ()
	{
		loader = GetComponent<Main> ().configLoader;
		if (loader == null)
			Debug.LogError ("Failed to find ConfigLoader component");
		
		if(loadAtStart)Load();
	}
	public void Load(){
		
		if (toLoad == ToLoad.Level) {
			if(scene == ""){
				loader.LoadLevel(toLoadName);
			}
			else{
				StartCoroutine (loader.SwitchSceneAndLoad (scene, toLoadName));
			}
			
		} else if (toLoad == ToLoad.Menu) {
	
			loader.LoadMenu (toLoadName);
		}
	}


}
