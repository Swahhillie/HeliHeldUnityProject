using UnityEngine;
using System.Collections;

public class LoadAuto : MonoBehaviour {

	ConfigLoader loader;
	public string levelToLoad;
	void Start () {
		loader = GetComponent<ConfigLoader>();
		loader.LoadLevel(levelToLoad);
	}

}
