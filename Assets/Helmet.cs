using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class HelmetAnimation : System.Object
{
	public Vector2 position;
	public Vector2 size;
	public MovieTexture _image;
	public float activateDuration=1; 
	
	
	private bool _active=false;
	private State _state = State.Deactive;
	private float _time;
	private float _scale;
	private Vector2 _startScale=new Vector2(0,0);
	private Vector2 _curScale = new Vector2(0,0);
	
	public enum State{
		Active,
		Deactive,
		Activate,
		Deactivate
	};
	
	public State state{
		get{return _state;}
	}
	
	public void setActive(bool _active)
	{
		if(_active)
		{
			_state = State.Activate;
			_time=0;
		}
		else
		{
			_time=0;
			_state = State.Deactivate;
		}
	}
	
	public Texture image
	{
		get{
			return _image;
		}	
	}
	
	public Vector2 getScale{
		get{return (_curScale);}
	}

	public void Update()
	{
		switch(_state)
		{
			case State.Activate:
			{
				_time+=Time.deltaTime;
				float step = _time/activateDuration;
				if(step>1)
				{
					step=1;
					_state=State.Active;
					if(_image!=null)
					{
						_image.loop=true;
						_image.Play();
					}
				}
				_curScale = Vector2.Lerp(_curScale,size,step);
				break;
			}	
			case State.Deactivate:
			{
				if(_image.isPlaying)
				{
					_image.Stop();
				}
				_time+=Time.deltaTime;
				float step = _time/activateDuration;
				if(step>1)
				{
					step=1;
					_state=State.Active;
				}
				_curScale = Vector2.Lerp(_curScale,_startScale,step);
				break;
			}
		}
	}
}

[System.Serializable]
public class HelmetText :System.Object
{
	public Vector2 position;
	public Vector2 size;
	public string text;
	public float activateDuration=1;
	public float writeDuration=1;
	
	
	private bool _active=false;
	private float _startTime;
	private float _scale;
	private Vector2 _startScale=new Vector2(0,0);
	private Vector2 _curScale = new Vector2(0,0);
	private float messagePercent;

	
	public bool state
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
	
	public Vector2 getScale{
		get{return (_curScale);}
	}

	public void Update()
	{
		float elapsed = Time.time - _startTime;
		float percent = elapsed / activateDuration;
		
		if(_active)
		{
			_curScale = Vector2.Lerp(_curScale, size, percent);
		}
		else
		{
			_curScale = Vector2.Lerp(_curScale, _startScale, percent);
		}
		
		float elapsedSinceOpen = Time.time - _startTime - activateDuration;
		messagePercent = Mathf.Clamp (elapsedSinceOpen / writeDuration, 0, 1);
	}
}


public class Helmet : TriggeredObject 
{
	public HelmetAnimation[] HelmetAnimations;
	//public HelmetText[] Text;
	
	void Start()
	{
		for(int i=0;i<HelmetAnimations.Length;++i)
		{
			HelmetAnimations[i].setActive(true);
		}
	}
		
	void OnGUI()
	{
		for(int i=0;i<HelmetAnimations.Length;++i)
		{
			if(HelmetAnimations[i].state!=HelmetAnimation.State.Deactive)
			{
				GUI.Label(new Rect(HelmetAnimations[i].position.x,HelmetAnimations[i].position.y,HelmetAnimations[i].getScale.x,HelmetAnimations[i].getScale.y),new GUIContent(HelmetAnimations[i].image));
			}
		}
	}
	

	
	public override void OnTriggered (EventReaction eventReaction)
	{
		if(eventReaction.type==EventReaction.Type.Animate)
		{
			
		}
	}
	
	public void Update()
	{
		for(int i=0;i<HelmetAnimations.Length;++i)
		{
			HelmetAnimations[i].Update();
		}
	}
}
