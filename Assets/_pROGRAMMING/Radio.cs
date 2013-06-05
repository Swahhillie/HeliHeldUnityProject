using UnityEngine;
using System.Collections;

public class Radio : TriggeredObject  
{
	public Texture2D backgroundImage;
	public float closeDuration=1.0f;
	public float writeDuration = 5.0f;
	
	public Vector2 openedScale = new Vector2(200,200);
	public Vector2 closedScale = new Vector2(0,0);
	public Vector4 textOffset = new Vector4(0,0,0,0);
	
	private float _width;	
	private Message _message;
	private Message _tempMessage;
	private float messagePercent;
	private bool _active=false;
    //private RadioMessageIndicator rmi;
	private AudioSource _audioSource;
	
	private float startTime;
	
	private Vector2 _currentScale=new Vector2(0,0);
    
	

	
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
        //rmi = this.transform.parent.parent.GetComponentInChildren<RadioMessageIndicator>();
		_audioSource = Camera.main.GetComponent<AudioSource>();
	}
	
	/// <summary>
	/// The Update-function will call the DrawRadio function
	/// </summary>
	public void Update()
	{
		DrawRadio();
	}
	
	public void OnGUI()
	{
		if(_currentScale.x>0)
		{
			Vector4 radioscreen = new Vector4(Screen.width/2-_currentScale.x/2,Screen.height/2-_currentScale.y/2,_currentScale.x,_currentScale.y);
			
			GUI.Label(
				new Rect(radioscreen.x,radioscreen.y,radioscreen.z,radioscreen.w),
				new GUIContent(backgroundImage));
			
			if(messagePercent>0)
			{
				string text = "asdjfhaksdgajk\nakjgskjhagjksdhgas\\njagsdhasdfjagsjdhfgasdf\\njhasdfkga";
				Vector4 textfield = radioscreen-textOffset;
				GUI.Label(
					new Rect(textfield.x,textfield.y,textfield.z,textfield.w),
					new GUIContent(text.Substring(0,Mathf.FloorToInt(messagePercent * text.Length))));
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
			//rmi.setActive=false;
			/*if(_message.audio!=null&&!_audioSource.isPlaying)
			{
				if(_message.audio!=null)
				{
					_audioSource.PlayOneShot(_message.audio);
				}
			}*/
			
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
		//if(_message!=null)
	//	{
			float elapsed = Time.time - startTime;
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
			//Debug.Log (string.Format ("{0},{1},{2}", elapsed, elapsedSinceOpen, messagePercent));
			//tMesh.text = _message.text.Substring(0,Mathf.FloorToInt(messagePercent * _message.text.Length));
		//}
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
			if(!_message.isWarning)
			{
			
				if(message.text!=null)
				{
					SetRadio(true);
				}
			}
			else
			{
				_tempMessage = message;
				//rmi.setActive=true;
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
	/*private IEnumerator ActivateHud()
	{
		_active=true;
		for(int i=(int)(this.transform.localScale.x*step);i<=step;++i)
		{
			this.transform.localScale=new Vector3((float)i/step,this.transform.localScale.y,0.0f);
			yield return new WaitForSeconds(delay);
		}
		for(int i=(int)(this.transform.localScale.y*step);i<=step;++i)
		{
			this.transform.localScale=new Vector3(this.transform.localScale.x,(float)i/step,0.0f);
			yield return new WaitForSeconds(delay);
		}
		StartCoroutine("DisplayText");
	}
	
	private IEnumerator DisplayText()
	{
		TextMesh tMesh = this.GetComponent<TextMesh>();
		tMesh.font.material.color=Color.green;
		tMesh.text="";
		for(int i = 0; i < _message.Length; i++)
		{
			tMesh.text+= _message[i];
			yield return new WaitForSeconds(delay);
		}
	}
	
	private IEnumerator DeactivateHud()
	{
		_active=false;
		TextMesh tMesh = this.GetComponent<TextMesh>();
		tMesh.text="";
		for(int i=(int)(this.transform.localScale.y*step);i>0;--i)
		{
			this.transform.localScale=new Vector3(this.transform.localScale.x,(float)i/10,0);
			yield return new WaitForSeconds(delay);
		}	
		for(int i=(int)(this.transform.localScale.x*step);i>=0;--i)
		{
			this.transform.localScale=new Vector3((float)i/step,this.transform.localScale.y,0.0f);
			yield return new WaitForSeconds(delay);
		}
				
	}*/
}