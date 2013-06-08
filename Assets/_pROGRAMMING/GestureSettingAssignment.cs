using UnityEngine;
using System.Collections;

public class GestureSettingAssignment : MonoBehaviour {

	// Use this for initialization
	public GestureSettings gestureSettings = new GestureSettings();
	
	void Start () {
		GestureAction.settings = gestureSettings;
	}

}
