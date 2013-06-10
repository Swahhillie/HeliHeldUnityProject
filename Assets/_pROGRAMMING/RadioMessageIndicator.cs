using UnityEngine;
using System.Collections;

public class RadioMessageIndicator : MonoBehaviour {

    public float speed=1;
	
	private GUITexture _texture;
    private bool _active=true;
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
		_texture = this.gameObject.GetComponent<GUITexture>();
	}
	// Update is called once per frame
	void Update () 
	{
		if(_active)
		{
            float alpha = Mathf.Sin((Time.time-_startTime) * speed);
            _texture.color = new Color(0.5f, 0.5f, 0.5f, alpha);
		}
	}
}
