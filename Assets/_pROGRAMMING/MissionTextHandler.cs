using UnityEngine;
using System.Collections;

public class MissionTextHandler : MonoBehaviour
{
	TextMesh textMesh;
	
	void Start ()
	{
		textMesh = this.GetComponent<TextMesh>();
		textMesh.font.material.color = Color.green;
	}
	
	void Update ()
	{
		int totalTime = ((int)Time.timeSinceLevelLoad);
		int minutes = (int)totalTime/60;
		int seconds = (int)totalTime-(minutes*60);
		
		string minutesString = minutes.ToString();
		string secondsString = seconds.ToString();
		
		if(minutes < 10)
			minutesString = "0"+minutes.ToString();
		if(seconds < 10)
			secondsString = "0"+seconds.ToString();
			
		string timeString = minutesString+":"+secondsString;
			
			
		/*setText( Mission.getCastawaysLeft().ToString() + "      " + 
			Mission.getShipsLeft().ToString() + "\n " + 
			timeString);
			*/
	}
	
	void setText( string aText )
	{
		textMesh.text = aText;
	}
}
