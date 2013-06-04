using UnityEngine;
using System.Collections;

[System.Serializable]
public class HandApartGesture : GestureAction
{
	public float activationDistance;
	public override GestureType Type {
		get {
			return GestureType.HandsApart;
		}
	}
	protected override bool CheckGesture ()
	{
		bool gestureActive = false;
		float dist = GetDistanceBetweenBones (Kinect.NuiSkeletonPositionIndex.HandLeft, Kinect.NuiSkeletonPositionIndex.HandRight);
		if (dist > activationDistance) {
			gestureActive = true;
		} else {
			gestureActive = false;
		}
		return gestureActive;
	}
}
[System.Serializable]
public class HandToHeadGesture : GestureAction{
	
	public KinectGestures.Hand activeHand;
	public float activationDistance  = 1.0f;
	public override GestureType Type {
		get {
			return GestureType.HandToHead;
		}
	}
	protected override bool CheckGesture ()
	{
		bool gestureActive = false;
		float dist; 
		if(activeHand == KinectGestures.Hand.Left)
			dist = GetDistanceBetweenBones(Kinect.NuiSkeletonPositionIndex.HandLeft, Kinect.NuiSkeletonPositionIndex.Head);
		else
			dist = GetDistanceBetweenBones(Kinect.NuiSkeletonPositionIndex.HandRight, Kinect.NuiSkeletonPositionIndex.Head);
		Debug.Log(dist);
		if(dist < activationDistance)
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
	
	public enum GestureType{HandsApart, HandToHead};
	
	public float activationTime = 2.0f;
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