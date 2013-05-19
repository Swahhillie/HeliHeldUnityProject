using UnityEngine;
using System.Collections;

public class ControlKinect : ControlBase
{
	public float forceThresshold = .5f;
	public float steerSensitivity = 4.0f;
	
	private KinectGestures kg;
	
	Vector3 shouldC;
	Vector3 handR;
	Vector3 elbowR;
	Vector3 handL;
	Vector3 elbowL;
	
	override protected void StartConcrete()
	{
		kg = new KinectGestures(skelWrap);
	}
	
	void Update()
	{
		float oldVal;
		
		if(!kg.GetHandsApart()) //hands tight
		{
			//set handstight time and save old value
			oldVal = actionTimers[Action.HandsTight];
			actionTimers[Action.HandsTight] += Time.deltaTime;
			Debug.Log(actionTimers[Action.HandsTight]);
			
			//if handstight is hold longer then set value...
			if(actionTimers[Action.HandsTight] > actionTime)
			{
				//reset handsapart
				actionActive[Action.HandsApart] = false;
				actionTimers[Action.HandsApart] = 0.0f;
				
				actionActive[Action.HandsTight] = true;
				if(oldVal < actionTime) heli.EnterFlyMode(); //if old value was smaller, set transition
				
				//calculate flying variables
				Vector3 dir = Vector3.zero;
				handR = skelWrap.bonePos[0, (int)Kinect.NuiSkeletonPositionIndex.HandRight];
				elbowR = skelWrap.bonePos[0, (int)Kinect.NuiSkeletonPositionIndex.ElbowRight];
				handL = skelWrap.bonePos[0, (int)Kinect.NuiSkeletonPositionIndex.HandLeft];
				elbowL = skelWrap.bonePos[0, (int)Kinect.NuiSkeletonPositionIndex.ElbowLeft];
				
				Vector3 diff = (handR + handL)*0.5f - (elbowR + elbowL)*0.5f;
				
				float steer = diff.x * steerSensitivity;
				
				dir.z = 0.5f - diff.y;
				
				dir = dir.magnitude > forceThresshold ? Vector3.Normalize(dir) : Vector3.zero;
				
				heli.Steer(steer);
				heli.Accelerate(dir);
			}
		}
		else //hands apart
		{
			//hands are apart
			oldVal = actionTimers[Action.HandsApart];
			actionTimers[Action.HandsApart] += Time.deltaTime;
			
			if(actionTimers[Action.HandsApart] > actionTime) //activated
			{
				//reset handstight
				actionActive[Action.HandsTight] = false;
				actionTimers[Action.HandsTight] = 0.0f;
				
				actionActive[Action.HandsApart] = true;
				if(oldVal < actionTime) heli.EnterSaveMode(); //transition
				
				cursorPosition = kg.GetCursorPosition();
				//give position to helicopter which handles it in saving mode
			}
			
			if(kg.GetRadioGesture()) //radio gesture on
			{
				oldVal = actionTimers[Action.Radio];
				actionTimers[Action.Radio] += Time.deltaTime;
				
				if(actionTimers[Action.Radio] > actionTime)
				{
					actionActive[Action.Radio] = true;
					if(oldVal < actionTime)
					{
						heli.ActivateRadio();
					}
				}
			}
			else //no radio gesture
			{
				if(actionActive[Action.Radio] == true) //if active, disable radio
					heli.DeactivateRadio();
				
				actionActive[Action.Radio] = false;
				actionTimers[Action.Radio] = 0.0f;
			}
		}
	}
}
