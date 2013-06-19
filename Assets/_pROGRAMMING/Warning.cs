using UnityEngine;
using System.Collections;

public class Warning : MonoBehaviour {
	
	
	public float speed=1;
	public Color textColor = Color.white;
	public Vector3 color;
	public float yPos;
	public float height;
	public float width;
	public GUIStyle style;

	private string _text;
    private bool _active=false;
	private float _startTime=0;
	public bool setActive
	{
		get{return _active;}
		set{
			_active = value;
			if(_active)
			{
				_startTime=Time.time;
			}
		}
	}
	void Start()
	{
		//setWarning(true,"test ajksdhfha kajsdhfklja skjashdfkjla kjdfahklsjd fkaj sdfkjah sdkljfhalksdjhfalkjsd fkjla sdklfjhaslkdjfhalkjsdhflkajsd kjah lskdjfakljsdh f");	
	}
	// Update is called once per frame
	void Update () 
	{
		if(_active)
		{
            float alpha = Mathf.Sin((Time.time-_startTime) * speed);
			style.normal.textColor = new Color(style.normal.textColor.r,style.normal.textColor.g,style.normal.textColor.b,alpha);
		}
	}
	
	void OnGUI()
	{
		if(_active)
		{
			GUI.Label(new Rect((Screen.width-width)/2,yPos,width,height), new GUIContent(_text),style);	
		}
	}
	
	
	public void setWarning(bool status, string text)
	{
		setActive = status;
		if(_active)
		{
			_text = text;
		}
		else
		{
			_text = "";
		}
	}
}
