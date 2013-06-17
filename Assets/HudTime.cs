using UnityEngine;
using System.Collections;

public class HudTime : MonoBehaviour 
{
	private GUIText _guiText;
	
	void Start () 
	{
		_guiText = this.gameObject.GetComponent<GUIText>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		int minutes = (int)Time.timeSinceLevelLoad / 60;
		float seconds = Time.timeSinceLevelLoad % 60;
		_guiText.text = minutes.ToString("00") + ":" +seconds.ToString("00.00");
	}
}
