using UnityEngine;
using System.Collections;
using System.Collections.Generic;



[System.Serializable]
public class HelmetAnimation : System.Object
{
	public string functioncall; // listenerfunction kinect
	public MovieTexture _image;
	

	
	public MovieTexture image
	{
		get{return _image;}	
	}
}

[System.Serializable]
public class Helmet : TriggeredObject 
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
	private State _state = State.Deactive;
	private Vector2 _currentScale = new Vector2(0,0);
	
	
	
	/// <summary>
	/// Gets reference to GUITexture and sets the current scale to closed scale.
	/// </summary>
	void Start()
	{	
		_animation = this.gameObject.GetOrAddComponent<GUITexture>();
		_currentScale = closedScale;
		//setAnimation(true,1);
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
			animations[animNr].image.loop=true;
			animations[animNr].image.Play();
			_animation.texture = animations[animNr].image;
			//add kinect gesture listener
			
			_active=true;
		}
		else
		{
			_active = false;	
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

