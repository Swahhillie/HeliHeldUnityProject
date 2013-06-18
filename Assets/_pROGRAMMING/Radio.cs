using UnityEngine;
using System.Collections;

public class Radio : TriggeredObject  
{
	public Font radioFont;
	public Color radioTextcolor= Color.white;
	public int radioTextSize = 10;
	public Texture2D radioBackgroundImage;
	public Texture2D warningBackgroundImage;
	public float closeDuration=1.0f;
	public float writeDuration = 1.0f;
	public float timeOnScreen;
	public Vector2 openedScale = new Vector2(200,200);
	private Vector2 closedScale = new Vector2(0,0);
	public Vector4 textOffset = new Vector4(0,0,0,0);
	
	
	private float _width;	
	private Message _message;
	private Message _tempMessage;
	private float messagePercent;
	private bool _active=false;
    private RadioMessageIndicator rmi;
	private AudioSource _audioSource;
	private float startTime;
	private Vector2 _currentScale=new Vector2(0,0);
	private GUIStyle style;
    private Vector2 position;
	private Warning warning;
	

	
	public float width
	{
		get{return _width;}
		set{_width = value;}
	}
	/// <summary>
	/// The Start-function will get references to the TextMesh (the text on scene),
	/// the RadioMessageIndicator and the audioSource from the maincamera
	/// </summary>
	void Start()
	{
		rmi = this.transform.GetComponentInChildren<RadioMessageIndicator>();
		_audioSource = Camera.main.GetComponent<AudioSource>();
		style = new GUIStyle();
		style.wordWrap=true;
		if(radioFont!=null)
		{
			style.font = radioFont;
		}
		style.normal.textColor = radioTextcolor;
		style.fontSize = radioTextSize;
		warning = this.transform.GetComponentInChildren<Warning>();
		if(timeOnScreen<closeDuration+writeDuration)
		{
			timeOnScreen=closeDuration+writeDuration;
		}
	}
	
	/// <summary>
	/// The Update-function will call the DrawRadio function
	/// </summary>
	public void Update()
	{
		DrawRadio();
	}
	
	/// <summary>
	/// Draws the Radio in GUI space if neccessary
	/// </summary>
	public void OnGUI()
	{
		if(_currentScale.x>0)
		{
			Vector4 radioscreen = new Vector4(Screen.width/2-_currentScale.x/2,Screen.height/2-_currentScale.y/2,_currentScale.x,_currentScale.y);
			
			if(_message.isWarning)
			{
				GUI.Label(
					new Rect(radioscreen.x,radioscreen.y,radioscreen.z,radioscreen.w),
					new GUIContent(warningBackgroundImage));
			}
			else
			{
				GUI.Label(
					new Rect(radioscreen.x,radioscreen.y,radioscreen.z,radioscreen.w),
					new GUIContent(radioBackgroundImage));
			}
			if(messagePercent>0)
			{
				Vector4 textfield = radioscreen-textOffset;
				GUI.Label(
					new Rect(textfield.x,textfield.y,textfield.z,textfield.w),
					new GUIContent(_message.text.Substring(0,Mathf.FloorToInt(messagePercent * _message.text.Length))),style);
			}
		}
	}
	
	/// <summary>
	/// The ToggleHud -function will change the state of the Radio from active to deactive and the other way around.
	/// </summary>
	public void ToggleHud()
	{
		startTime = Time.time;
		_active = !_active;
		StateChanged();
	}
	
	/// <summary>
	/// The SetRadio -function will change the state of the radio to a specific state (active or deactive)
	/// </summary>
	/// <param name='active'>
	/// The state the radio should have.
	/// </param>
	
	public void SetRadio(bool active)
	{
	    //Debug.LogError("should not be called");
		if(active!=_active)
		{
			startTime = Time.time;
			_active = active;
			StateChanged();
		}
	}
	
	/// <summary>
	/// The StateChanged -function handles everthing that should happen after the state of the radio changed
	/// Active:
	/// -deactivate the RadioMessageIdicator
	/// -play the sound
	/// Deactivate:
	/// -stop the sound
	/// -reset the message if the active message was a warning
	/// </summary>
	private void StateChanged()
	{
		if(_active)
		{
			if(_message==null)
			{
				_active=false;
				return;
			}
			rmi.setActive=false;
			if(_message.audio!=null)
			{
				_audioSource.PlayOneShot(_message.audio);
			}
			
		}
		else
		{
			if(_tempMessage!=null)
			{
				if(_message.isWarning)
				{
					_message = _tempMessage;	
				}
				if(_audioSource.isPlaying)
				{
					_audioSource.Stop();
				}
			}
		}
	}
	
	/// <summary>
	/// The DrawRadio -function changes the scale of the radio object and draws the text on screen.
	/// </summary>
	private void DrawRadio()
	{
		if(_message!=null)
		{
			float elapsed = Time.time - startTime;
			//deactivate if elapsed time is bigger than the time on screen!
			if(_message.audio==null)
			{
				if(elapsed>timeOnScreen)
				{
					_active=false;
					startTime = Time.time;
					elapsed=0;
				}
			}
			else
			{
				//deactivate if message audio file is done!
				if(!_audioSource.isPlaying)
				{
					_active=false;	
				}
			}
			float percent = elapsed / closeDuration;
			
			if(_active)
			{
				_currentScale = Vector3.Lerp(_currentScale, openedScale, percent);
			}
			else
			{
				_currentScale = Vector3.Lerp(_currentScale, closedScale, percent);
			}
			
			float elapsedSinceOpen = Time.time - startTime - closeDuration;
			messagePercent = Mathf.Clamp (elapsedSinceOpen / writeDuration, 0, 1);
		}
	}
	/// <summary>
	/// Overrides the OnTriggered  -function from the TriggeredObject class
	/// It takes a eventreaction and if it has the type Say it will draw use the message name to load a valid message.
	/// If the message isn't a warning it will be saved as the last missionrelated message.
	/// </summary>
	/// <param name='evr'>
	/// Evr.
	/// </param>
	override public void OnTriggered(EventReaction evr)
	{
		if(evr.type==EventReaction.Type.Say)
		{
			Message message = ConfigLoader.GetMessage(evr.messageName);
			Debug.Log("Displaying message " + message.text);
			_message = message;
			if(_message.isWarning)
			{
				if(message.text!=null)
				{
					SetRadio(true);
				}
			}
			else
			{
				_tempMessage = message;
				rmi.setActive=true;
			}
            //SetRadio(true);
		}
	}
	
	/// <summary>
	/// Gets a value indicating whether this <see cref="Radio"/> radio is active.
	/// </summary>
	/// <value>
	/// <c>true</c> if radio is active; otherwise, <c>false</c>.
	/// </value>
	
	public bool radioIsActive
	{
		get{return _active;}
	}
	
	public void Warning(bool status,string text)
	{
		warning.setWarning(status,text);
	}
}