using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControlBase : MonoBehaviour
{
	public float actionTime = 1.0f;
	
	public enum Action
	{
		//general actions
		Radio,
		
		//flying actions
		HandsTight,
		GoLeft,
		GoRight,
		GoForward,
		GoBack,
		
		//saving actions
		HandsApart
	}
	
	public Dictionary<Action, float> actionTimers = new Dictionary<Action, float>()
	{
		{ Action.Radio, 0.0f },
		{ Action.HandsTight, 0.0f },
		{ Action.GoLeft, 0.0f },
		{ Action.GoRight, 0.0f },
		{ Action.GoForward, 0.0f },
		{ Action.GoBack, 0.0f },
		{ Action.HandsApart, 0.0f }
	};
	
	public Dictionary<Action, bool> actionActive = new Dictionary<Action, bool>()
	{
		{ Action.Radio, false },
		{ Action.HandsTight, false },
		{ Action.GoLeft, false },
		{ Action.GoRight, false },
		{ Action.GoForward, false },
		{ Action.GoBack, false },
		{ Action.HandsApart, false }
	};
	
	public Vector2 cursorPosition;
	
	public Helicopter heli;
	public SkeletonWrapper skelWrap;
	
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
