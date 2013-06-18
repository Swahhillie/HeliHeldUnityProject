using UnityEngine;
using System.Collections;
[System.Serializable]
public class GestureSettings : System.Object
{
	public enum Hand
	{
		Left,
		Right
	};
	
	public float activationDistanceFlying = 0.3f;
	public float activationDistanceRadio = 0.6f;
	public float activationDistanceLeave = -0.1f;
	public Hand activeHand = Hand.Right;
}

[System.Serializable]
public class SavingModeGesture : GestureAction
{
	public override GestureType Type {
		get {
			return GestureType.SavingModeGesture;
		}
	}
	protected override bool CheckGesture ()
	{
		bool gestureActive = false;
		float dist = GetDistanceBetweenBones (Kinect.NuiSkeletonPositionIndex.HandLeft, Kinect.NuiSkeletonPositionIndex.HandRight);
		
		//bool leftUnderShoulder = GetVectorBetween(Kinect.NuiSkeletonPositionIndex.ShoulderLeft, Kinect.NuiSkeletonPositionIndex.HandLeft).y < 0;
		//bool rightUnderShoulder = GetVectorBetween(Kinect.NuiSkeletonPositionIndex.ShoulderRight, Kinect.NuiSkeletonPositionIndex.HandRight).y < 0;
		
		//Debug.Log("Left: " + leftUnderShoulder + "  Right: " + rightUnderShoulder);
		float distToHead; 
		if(settings.activeHand == GestureSettings.Hand.Right)
			distToHead = GetDistanceBetweenBones(Kinect.NuiSkeletonPositionIndex.HandLeft, Kinect.NuiSkeletonPositionIndex.Head);
		else
			distToHead = GetDistanceBetweenBones(Kinect.NuiSkeletonPositionIndex.HandRight, Kinect.NuiSkeletonPositionIndex.Head);
		
		if (dist > settings.activationDistanceFlying && distToHead > settings.activationDistanceRadio) {
			gestureActive = true;
		} else {
			gestureActive = false;
		}
		return gestureActive;
	}
}
[System.Serializable]
public class FlyingModeGesture : GestureAction
{
	public override GestureType Type {
		get {
			return GestureType.FlyingModeGesture;
		}
	}
	protected override bool CheckGesture ()
	{
		bool gestureActive = false;
		float dist = GetDistanceBetweenBones (Kinect.NuiSkeletonPositionIndex.HandLeft, Kinect.NuiSkeletonPositionIndex.HandRight);
		Vector3 distLeft = GetVectorBetween (Kinect.NuiSkeletonPositionIndex.HandLeft, Kinect.NuiSkeletonPositionIndex.ShoulderLeft);
		Vector3 distRight = GetVectorBetween (Kinect.NuiSkeletonPositionIndex.HandRight, Kinect.NuiSkeletonPositionIndex.ShoulderRight);
		
		Debug.Log(distRight.y);
		if (dist < settings.activationDistanceFlying && distLeft.y > 0 && distRight.y > 0){
			gestureActive = true;
		} else {
			gestureActive = false;
		}
		return gestureActive;
	}
}
[System.Serializable]
public class ExitModeGesture : GestureAction
{
	public override GestureType Type {
		get {
			return GestureType.ExitModeGesture;
		}
	}
	protected override bool CheckGesture ()
	{
		bool gestureActive = false;
		Vector3 distLeft = GetVectorBetween (Kinect.NuiSkeletonPositionIndex.HandLeft, Kinect.NuiSkeletonPositionIndex.Head);
		Vector3 distRight = GetVectorBetween (Kinect.NuiSkeletonPositionIndex.HandRight, Kinect.NuiSkeletonPositionIndex.Head);
		
		if (distLeft.y < settings.activationDistanceLeave && distRight.y < settings.activationDistanceLeave) {
			gestureActive = true;
		} else {
			gestureActive = false;
		}
		
		return gestureActive;
	}
}
[System.Serializable]
public class RadioGesture : GestureAction{
	
	public override GestureType Type {
		get {
			return GestureType.RadioGesture;
		}
	}
	protected override bool CheckGesture ()
	{
		bool gestureActive = false;
		float dist; 
		if(settings.activeHand == GestureSettings.Hand.Right)
			dist = GetDistanceBetweenBones(Kinect.NuiSkeletonPositionIndex.HandLeft, Kinect.NuiSkeletonPositionIndex.Head);
		else
			dist = GetDistanceBetweenBones(Kinect.NuiSkeletonPositionIndex.HandRight, Kinect.NuiSkeletonPositionIndex.Head);
		
		if(dist < settings.activationDistanceRadio)
		{
			gestureActive = true;
		}
		else{
			gestureActive = false;
		}
		return gestureActive;
	}
}
[System.Serializable]
public abstract class GestureAction : System.Object
{
	public static GestureSettings settings;
	public enum GestureType{SavingModeGesture, RadioGesture, FlyingModeGesture, ExitModeGesture};
	
	public float activationTime = 1.5f;
	protected float timeSinceStart = 0.0f;
	protected bool isGestureActive = false;
	private bool activated = false;
	
	public delegate void KinectEventActive (GestureAction sender,System.EventArgs e);

	public event KinectEventActive BecomeActived;
	public event KinectEventActive WhileActivated;
	public event KinectEventActive BecomeDeactived;
	
	public abstract GestureType Type{
		get;
	}
	protected abstract bool CheckGesture ();
	public static SkeletonWrapper skelWrap;
	
	protected float GetDistanceBetweenBones (Kinect.NuiSkeletonPositionIndex bone1, Kinect.NuiSkeletonPositionIndex bone2)
	{
		return Vector3.Distance (skelWrap.bonePos [0, (int)bone1], skelWrap.bonePos [0, (int)bone2]);
	}
	protected Vector3 GetVectorBetween(Kinect.NuiSkeletonPositionIndex fromBone, Kinect.NuiSkeletonPositionIndex toBone)
	{
		return skelWrap.bonePos [0, (int)toBone] - skelWrap.bonePos [0, (int)fromBone];
	}
	public void Update ()
	{
		isGestureActive = CheckGesture();
		if (isGestureActive) {
			timeSinceStart += Time.deltaTime;
			if (timeSinceStart > activationTime) {
				Activated = true;
				WhileActivated(this, new System.EventArgs());
			}
		} else {
			timeSinceStart = 0;
			Activated = false;
		}
	}
	public bool Activated{
		get{return activated;}
		private set{
			if(activated != value){
				activated = value;
				if(value == true){
					BecomeActived(this, new System.EventArgs());
				}
				else{
					BecomeDeactived(this, new System.EventArgs());
				}
			}
		}
	}
}