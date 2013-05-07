using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	
	private static Dictionary<string,Type> typeHelper= new Dictionary<string, Type>();
	private static bool init=false;
	
	public Type type;
	public string message;
	public Vector3 pos;
	
	EventReaction(string aType,string aMessage=null,Vector3 aPos=default(Vector3))
	{
		if(!init)
		{
			init=true;
			typeHelper["Spawn"]=Type.Spawn;
			typeHelper["Destroy"]=Type.Destroy;	
			typeHelper["Radio"]=Type.Radio;	
			typeHelper["Displace"]=Type.Displace;
			typeHelper["Enable"]=Type.Enable;
			typeHelper["Disable"]=Type.Disable;
		}	
		
		type=typeHelper[aType];
		message = aMessage;
		pos = aPos;
	}
}
