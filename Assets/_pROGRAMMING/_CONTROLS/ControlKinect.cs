using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	public SavingModeGesture savingModeGesture = new SavingModeGesture();
	public RadioGesture radioGesture = new RadioGesture();
	public FlyingModeGesture flyingModeGesture = new FlyingModeGesture();
	
	private List<GestureAction> gestures;
	
	override protected void StartConcrete ()
	{
		kg = new KinectGestures (skelWrap);
		handTex = Resources.Load ("Hand", typeof(Texture2D)) as Texture2D;
		
		gestures = new List<GestureAction>();
		
		AddGesture(savingModeGesture);
		AddGesture(flyingModeGesture);
		AddGesture(radioGesture);
		
		GestureAction.skelWrap = skelWrap;
		
	}
	private void AddGesture(GestureAction g){
		g.BecomeActived += OnGestureBecomeActive;
		g.BecomeDeactived += OnGestureBecomeDeactive;
		g.WhileActivated += OnGesture;
		gestures.Add(g);
	}
	void Update(){
		foreach(var g in gestures)g.Update();
		
		cursorPosition = kg.GetCursorPosition ();
		
		if(heli.IsSaving())
		{
			KinectSaving();
		}
	}
	private void OnGestureBecomeActive(GestureAction sender, System.EventArgs e){
		
		switch(sender.Type)
		{
			case GestureAction.GestureType.SavingModeGesture:
				heli.EnterSaveMode();
				break;
			
			case GestureAction.GestureType.FlyingModeGesture:
				heli.EnterFlyMode();
				break;
			
			case GestureAction.GestureType.RadioGesture:
				heli.ToggleRadio();
				break;
			
			default:
				break;
		}
	}
	private void OnGesture(GestureAction sender, System.EventArgs e){
	
		switch(sender.Type)
		{
			case GestureAction.GestureType.SavingModeGesture:
				
				break;
			
			case GestureAction.GestureType.FlyingModeGesture:
				KinectFlying();
				break;
			
			case GestureAction.GestureType.RadioGesture:
			
				break;
			
			default:
				break;
		}
	}
	private void OnGestureBecomeDeactive(GestureAction sender, System.EventArgs e){
		switch(sender.Type)
		{
			case GestureAction.GestureType.SavingModeGesture:
			
				break;
			
			case GestureAction.GestureType.FlyingModeGesture:
			
				break;
			
			case GestureAction.GestureType.RadioGesture:
				heli.DeactivateRadio();
				break;
			
			default:
				break;
		}
	}
	void DebugGUI(){
		
			GUILayout.BeginVertical();
			foreach(var g in gestures)
				GUILayout.Label(string.Format("gesture {0} is {1}", g.Type, g.Activated ));
			GUILayout.EndVertical();
		
	}
	
//	void UpdateOld ()
//	{
//		float oldVal;
//		
//		if (!kg.GetHandsApart ()) { //hands tight
//			showHand = false;
//			
//			//set handstight time and save old value
//			oldVal = actionTimers [Action.HandsTight];
//			actionTimers [Action.HandsTight] += Time.deltaTime;
//			
//			//if handstight is hold longer then set value...
//			if (actionTimers [Action.HandsTight] > actionTime) {
//				KinectFlying ();
//			}
//		} else if (kg.GetRadioGesture ()) { //radio gesture on
//			oldVal = actionTimers [Action.Radio];
//			actionTimers [Action.Radio] += Time.deltaTime;
//			
//			if (actionTimers [Action.Radio] > actionTime) {
//				KinectRadio ();
//			}
//		} else { //no radio gesture
//		
//			//hands are apart
//			oldVal = actionTimers [Action.HandsApart];
//			actionTimers [Action.HandsApart] += Time.deltaTime;
//			
//			if (actionTimers [Action.HandsApart] > actionTime) { //activated
//				//reset handstight
//				actionActive [Action.HandsTight] = false;
//				actionTimers [Action.HandsTight] = 0.0f;
//				
//				if (actionActive [Action.Radio]) {
//					//if active, disable radio
//					heli.DeactivateRadio ();
//				}
//				
//				actionActive [Action.Radio] = false;
//				actionTimers [Action.Radio] = 0.0f;
//				
//				actionActive [Action.HandsApart] = true;
//				if (oldVal < actionTime) {
//					heli.EnterSaveMode ();
//				} //transition
//				
//				cursorPosition = kg.GetCursorPosition ();
//				
//				if (heli.IsSaving ()) {
//					KinectSaving ();
//				}
//			} else {
//				showHand = false;
//			}
//		}
//	}
	
	void OnGUI ()
	{
		bool debug = false;
		if(debug){DebugGUI();}
		
		if (showHand) {
			GUI.DrawTexture (new Rect (cursorPosition.x - handTex.width * 0.5f, Screen.height - cursorPosition.y - handTex.height * 0.5f, handTex.width, handTex.height), handTex);
		}
	}
	
	void KinectFlying ()
	{
		//calculate flying variables
		Vector3 dir = Vector3.zero;
		handR = skelWrap.bonePos [0, (int)Kinect.NuiSkeletonPositionIndex.HandRight];
		elbowR = skelWrap.bonePos [0, (int)Kinect.NuiSkeletonPositionIndex.ElbowRight];
		handL = skelWrap.bonePos [0, (int)Kinect.NuiSkeletonPositionIndex.HandLeft];
		elbowL = skelWrap.bonePos [0, (int)Kinect.NuiSkeletonPositionIndex.ElbowLeft];
		
		Vector3 diff = (handR + handL) * 0.5f - (elbowR + elbowL) * 0.5f;
		
		float steer = diff.x * steerSensitivity; //add sensitivity value
		
		dir.z = 0.5f - diff.y; //prevent backwards flying
		
		dir = dir.magnitude > forceThresshold ? Vector3.Normalize (dir) : Vector3.zero; //get direction
		
		heli.Steer (steer);
		heli.Accelerate (dir);
	}
	
	void KinectSaving ()
	{
		//give position to helicopter which handles it in saving mode
		float saveX = (cursorPosition.x / Screen.width) - 0.5f;
		float saveZ = (cursorPosition.y / Screen.height) - 0.5f;
					
		//sensitivity fix
		if (saveX < 0.25f && saveX > 0.0f) {
			saveX = 0;
		}
		if (saveX > -0.25f && saveX < 0.0f) {
			saveX = 0;
		}
		if (saveZ < 0.25f && saveZ > 0.0f) {
			saveZ = 0;
		}
		if (saveZ > -0.25f && saveZ < 0.0f) {
			saveZ = 0;
		}
				
		heli.Accelerate (new Vector3 (saveX * saveSensitvity, 0, saveZ * saveSensitvity));
				
		showHand = true;
	}
}
