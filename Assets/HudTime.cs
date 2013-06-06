using UnityEngine;
using System.Collections;

public class HudTime : MonoBehaviour 
{
	private float _startTime;
	private GUIText guiText;
	
	void Start () 
	{
		_startTime = Time.time;
		guiText = this.gameObject.GetComponent<GUIText>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		int minutes = (int)Time.time / 60;
		float seconds = Time.time % 60;
		guiText.text = minutes.ToString("00") + ":" +seconds.ToString("00.00");
	}
}
