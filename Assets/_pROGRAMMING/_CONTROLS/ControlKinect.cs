using UnityEngine;
using System.Collections;

public class ControlKinect : ControlBase
{
	public float forceThresshold = .5f;
	public float steerSensitivity = 4.0f;
	public float saveSensitvity = 30.0f;
	
	private KinectGestures kg;
	
	Vector3 shouldC;
	Vector3 handR;
	Vector3 elbowR;
	Vector3 handL;
	Vector3 elbowL;
	
	private Texture2D handTex;
	bool showHand;
	
	override protected void StartConcrete()
	{
		kg = new KinectGestures(skelWrap);
		handTex = Resources.Load ("Hand", typeof(Texture2D)) as Texture2D;
	}
	
	void Update()
	{
		float oldVal;
		
		if(!kg.GetHandsApart()) //hands tight
		{
			showHand = false;
			
			//set handstight time and save old value
			oldVal = actionTimers[Action.HandsTight];
			actionTimers[Action.HandsTight] += Time.deltaTime;
			
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
				float saveX = (cursorPosition.x / Screen.width) - 0.5f;
				float saveZ = (cursorPosition.y / Screen.height) - 0.5f;
				
				//sensitivity fix
				if(saveX < 0.25f && saveX > 0.0f) saveX = 0;
				if(saveX > -0.25f && saveX < 0.0f) saveX = 0;
				if(saveZ < 0.25f && saveZ > 0.0f) saveZ = 0;
				if(saveZ > -0.25f && saveZ < 0.0f) saveZ = 0;
				
				heli.Accelerate(new Vector3(saveX*saveSensitvity, 0, saveZ*saveSensitvity));
				
				showHand = true;
			}
			else
				showHand = false;
			
			if(kg.GetRadioGesture()) //radio gesture on
			{
				oldVal = actionTimers[Action.Radio];
				actionTimers[Action.Radio] += Time.deltaTime;
				
				if(actionTimers[Action.Radio] > actionTime)
				{
					actionActive[Action.Radio] = true;
					if(oldVal < actionTime)
					{
						heli.ToggleRadio();
					}
				}
			}
			else //no radio gesture
			{
				if(actionActive[Action.Radio]) //if active, disable radio
					heli.DeactivateRadio();
				
				actionActive[Action.Radio] = false;
				actionTimers[Action.Radio] = 0.0f;
			}
		}
	}
	
	void OnGUI()
	{
		Vector2 boxPos = new Vector2(1400, 150);
		Vector2 boxSize = new Vector2(120, 120);
		
		GUI.Box(new Rect(boxPos.x, boxPos.y, boxSize.x, boxSize.y), "");
		
		Vector3 playerPos = kg.GetPlayerPosition();
		//limits
		if(playerPos.x > 1) playerPos.x = 1;
		if(playerPos.x < -1) playerPos.x = -1;
		if(playerPos.z > 1) playerPos.z = -1;
		if(playerPos.z < -1) playerPos.z = -1;
		
		Vector2 playerOnscreen = new Vector2((playerPos.x*boxSize.x*0.5f), (playerPos.z*boxSize.y*0.5f));
		
		GUI.Label(new Rect(boxPos.x+boxSize.x*0.5f+playerOnscreen.x-10, boxPos.y+boxSize.y*0.5f+playerOnscreen.y-10, 20, 20), "X");
		
		if(showHand)
			GUI.Label (new Rect (cursorPosition.x - handTex.width*0.5f, Screen.height - cursorPosition.y - handTex.height*0.5f, handTex.width, handTex.height), handTex);
	}
}
