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
	
	public Dictionary<Action, float> actionTimers = new Dictionary<Action, float>();
	
	public Dictionary<Action, bool> actionActive = new Dictionary<Action, bool>();
	
	private void Start()
	{
		StartConcrete();
		foreach(Action t in System.Enum.GetValues(typeof(Action))){
			actionTimers.Add(t, 0.0f);
		}
		foreach(Action t in System.Enum.GetValues(typeof(Action))){
			actionActive.Add(t, false);
		}
		
	}
	protected virtual void StartConcrete(){
		
	}
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
