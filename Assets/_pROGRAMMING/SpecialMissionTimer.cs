using UnityEngine;
using System.Collections;

public class SpecialMissionTimer : TriggeredObject {

	// Use this for initialization
	
	
	public Vector2 openedScale;
	public Vector2 closedScale;
	private float _time=0;
	public float openCloseDuration;
	
	public bool ReversedAnim = false;
	
	private Vector2 _position;
	private Vector2 _currentScale;
	private GUITexture _background;
	private GUIText _timer;
	
	private bool _active=false;
	
	private float _startTime;
		
	
	void Start()
	{
		if(ReversedAnim)
		{
			openedScale.x*=-1;
		}
		_currentScale = closedScale;
		_background = this.gameObject.GetOrAddComponent<GUITexture>();
		_timer = this.gameObject.GetOrAddComponent<GUIText>();
		_position.x = _background.pixelInset.x;
		_position.y = _background.pixelInset.y;
		_background.pixelInset = new Rect(_position.x,_position.y,closedScale.x,closedScale.y);
	}
	
	// Update is called once per frame
	void Update () 
	{
		float elapsed = Time.time - _startTime;
		float percent = elapsed / openCloseDuration;
	
		
		if(_active)
		{
			_time -=Time.deltaTime;
			if(_time<0)
			{
				_active=false;
				_timer.enabled = false;				
				_startTime = Time.time;
			}
			else if(percent>0.9f)
			{
				_timer.enabled = true;
				_timer.text = _time.ToString("00.0");
			}
			_currentScale = Vector2.Lerp(_currentScale, openedScale, percent);
		}
		else
		{
			_currentScale = Vector2.Lerp(_currentScale, closedScale, percent);
		}
		_background.pixelInset = new Rect(_position.x,_position.y,_currentScale.x,_currentScale.y);
	}
	
	public override void OnTriggered (EventReaction eventReaction)
	{
		if(eventReaction.type == EventReaction.Type.Enable)
		{
			_active =true;
			_time = eventReaction.time;
			_startTime = Time.time;
		}
		if(eventReaction.type == EventReaction.Type.Disable)
		{
			_active =false;
		}
	}
}


