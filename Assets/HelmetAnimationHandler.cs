using UnityEngine;
using System.Collections;
using System.Collections.Generic;



[System.Serializable]
public class HelmetAnimation : System.Object
{
	public GestureAction.GestureType gesture; // listenerfunction kinect
	public MovieTexture _image;
	

	
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
	
	
	
	/// <summary>
	/// Gets reference to GUITexture and sets the current scale to closed scale.
	/// </summary>
	void Start()
	{	
		kinectController = (ControlKinect)FindObjectOfType(typeof(ControlKinect));
		if(kinectController!=null)
		{
			kinectController.Gestures.ForEach((obj) => obj.BecomeActived += AlertGesture);
		}
		_animation = this.gameObject.GetOrAddComponent<GUITexture>();
		_currentScale = closedScale;
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
		if(activate)
		{
			_startTime = Time.time;
			_activeAnimation = animations[animNr];
			_activeAnimation.image.loop=true;
			_activeAnimation.image.Play();
			_animation.texture = _activeAnimation.image;
			//add kinect gesture listener
			
			_active=true;
		}
		else
		{
			_active = false;	
		}
	}
	
	private void AlertGesture(GestureAction gesture, System.EventArgs e)
	{
		Debug.Log("made a " + gesture.Type + " gesture " );
		if(gesture.Type == _activeAnimation.gesture)
		{
			setAnimation(false,0);	
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
		_animation.pixelInset = new Rect(59,398,_currentScale.x,_currentScale.y);
	}
	
}

