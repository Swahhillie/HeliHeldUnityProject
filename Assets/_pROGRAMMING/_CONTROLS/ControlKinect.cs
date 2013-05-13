using UnityEngine;
using System.Collections;

public class ControlKinect : ControlBase
{
	public float forceThresshold = .5f;
	public float steerSensitivity = 5.0f;
	
	private KinectGestures kg;
	private GUIStyle guistyle;
	
	Vector3 shouldC;
	Vector3 handR;
	Vector3 elbowR;
	Vector3 handL;
	Vector3 elbowL;
	
	void Start()
	{
		kg = new KinectGestures(skelWrap);
		guistyle = new GUIStyle();
		guistyle.fontSize = 56;
	}
	string test;
	void Update()
	{
		if(kg.GetHandsApart())
		{
			Vector3 dir = Vector3.zero;
			handR = skelWrap.bonePos[0, (int)Kinect.NuiSkeletonPositionIndex.HandRight];
			elbowR = skelWrap.bonePos[0, (int)Kinect.NuiSkeletonPositionIndex.ElbowRight];
			handL = skelWrap.bonePos[0, (int)Kinect.NuiSkeletonPositionIndex.HandLeft];
			elbowL = skelWrap.bonePos[0, (int)Kinect.NuiSkeletonPositionIndex.ElbowLeft];
			
			Vector3 diff = (handR + handL)*0.5f - (elbowR + elbowL)*0.5f;
			
			float steer = diff.x * steerSensitivity;
			
			dir.z = 0.5f - diff.y;
			
			dir = dir.magnitude > forceThresshold ? Vector3.Normalize(dir) : Vector3.zero;
			
			test = dir.z.ToString();
			
			heli.Steer(steer);
			heli.Accelerate(dir);
		}
		else
		{
			cursorPosition = kg.GetCursorPosition();
		}
	}
	
	void OnGUI()
	{
		GUI.contentColor = Color.green;
		GUI.Label(new Rect(100, 100, 300, 300), test, guistyle);
	}
}
