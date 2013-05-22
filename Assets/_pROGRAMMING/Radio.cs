using UnityEngine;
using System.Collections;

public class Radio : TriggeredObject  
{
	private float _width;	
	private string _message = "";
	private bool _active;
	private TextMesh tMesh;
	
	private float startTime;
	public float closeDuration=1.0f;
	public float writeDuration = 5.0f;
	
	public float delay = 0.01f;
	public int step = 10;
	
	public Vector3 openedScale = new Vector3(1,1,0);
	public Vector3 closedScale = new Vector3(0,0,0);
	
	public float width
	{
		get{return _width;}
		set{_width = value;}
	}
	
	void Awake()
	{
		tMesh = this.GetComponent<TextMesh>();	
	}
	
	
	public void Update()
	{
		
		float elapsed = Time.time - startTime;
		float percent = elapsed / closeDuration;
		
		if(_active)
		{
			transform.localScale = Vector3.Lerp(closedScale, openedScale, percent);
		}
		else
		{
			transform.localScale = Vector3.Lerp(openedScale, closedScale, percent);
		}
		
		float elapsedSinceOpen = Time.time - startTime - closeDuration;
		
		float messagePercent = elapsedSinceOpen / writeDuration;
		messagePercent = Mathf.Clamp (messagePercent, 0, 1);
		//Debug.Log (string.Format ("{0},{1},{2}", elapsed, elapsedSinceOpen, messagePercent));
		tMesh.text = _message.Substring(0,Mathf.FloorToInt(messagePercent * _message.Length));
	}
	
	public void ToggleHud()
	{
		startTime = Time.time;
		_active = !_active;
		/*StopAllCoroutines();
		if(!_active)
		{
			StartCoroutine("ActivateHud");	
		}
		else
		{
			StartCoroutine("DeactivateHud");
		}*/
	}
	
	public void SetRadio(bool active)
	{
	    //Debug.LogError("should not be called");
		startTime = Time.time;
		_active = active;
		/*StopAllCoroutines();
		if(active)
		{
			StartCoroutine("ActivateHud");
		}
		else
		{
			StartCoroutine("DeactivateHud");
		}*/
	}
	
	override public void OnTriggered(EventReaction evr)
	{
		if(evr.type==EventReaction.Type.Say)
		{
			Message message = ConfigLoader.GetMessage(evr.messageName);
			Debug.Log("Displaying message " + message.text);
			_message = message.text;
			
			SetRadio(true);
			//audio.PlayOneShot(message.audio);
			
			//StopAllCoroutines();
			//StartCoroutine("ActivateHud");
		}
	}
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