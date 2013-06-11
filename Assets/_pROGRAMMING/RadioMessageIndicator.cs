using UnityEngine;
using System.Collections;

public class RadioMessageIndicator : MonoBehaviour {

    public float speed=1;
	
	private GUITexture _texture;
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
				//makes the indicator invisible
				_texture.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
			}
		}
	}
	void Start()
	{
		_texture = this.gameObject.GetComponent<GUITexture>();
		//makes the indicator invisible
		_texture.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
	}
	// Update is called once per frame
	void Update () 
	{
		if(_active)
		{
            float alpha = Mathf.Sin((Time.time-_startTime) * speed);
            _texture.color = new Color(1.0f, 1.0f, 1.0f, alpha);
		}
	}
}
