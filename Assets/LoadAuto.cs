using UnityEngine;
using System.Collections;
[RequireComponent(typeof(ConfigLoader))]
public class LoadAuto : MonoBehaviour
{

	ConfigLoader loader;
<<<<<<< HEAD
	
	public enum ToLoad
	{
		Level,
		Menu
	}
	public ToLoad toLoad = ToLoad.Level;
	public string toLoadName;
<<<<<<< HEAD
=======
	public string levelToLoad;
>>>>>>> highlight shader
=======
	public string scene;
	
	void Start ()
	{
		loader = GetComponent<ConfigLoader> ();
		if(loader == null) Debug.LogError("Failed to find ConfigLoader component");
		if (toLoad == ToLoad.Level) {
			loader.StartCoroutine(loader.SwitchSceneAndLoad(scene, toLoadName));
		} else if (toLoad == ToLoad.Menu) {
>>>>>>> Message editor utility added, UT for ParseVec3
	
			loader.LoadMenu (toLoadName);
		}
	}

}
