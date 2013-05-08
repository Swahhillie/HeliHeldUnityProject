using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControlBase : MonoBehaviour
{
	public enum Action
	{
		HoldStick,
		GoLeft,
		GoRight,
		GoForward,
		GoBack
	}
	
	private Dictionary<int, bool> actions; //true if active/used
	
	public Helicopter heli;
	
//	public ControlBase()
//	{
//		
//	}
//	
//	float GetAcceleration()
//	{
//		return 0.0f;
//	}
//	
//	float GetDirection()
//	{
//		return 0.0f;
//	}
//	
//	Vector2 GetCursorPosition()
//	{
//		return Vector2(0,0,0);
//	}
//	
//	bool IsActionActive(Action anAction)
//	{
//		return actions[anAction];
//	}
}
