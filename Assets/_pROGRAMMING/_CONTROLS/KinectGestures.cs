using UnityEngine;
using System.Collections;

public class KinectGestures
{
	public SkeletonWrapper skelWrap;
	
	public enum Hand
	{
		Left,
		Right
	};
	
	public static Hand activeHand = Hand.Right;
	public static float distHandsApart = 0.3f;
	private static bool handsApart = true;
	
	private static Vector3 handPos;
	private static Vector3 elbowPos;
	private static Vector2 cursorPos;
	
	static int iHandR;
	static int iHandL;
	static int iElbowR;
	static int iElbowL;
	static int iShouldC;
		
	public KinectGestures(SkeletonWrapper aSkelWrap)
	{
		skelWrap = aSkelWrap;
		
		iHandR = (int)Kinect.NuiSkeletonPositionIndex.HandRight;
		iHandL = (int)Kinect.NuiSkeletonPositionIndex.HandLeft;
		iShouldC = (int)Kinect.NuiSkeletonPositionIndex.ShoulderCenter;
	}
	
	public static Vector2 cursorPosition {
		get{ return cursorPos;}
	}
	
	public bool GetHandsApart()
	{
		float dist = GetDistanceBetweenBones(Kinect.NuiSkeletonPositionIndex.HandLeft, Kinect.NuiSkeletonPositionIndex.HandRight);
		if(dist < distHandsApart)
			return true;
		return false;
	}
	
	public Vector2 GetCursorPosition()
	{
		if (activeHand == Hand.Right)
			handPos = skelWrap.bonePos [0, iHandR];
		else
			handPos = skelWrap.bonePos [0, iHandL];
		
		//2 and 1.6 are temp(!) hardcoded values
		float distX = (handPos.x * 2 - skelWrap.bonePos [0, iShouldC].x);
		float distY = (handPos.y * 1.6f - skelWrap.bonePos [0, iShouldC].y);
		
		distX = Mathf.Clamp (distX, 0, 1);
		distY = Mathf.Clamp (distY, 0, 1);
		
		return new Vector2 (Screen.width * distX, Screen.height * distY);
	}

	public float GetDistanceBetweenBones(Kinect.NuiSkeletonPositionIndex bone1, Kinect.NuiSkeletonPositionIndex bone2)
	{
		return Vector3.Distance(skelWrap.bonePos[0, (int)bone1],skelWrap.bonePos[0, (int)bone2]);
	}
	
	public Vector3 GetBoneDifference(Kinect.NuiSkeletonPositionIndex bone1, Kinect.NuiSkeletonPositionIndex bone2)
	{
		return skelWrap.bonePos[0, (int)bone1] - skelWrap.bonePos[0, (int)bone2];
	}
}
