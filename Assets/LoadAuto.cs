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
		loader = GetComponent<Main> ().configLoader;
		if (loader == null)
			Debug.LogError ("Failed to find ConfigLoader component");
		if (toLoad == ToLoad.Level) {
			if(scene == ""){
				loader.LoadLevel(toLoadName);
			}
			else{
				StartCoroutine (loader.SwitchSceneAndLoad (scene, toLoadName));
			}
			
		} else if (toLoad == ToLoad.Menu) {
>>>>>>> Message editor utility added, UT for ParseVec3
	
			loader.LoadMenu (toLoadName);
		}
	}


}
