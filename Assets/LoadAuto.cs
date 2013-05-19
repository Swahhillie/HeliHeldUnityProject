using UnityEngine;
using System.Collections;
[RequireComponent(typeof(ConfigLoader))]
public class LoadAuto : MonoBehaviour
{

	ConfigLoader loader;
	
	public enum ToLoad
	{
		Level,
		Menu
	}
	public ToLoad toLoad = ToLoad.Level;
	public string toLoadName;
	public string scene;
	
	void Start ()
	{
		loader = GetComponent<ConfigLoader> ();
		if(loader == null) Debug.LogError("Failed to find ConfigLoader component");
		if (toLoad == ToLoad.Level) {
			loader.StartCoroutine(loader.SwitchSceneAndLoad(scene, toLoadName));
		} else if (toLoad == ToLoad.Menu) {
	
			loader.LoadMenu (toLoadName);
		}
	}

}
