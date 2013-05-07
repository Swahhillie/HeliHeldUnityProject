using UnityEngine;
using System.Collections;

public class KinectMouse : MonoBehaviour
{
	public SkeletonWrapper skelWrap;
	public enum Hand
	{
		Left,
		Right
	};
	public Hand hand = Hand.Right;
	private Vector3 handPos;
	private Vector2 screenPos;
	private Texture2D handTex;
	
	void Start ()
	{
		Screen.showCursor = false;
		handTex = Resources.Load ("Hand", typeof(Texture2D)) as Texture2D;
	}
	
	void Update ()
	{
		if (!skelWrap.pollSkeleton ()) { //no kinect
			screenPos = Input.mousePosition;
			return;
		}
		int handR = (int)Kinect.NuiSkeletonPositionIndex.HandRight;
		int handL = (int)Kinect.NuiSkeletonPositionIndex.HandLeft;
		int shouldC = (int)Kinect.NuiSkeletonPositionIndex.ShoulderCenter;
		
		
		if (hand == Hand.Right)
			handPos = skelWrap.bonePos [0, handR]; //right hand, aka 11
		else
			handPos = skelWrap.bonePos [0, handL]; //left hand, aka 7

		float distX = (handPos.x * 2 - skelWrap.bonePos [0, shouldC].x); //shoulder center aka 2
		float distY = (handPos.y * 1.6f - skelWrap.bonePos [0, shouldC].y); //shoulder center aka 2
		
		
		distX = Mathf.Clamp (distX, 0, 1);
		distY = Mathf.Clamp (distY, 0, 1);
		
		
		screenPos = new Vector2 (Screen.width * distX, Screen.height * distY);
		
	}
	
	void OnGUI ()
	{
		if (Application.loadedLevelName == "menu" || Application.loadedLevelName == "PlacingObjects")
			GUI.Label (new Rect (screenPos.x - handTex.width / 2, Screen.height - screenPos.y - handTex.height / 2, handTex.width, handTex.height), handTex);
	}
	
	public Vector2 position {
		get{ return screenPos;}
	}
}
