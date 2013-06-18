using UnityEngine;
using System.Collections;

public class Point : MonoBehaviour 
{

	public float timeOnScreen;
	public int maxFontSize;
	
	private bool _active=false;
	private float startTime;
	private TextMesh tmesh;
	private int _points;
	
	public int points
	{
		set
		{
			_points = value;
			tmesh.text = _points.ToString()+" punten!";
			_active=true;
			startTime = Time.time;
		}
	}
	
	void Awake ()
	{
		tmesh = GetComponentInChildren<TextMesh>();
	}
	
	
	void Update () 
	{
		if(_active)
		{
			float elapsedTime = Time.time-startTime;
			float percentage = elapsedTime/timeOnScreen;
			tmesh.fontSize = (int)(maxFontSize* percentage);
			if(elapsedTime>timeOnScreen)
			{
				Destroy(this.gameObject);	
			}
		}
	}
}
