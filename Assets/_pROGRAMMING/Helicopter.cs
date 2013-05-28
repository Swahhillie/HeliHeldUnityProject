using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.SerializableAttribute()]
public class HeliSettings
{
	public float maxSpeed = 2.0f;
	public float acceleration = 0.2f; // acceleration per second
	public float drag = .1f; // fraction of the velocity that is lost every second
	
	public float flyHeightMax = 40.0f; // maximum altitude of the helicopter. later used to stop the helicopter from flying over very high mountains
	public float avoidRadius = 20.0f; //Repulsor bubble around the helicopter
	
	public float rotationSpeed = 360.0f; // 360 = 1 rotation per second
	public float decendRate = .2f;
	/// <summary>
	/// degrees the helicopter tilts per second
	/// </summary>
	public float leanSpeed = -25.0f; 
	
	/// <summary>
	///how many seconds it takes for the helicopter to completly right itself after rotation
	/// </summary>
	public float rightSpeed = .6f;

	/// <summary>
	///max distance an object can be away from the helicopter for rescueing
	/// </summary>
	public float rescueRadius = 50.0f; 
	/// <summary>
	/// determines where the castaway must be in relation to the helicopter. 1 == Directly below, no margin for error, 0 == anywhere below the helicopter, -1 == Anywhere.
	/// </summary>
	public float hoverPrecision = .70f;
	/// <summary>
	/// rotation that the stick can go to the left or right.
	/// </summary>
	public float stickRotationRange = 45; 
	/// <summary>
	/// The amount the player has to steer to get the stick to move.
	/// </summary>
	public float stickRotationThresshold = 0.1f;
}

public class Helicopter : MonoBehaviour
{
	
	public SkeletonWrapper skelWrap;
	public HeliSettings heliSettings;
	public LayerMask terrainLayer;
	public float waterHeight = 20.0f; // height of the water. it has no collider so it cannot be raycast
	private Vector3 velocity;
	public bool debugLines = true;
	private Radio radio;
	public Animation camAnimation;
	
	private enum Helistate
	{
		IDLE,
		FLY,
		SAVE
	}
	
	private Helistate prevState;
	private Helistate state;
	public MissionObjectBase nearestRescuable = null;
	public float dotToNearest;
	private enum ControlType
	{
		None,
		Kinect,
		Keyboard
	}
	
	private IEnumerator rescueRoutine;
	private bool rescueing = false;
	public float rescueTime = 4.0f;
	
	private ControlType controlType = ControlType.None;
	public string toSavePosition;
	public string toPilotPosition;
	private RescuePointer rescuePointer;
	private RescueNearbyIndicator rescueNearbyIndicator;
	
	public Transform joystick;
	
	public void Start ()
	{
		
		rescuePointer = GetComponentInChildren<RescuePointer>();
		rescueNearbyIndicator = GetComponentInChildren<RescueNearbyIndicator>();
		radio = GetComponentInChildren<Radio> ();
		InitializeControls();
		
		if (heliSettings.rescueRadius < heliSettings.avoidRadius) {
			Debug.LogWarning (@"Helicopter avoid radius is bigger than rescue radius, 
			Objects on terrain will not get rescued because the helicopter will fly to high");
		}
	}
	
	private void InitializeControls()
	{
		prevState = Helistate.FLY;
		state = Helistate.FLY;
		
		
		
		// disable the keyboard if connect is there and vice versa
		if (skelWrap.devOrEmu.device.connected) {
			controlType = ControlType.Kinect;
			gameObject.GetComponent<ControlKeyboard> ().enabled = false;
				
		} else {
			
			controlType = ControlType.Keyboard;
			gameObject.GetComponent<ControlKinect> ().enabled = false;
		}
	}
	
	public float closestPoint = 0;
	public float distanceToWater = 0;

	//should work now. try it, so long as poll skelleton is false
	public void Steer (float x)
	{
		if (state == Helistate.FLY){
			transform.Rotate (Vector3.up, x * heliSettings.rotationSpeed * Time.deltaTime); //rotate over the y axis
		
			Quaternion goalRotation = Quaternion.AngleAxis (x * heliSettings.leanSpeed * Time.deltaTime, transform.forward) * transform.rotation; //lean over to a side
			transform.rotation = Quaternion.Slerp (goalRotation, Quaternion.FromToRotation (transform.up, Vector3.up) * transform.rotation, Time.deltaTime * heliSettings.rightSpeed);// bring upright overtime
			//joystick.localEulerAngles = new Vector3(0, 0, joystick.localEulerAngles.z + x);
			
			float angle = 0;
			if(Mathf.Abs(x) > heliSettings.stickRotationThresshold){
				angle = -x * heliSettings.stickRotationRange;
			}
			
			float joystickSpeed = 1.0f;
			Quaternion joystickGoal = Quaternion.AngleAxis(angle, Vector3.forward);
			joystick.localRotation = Quaternion.Lerp(joystick.localRotation, joystickGoal, Time.deltaTime * joystickSpeed);
		}
		else if( state == Helistate.SAVE){
			Accelerate(new Vector3(x, 0, 0));
			
		}
		
		
	}

	public void Accelerate (Vector3 direction)
	{
		direction = transform.TransformDirection (direction); // make the direction in local space
		AddAvoidForce(ref direction);
		
		if (state != Helistate.IDLE) {			
						
			direction = Vector3.Normalize (direction);
			velocity += heliSettings.acceleration * Time.deltaTime * direction;
			
			
			
			
			velocity = Vector3.ClampMagnitude (velocity, heliSettings.maxSpeed);
		}
	}

	private void AddAvoidForce (ref Vector3 direction)
	{
		RaycastHit[] hits = Physics.SphereCastAll (transform.position, heliSettings.avoidRadius, Vector3.down, heliSettings.avoidRadius, terrainLayer.value);
		float lastDistance = Mathf.Infinity;
		RaycastHit closestHit = new RaycastHit ();// = System.Array.FindLast<RaycastHit>(hits, x => x.distance < lastDistance);
		foreach (RaycastHit h in hits) {
			if (h.distance < lastDistance) {
				closestHit = h;
				lastDistance = h.distance;
			}
		}
		//no hits below helicopter
		if (hits.Length > 0) {
			//k			Debug.Log(closestHit.transform.name);
			if (debugLines)
				Debug.DrawLine (transform.position, closestHit.point, Color.red);
			closestPoint = closestHit.distance;
		}
					
		distanceToWater = transform.position.y - waterHeight;
		if (distanceToWater < heliSettings.avoidRadius) {//avoid water first
			if (debugLines)
				Debug.DrawLine (transform.position, transform.position - new Vector3 (0, distanceToWater, 0), Color.magenta);
			direction += Vector3.up; //water is always below the helicopter
			closestPoint = distanceToWater;
		} else if (closestHit.distance < heliSettings.avoidRadius && hits.Length > 0) { // if there are hits below helicopter, move away from those hits
			direction += Vector3.Normalize (transform.position - closestHit.point); //directly away from closest hit
			closestPoint = closestHit.distance;
		} else {
			direction += Vector3.down * heliSettings.decendRate;	
		}
			
			
	}

	private void OnDrawGizmos ()
	{
		Gizmos.color = Color.cyan;
		if (debugLines)
			Gizmos.DrawWireSphere (transform.position, heliSettings.rescueRadius);
	}
	
	private IEnumerator RescueRoutine()
	{
		
		if(rescueing == true)
		{
			Debug.LogError("Multiple rescue routines are running at the same time! Respect the guard!");
			
		}
		else{
			rescueing = true;
			float elaspedTime = 0;
			while(dotToNearest > heliSettings.hoverPrecision){
				elaspedTime += Time.deltaTime;
				if(elaspedTime > rescueTime)
				{
					Debug.Log("Rescue timer gone off");
					MakeRescue();
					break;
				}
				else{
					yield return null;
				}
			}
			rescueing = false;
		}
		
		
	}
	bool FindNearestInRange<T> (List<T> toSearch, Vector3 fromPos, float radius, out T result) where T : Component
	{
		
		
		result = null;
		float closest = radius * radius;
		foreach (var mib in toSearch) {
			if(mib.gameObject.activeSelf == false)continue; //skip over disabled level object
			Transform tr = mib.transform;
			float dist = Vector3.SqrMagnitude (tr.position - fromPos);
			if (dist < closest) {
				closest = dist;
				result = mib;
				
			}
		}
		return result != null;
		
	}

	float GetDotToRescuable (Transform missionObject)
	{
		
		
		//check if the mib is positioned withing the hoverprecision of the helicopter
		Vector3 vecToMib = Vector3.Normalize (missionObject.position - transform.position);
		return Vector3.Dot (vecToMib, Vector3.down);		

	}

	void Update ()
	{
		if (debugLines)
			Debug.DrawRay (transform.position, velocity, Color.cyan);
		transform.position += velocity * Time.deltaTime; //misschien niet de deltatime hier maar in de controller. waarschijn lijk wel though
		
		if(state == Helistate.SAVE)
		   velocity = new Vector3(0.0f, 0.0f, 0.0f);
		else
		   velocity *= 1 - heliSettings.drag * Time.deltaTime;
		
		if (controlType == ControlType.Kinect){
			skelWrap.pollSkeleton ();
		}
			
		heliSettings.hoverPrecision = Mathf.Clamp (heliSettings.hoverPrecision, -1.0f, 1.0f);
		
		
		if (FindNearestInRange <MissionObjectBase> (GetRescuables (), transform.position, heliSettings.rescueRadius, out nearestRescuable)) {
			dotToNearest = GetDotToRescuable (nearestRescuable.transform);
			rescueNearbyIndicator.Activate();
		}
		else{
			rescueNearbyIndicator.DeActivate();
			dotToNearest = 0;
		}

		
		if (state == Helistate.SAVE) {
			
			rescuePointer.alpha = dotToNearest;
		
			
			if(!rescueing)StartCoroutine(RescueRoutine());
			
		}
	}
	public void MakeRescue()
	{
		nearestRescuable.AttemptRescue();
	}

	public List<MissionObjectBase> GetRescuables ()
	{
		List<MissionObjectBase> missionObjects = new List<MissionObjectBase> ();
		if ((ConfigLoader.instance != null) && ConfigLoader.instance.activeLevel != null) {
			//all game objects in the level
			List<GameObject> gos = ConfigLoader.instance.activeLevel.levelElements;
			
			foreach (var go in gos) {
				//all rescuable components on a gameobject
				MissionObjectBase[] mibs = go.GetComponentsInChildren<MissionObjectBase> ();
				
				//filter mission objects that are not rescuable
				missionObjects.AddRange (System.Array.FindAll<MissionObjectBase>(mibs, x=> x.rescuable == true));
			}
		}
		
		return missionObjects;
	}
	
	public void EnterSaveMode ()
	{
		if (nearestRescuable != null) {//check if above mission object
			SetState (Helistate.SAVE);
		} else if (controlType == ControlType.Kinect) {
			SetState (Helistate.IDLE);
		} else if (controlType == ControlType.Keyboard) {
			SetState (Helistate.FLY);
		}
		
	}

	public void EnterIdleMode ()
	{
		SetState (Helistate.IDLE);
	}

	public void EnterFlyMode ()
	{
		SetState (Helistate.FLY);
		
	}
	
	public void ActivateRadio ()
	{
		SetState (Helistate.IDLE);
		radio.SetRadio (true);
	}
	
	public void DeactivateRadio ()
	{
		SetState (prevState);
		radio.SetRadio (false);
	}

	public void ToggleRadio ()
	{
		//for use by keyboard
		
		radio.ToggleHud();
	
		if(radio.radioIsActive){
			SetState(Helistate.IDLE);
		}
		else{
			SetState(prevState);
		}
		/*
		Debug.Log ("Toggling radio");
		if (state == Helistate.FLY || state == Helistate.SAVE) {
			SetState (Helistate.IDLE);
			radio.SetRadio (true);
		} else if (state == Helistate.IDLE) {
			SetState (prevState);
			radio.SetRadio (false);
		}
		*/
	}

	private void SetState (Helistate aState)
	{
		
		
		if (state == Helistate.FLY && aState == Helistate.SAVE)
			camAnimation.PlayQueued (toSavePosition, QueueMode.PlayNow);
		if (state == Helistate.SAVE && aState == Helistate.FLY)
			camAnimation.PlayQueued (toPilotPosition, QueueMode.PlayNow);
		if (state != aState)
			Debug.Log ("Changing state from " + prevState + " to " + aState);
	
		prevState = state;
		state = aState;
	}
}
