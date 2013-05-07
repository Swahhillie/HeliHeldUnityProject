using UnityEngine;
using System.Collections;

public class EventReaction
{
	public enum Type{
		Spawn,
		Destroy,
		Radio,
		Displace,
		Enable,
		Disable
	}
	
	public Type type;
	public string message;
	public Vector3 pos;
	
	EventReaction(Type aType,string aMessage=null,Vector3 aPos=default(Vector3))
	{
		type=aType;
		message = aMessage;
		pos = aPos;
	}
}
