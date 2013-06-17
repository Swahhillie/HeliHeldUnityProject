using UnityEngine;
using System.Collections;

public class Warning : MonoBehaviour {
	
	
	public float speed=1;
	public Color textColor = Color.white;
	public Vector3 color;
	
	private GUITexture _texture;
	private GUIText _text;
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
			else
			{
				_texture.color = new Color(color.x, color.y, color.z, 0.0f);
			}
		}
	}
	void Start()
	{
		_texture = this.gameObject.GetComponent<GUITexture>();
		_text = this.gameObject.GetComponent<GUIText>();
		textColor.a =0;
		//makes the indicator invisible
		_text.material.color = textColor;
		_texture.color = new Color(color.x, color.y, color.z, 0.0f);
	}
	// Update is called once per frame
	void Update () 
	{
		if(_active)
		{
            float alpha = Mathf.Sin((Time.time-_startTime) * speed);
			textColor.a = alpha;
           _texture.color = new Color(color.x, color.y, color.z, alpha);
			_text.material.color = textColor;
		}
	}
	
	public void setWarning(bool status, string text)
	{
		setActive = status;
		if(_active)
		{
			_text.text = text;
		}
		else
		{
			_text.text = "";
		}
	}
}
