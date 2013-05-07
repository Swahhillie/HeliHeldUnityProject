using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class Message
{
	private string label;
	private float delay;
		
	public Message(string text,float time)
	{
		label=text;
		delay=time;
	}
	
	public bool checkTime(float step)
	{
		delay-=step;
		if(delay<=0)
		{
			return false;
		}
		else
		{
			return true;	
		}
	}
	
	public string getLabel()
	{
		return (label);	
	}
}

public class HUD : MonoBehaviour {
	
	
	private List<Message> messages;
	// Use this for initialization
	void Start () 
	{
		messages = new List<Message>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		for(int x=messages.Count-1;x>=0;--x)
		{
			if(!messages[x].checkTime(Time.deltaTime))
			{
				messages.RemoveAt(x);
			}
		}
	}
	
	public void addMessage(string text,float time)
	{
		messages.Add(new Message(text,time));	
	}
	
	void OnGUI()
	{
		for(int x=messages.Count-1;x>=0;--x)
		{
			GUI.Label(new Rect(200,100+(40*x),800,120),messages[x].getLabel());
		}
	}
}
