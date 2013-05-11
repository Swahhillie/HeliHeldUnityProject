using UnityEngine;
using System.Collections;

public class HudScript : TriggeredObject  
{
	private float _width;	
	private string _message;
	private bool _active;
	
	public float width
	{
		get{return _width;}
		set{_width = value;}
	}
	
	private IEnumerator DisplayText()
	{
		float time = 0.01f;
		TextMesh tMesh = this.GetComponent<TextMesh>();
		tMesh.font.material.color=Color.green;
		tMesh.text="";
		for(int i = 0; i < _message.Length; i++)
		{
			tMesh.text+= _message[i];
			yield return new WaitForSeconds(time);
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
	
	private IEnumerator ActivateHud()
	{
		_active=true;
		for(int i=(int)(this.transform.localScale.x*10.0f);i<=10;++i)
		{
			this.transform.localScale=new Vector3((float)i/10,this.transform.localScale.y,0.0f);
			yield return new WaitForSeconds(0.01f);
		}
		for(int i=(int)(this.transform.localScale.y*10.0f);i<=10;++i)
		{
			this.transform.localScale=new Vector3(this.transform.localScale.x,(float)i/10,0);
			yield return new WaitForSeconds(0.01f);
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
			yield return new WaitForSeconds(0.01f);
		}	
		for(int i=(int)(this.transform.localScale.x*10.0f);i>=0;--i)
		{
			this.transform.localScale=new Vector3((float)i/10,this.transform.localScale.y,0.0f);
			yield return new WaitForSeconds(0.01f);
		}
				
	}
	
	override public void OnTriggered(EventReaction evr)
	{
		if(evr.type==EventReaction.Type.Say)
		{
			Message message = ConfigLoader.GetMessage(evr.messageName);
			
			_message = message.text;
			StopAllCoroutines();
			StartCoroutine("ActivateHud");
		}
	}
}