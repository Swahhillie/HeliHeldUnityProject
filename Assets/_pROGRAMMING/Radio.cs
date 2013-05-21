using UnityEngine;
using System.Collections;

public class Radio : TriggeredObject  
{
	private float _width;	
	private string _message = "";
	private bool _active;
	public float delay = 0.01f;
	
	public float width
	{
		get{return _width;}
		set{_width = value;}
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
	
	public void ToggleHud()
	{
		StopAllCoroutines();
		if(!_active)
		{
			StartCoroutine("ActivateHud");	
		}
		else
		{
			StartCoroutine("DeactivateHud");
		}
	}
	
	public void SetRadio(bool active)
	{
		StopAllCoroutines();
		if(active)
		{
			StartCoroutine("ActivateHud");
		}
		else
		{
			StartCoroutine("DeactivateHud");
		}
	}
	
	private IEnumerator ActivateHud()
	{
		_active=true;
		for(int i=(int)(this.transform.localScale.x*10.0f);i<=10;++i)
		{
			this.transform.localScale=new Vector3((float)i/10,this.transform.localScale.y,0.0f);
			yield return new WaitForSeconds(delay);
		}
		for(int i=(int)(this.transform.localScale.y*10.0f);i<=10;++i)
		{
			this.transform.localScale=new Vector3(this.transform.localScale.x,(float)i/10,0);
			yield return new WaitForSeconds(delay);
		}
		StartCoroutine("DisplayText");
	}
	
	private IEnumerator DeactivateHud()
	{
		_active=false;
		TextMesh tMesh = this.GetComponent<TextMesh>();
		tMesh.text="";
		for(int i=(int)(this.transform.localScale.y*10.0f);i>0;--i)
		{
			this.transform.localScale=new Vector3(this.transform.localScale.x,(float)i/10,0);
			yield return new WaitForSeconds(delay);
		}	
		for(int i=(int)(this.transform.localScale.x*10.0f);i>=0;--i)
		{
			this.transform.localScale=new Vector3((float)i/10,this.transform.localScale.y,0.0f);
			yield return new WaitForSeconds(delay);
		}
				
	}
	
	override public void OnTriggered(EventReaction evr)
	{
		Debug.Log("asdfjkgaksfjdghlkjsdfghksjdfhglksjdgksjdfghlajghösdfgölsakdjfglö");
		if(evr.type==EventReaction.Type.Say)
		{
			Message message = ConfigLoader.GetMessage(evr.messageName);
			Debug.Log("Displaying message " + message.text);
			_message = message.text;

			audio.PlayOneShot(message.audio);
			
			StopAllCoroutines();
			StartCoroutine("ActivateHud");
		}
	}
}