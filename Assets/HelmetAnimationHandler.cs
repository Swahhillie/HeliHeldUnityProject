using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Helmet animation.
/// </summary>
[System.Serializable]
public class HelmetAnimation : System.Object
{
	public GestureAction.GestureType gesture; // listenerfunction kinect
	public MovieTexture _image;
	public bool loopAnimation=false;

	
	public MovieTexture image
	{
		get{return _image;}	
	}
}

[System.Serializable]
public class HelmetAnimationHandler : TriggeredObject 
{
	public enum State{
		Active,
		Deactive,
		Activate,
		Deactivate
	};
	
	
	
	public HelmetAnimation[] animations;

	public float openCloseDuration;
	
	public Vector2 openedScale;
	public Vector2 closedScale;
	
	private bool _active=false;
	private float _startTime;
	private GUITexture _animation;
	private ControlKinect kinectController;
	private HelmetAnimation _activeAnimation;
	private State _state = State.Deactive;
	private Vector2 _currentScale = new Vector2(0,0);
	private Vector2 _position;
	public KeyCode hideAniKey = KeyCode.Alpha3;
	public float stopTime=10;
	
	
	/// <summary>
	/// Gets reference to GUITexture and sets the current scale to closed scale.
	/// </summary>
	void Start()
	{
		kinectController = (ControlKinect)FindObjectOfType(typeof(ControlKinect));
		if(kinectController.enabled)
		{
			kinectController.Gestures.ForEach((obj) => obj.BecomeActived += AlertGesture);
		}
		else
		{
			Debug.LogError("Failed to load kinectController",this);	
		}

		_animation = this.gameObject.GetOrAddComponent<GUITexture>();
		_currentScale = closedScale;
		
		//setAnimation(true,0);
		
		_position.x = _animation.pixelInset.x;
		_position.y = _animation.pixelInset.y;		
		
	}
	/// <summary>
	/// Raises the triggered event.
	/// </summary>
	/// <param name='eventReaction'>
	/// Event reaction.
	/// </param>
	public override void OnTriggered (EventReaction eventReaction)
	{
		if(eventReaction.type==EventReaction.Type.Animate)
		{
			setAnimation(true,System.Array.FindIndex<HelmetAnimation>(animations, (obj) => obj.image.name == eventReaction.messageName));
		}
	}
	
	/// <summary>
	/// Gets the animation from a List of animations
	/// </summary>
	/// <param name='activate'>
	/// Used in the Update to open the window
	/// </param>
	/// <param name='animNr'>
	/// Index of the animation in the array
	/// </param>
	
	public void setAnimation(bool activate,int animNr)
	{
		_startTime = Time.time;
		if(activate)
		{
			_activeAnimation = animations[animNr];
			_activeAnimation.image.loop=_activeAnimation.loopAnimation;
			_activeAnimation.image.Play();
			_animation.texture = _activeAnimation.image;
			_active=true;
		}
		else
		{
			_active = false;	
		}
	}
	/// <summary>
	/// If a gesture is made it checks if the animation should stop
	/// </summary>
	/// <param name='gesture'>
	/// Gesture.
	/// </param>
	/// <param name='e'>
	/// E.
	/// </param>
	private void AlertGesture(GestureAction gesture, System.EventArgs e)
	{
		if(_active)
		{
			if(_activeAnimation!=null)
			{
				Debug.Log("made a " + gesture.Type + " gesture " );
				if(gesture.Type == _activeAnimation.gesture)
				{
					setAnimation(false,0);	
				}
			}
		}
	}
	
	
	/// <summary>
	/// Updates the Animationscreen in the Helmet
	/// </summary>
	public void Update()
	{
		float elapsed = Time.time - _startTime;
		float percent = elapsed / openCloseDuration;
		if(_active)
		{
			_currentScale = Vector2.Lerp(_currentScale, openedScale, percent);
		}
		else
		{
			_currentScale = Vector2.Lerp(_currentScale, closedScale, percent);
		}
		_animation.pixelInset = new Rect(_position.x,_position.y,_currentScale.x,_currentScale.y);
		
		if(elapsed>stopTime)
		{
			setAnimation(false,0);
		}
		
		if( Input.GetKeyDown(hideAniKey))
		{
			setAnimation(false, 0);
		}
	}
	
}

