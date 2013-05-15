using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Helicopter : MonoBehaviour
{
	
	public SkeletonWrapper skelWrap;
	public float maxSpeed = 2.0f;
	public float acceleration = 0.2f; // acceleration per second
	public float drag = .1f; // fraction of the velocity that is lost every second
	public float waterHeight = 20.0f; // height of the water. it has no collider so it cannot be raycast
	public float flyHeightMax = 40.0f; // maximum altitude of the helicopter. later used to stop the helicopter from flying over very high mountains
	public float avoidRadius = 20.0f; //Repulsor bubble around the helicopter
	public float rotationSpeed = 360.0f; // 360 = 1 rotation per second
	public float decendRate = .2f;
	public float leanSpeed = -25.0f; //degrees the helicopter tilts per second
	public float rightSpeed = .6f; //how many seconds it takes for the helicopter to completly right itself after rotation
	//public float raiseSpeed= .1f; // should be applied to normalized direction vector, so loww

	public float hoverPrecision = .70f; // determines where the castaway must be in relation to the helicopter. 1 == Directly below, no margin for error, 0 == anywhere below the helicopter, -1 == Anywhere.
	public LayerMask terrainLayer;
	public float rescueRadius = 50.0f; //max distance an object can be away from the helicopter for rescueing
	
	private Vector3 velocity;
	public bool debugLines = true;
	private Radio radio;
	
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
	private enum ControlType{
		None,
		Kinect,
		Keyboard
	}
	private ControlType controlType = ControlType.None;
	
	public void Start ()
	{
		
		radio = GetComponentInChildren<Radio>();
		prevState = Helistate.FLY;
		state = Helistate.IDLE;
		
		ControlBase cb = null;
		
		if (skelWrap.devOrEmu.device.connected) {
			cb = gameObject.AddComponent<ControlKinect> ();
			cb.skelWrap = skelWrap;
			controlType = ControlType.Kinect;
		} else {
			cb = gameObject.AddComponent<ControlKeyboard> ();
			controlType = ControlType.Keyboard;
		}
		
		cb.heli = this; //give controller a reference to this for controls
		
		if (rescueRadius < avoidRadius) {
			Debug.LogWarning (@"Helicopter avoid radius is bigger than rescue radius, 
			Objects on terrain will not get rescued because the helicopter will fly to high");
		}
		if(controlType == ControlType.Keyboard)state = Helistate.FLY;
	}
	
	public float closestPoint = 0;
	public float distanceToWater = 0;
	
	//should work now. try it, so long as poll skelleton is false
	public void Steer (float x)
	{
		if (state != Helistate.FLY)
			return;
		
		transform.Rotate (Vector3.up, x * rotationSpeed * Time.deltaTime); //rotate over the y axis
		
		Quaternion goalRotation = Quaternion.AngleAxis (x * leanSpeed * Time.deltaTime, transform.forward) * transform.rotation; //lean over to a side
		transform.rotation = Quaternion.Slerp (goalRotation, Quaternion.FromToRotation (transform.up, Vector3.up) * transform.rotation, Time.deltaTime * rightSpeed);// bring upright overtime
	}

	public void Accelerate (Vector3 direction)
	{
		if (state != Helistate.FLY)
			return;
		
		direction = transform.TransformDirection (direction); // make the direction in local space
		
		RaycastHit[] hits = Physics.SphereCastAll (transform.position, avoidRadius, Vector3.down, avoidRadius, terrainLayer.value);
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
		if (distanceToWater < avoidRadius) {//avoid water first
			if (debugLines)
				Debug.DrawLine (transform.position, transform.position - new Vector3 (0, distanceToWater, 0), Color.magenta);
			direction += Vector3.up; //water is always below the helicopter
			closestPoint = distanceToWater;
		} else if (closestHit.distance < avoidRadius && hits.Length > 0) { // if there are hits below helicopter, move away from those hits
			direction += Vector3.Normalize (transform.position - closestHit.point); //directly away from closest hit
			closestPoint = closestHit.distance;
		} else {
			//nothing below the helicopter to avoid, start dropping a little
			direction += Vector3.down * decendRate;	
		}
		
		
		
		direction = Vector3.Normalize (direction);
		velocity += acceleration * Time.deltaTime * direction;
		
		
		velocity = Vector3.ClampMagnitude (velocity, maxSpeed);
	}

	private void OnDrawGizmos ()
	{
		Gizmos.color = Color.cyan;
		if (debugLines)
			Gizmos.DrawWireSphere (transform.position, rescueRadius);
	}

	public bool AttemptRescue ()
	{
	
		bool rescueSuccess = false;
		string rescueReport = "Heli attempting a rescue:";
		
		if ((ConfigLoader.instance == null) || ConfigLoader.instance.activeLevel == null) {
			Debug.LogError ("Making a rescue attempt without having loaded a level");
			return false;
		}
		
		if(dotToNearest > hoverPrecision)
		{
			return (nearestRescuable.AttemptRescue());
		}
		//getting all the objects that have mission object base attached
		
		
		
		return rescueSuccess;
		
		
		
	}

	bool FindNearestInRange<T> (List<T> toSearch, Vector3 fromPos, float radius, out T result) where T : Component
	{
		result = null;
		float closest = rescueRadius * rescueRadius;
		foreach (var mib in toSearch) {
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
		velocity *= 1 - drag * Time.deltaTime;
		
		if (this.GetComponent<ControlKinect> ())
			skelWrap.pollSkeleton ();
			
		hoverPrecision = Mathf.Clamp (hoverPrecision, -1.0f, 1.0f);
		
		if(state == Helistate.SAVE)
		{
			
			
		
			
			if (FindNearestInRange <MissionObjectBase>(GetRescuables(), transform.position, rescueRadius, out nearestRescuable)) {
				dotToNearest = GetDotToRescuable(nearestRescuable.transform);
			}
			if(Input.GetKeyDown(KeyCode.Alpha2))
			{
				AttemptRescue();
			}
		}
	}
	public List<MissionObjectBase> GetRescuables()
	{
		List<MissionObjectBase> missionObjects = new List<MissionObjectBase> ();
		if((ConfigLoader.instance != null) && ConfigLoader.instance.activeLevel != null){
			foreach (var go in ConfigLoader.instance.activeLevel.levelElements) {
				missionObjects.AddRange (go.GetComponentsInChildren<MissionObjectBase> ());
			}
		}
		
		return missionObjects;
	}
	
	public void EnterSaveMode ()
	{
		MissionObjectBase mib = null;
		if (FindNearestInRange<MissionObjectBase>(GetRescuables(), transform.position, rescueRadius, out mib))//check if above mission object
			SetState (Helistate.SAVE);
		else if(controlType == ControlType.Kinect)
		{
			SetState (Helistate.IDLE);
		}
		else if(controlType == ControlType.Keyboard)
		{
			SetState (Helistate.FLY);
		}
		
	}
	public void EnterIdleMode()
	{
		SetState(Helistate.IDLE);
	}
	public void EnterFlyMode ()
	{
		SetState (Helistate.FLY);
		
	}
	
	public void ActivateRadio ()
	{
		SetState (Helistate.IDLE);
		radio.SetRadio(true);
	}
	
	public void DeactivateRadio ()
	{
		SetState (prevState);
		radio.SetRadio(false);
	}
	public void ToggleRadio()
	{
		//for use by keyboard
		Debug.Log("Toggling radio");
		if(state == Helistate.FLY || state == Helistate.SAVE)
		{
			SetState(Helistate.IDLE);
			radio.SetRadio(true);
		}
		else if(state == Helistate.IDLE)
		{
			SetState(prevState);
			radio.SetRadio(false);
		}
	}
	
	private void SetState (Helistate aState)
	{
		prevState = state;
		state = aState;
	}
}
