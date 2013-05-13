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
	void Awake ()
	{
		ControlBase cb = null;
		
		if(skelWrap.devOrEmu.device.connected)
		{
			cb = gameObject.AddComponent<ControlKinect>();
			cb.skelWrap = skelWrap;
		}
		else
		{
			cb = gameObject.AddComponent<ControlKeyboard>();
		}
		
		cb.heli = this; //give controller a reference to this for controls
	}
	public void Start()
	{
		if(rescueRadius < avoidRadius)
		{
			Debug.LogWarning(@"Helicopter avoid radius is bigger than rescue radius, 
			Objects on terrain will not get rescued because the helicopter will fly to high");
		}
	}
	public float closestPoint = 0;
	public float distanceToWater = 0;
	
	//should work now. try it, so long as poll skelleton is false
	public void Steer (float x)
	{
		transform.Rotate (Vector3.up, x * rotationSpeed * Time.deltaTime); //rotate over the y axis
		
		Quaternion goalRotation = Quaternion.AngleAxis (x * leanSpeed * Time.deltaTime, transform.forward) * transform.rotation; //lean over to a side
		transform.rotation = Quaternion.Slerp (goalRotation, Quaternion.FromToRotation (transform.up, Vector3.up) * transform.rotation, Time.deltaTime * rightSpeed);// bring upright overtime
	}

	public void Accelerate (Vector3 direction)
	{
		
		
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
			if(debugLines)Debug.DrawLine (transform.position, closestHit.point, Color.red);
			closestPoint = closestHit.distance;
		}
			
		distanceToWater = transform.position.y - waterHeight;
		if (distanceToWater < avoidRadius) {//avoid water first
			if(debugLines)Debug.DrawLine (transform.position, transform.position - new Vector3 (0, distanceToWater, 0), Color.magenta);
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

	private void OnDrawGizmos(){
		Gizmos.color = Color.cyan;
		if(debugLines)Gizmos.DrawWireSphere(transform.position, rescueRadius);
	}
	public bool AttemptRescue(){
	
		bool rescueSuccess = false;
		string rescueReport = "Heli attempting a rescue:";
		if((ConfigLoader.instance == null ) || ConfigLoader.instance.activeLevel == null){
			Debug.LogError("Making a rescue attempt without having loaded a level");
			return false;
		}
		
		//getting all the objects that have mission object base attached
		List<MissionObjectBase> missionObjects = new List<MissionObjectBase>();
		foreach(var go in ConfigLoader.instance.activeLevel.levelElements)
		{
			missionObjects.AddRange(go.GetComponentsInChildren<MissionObjectBase>());
		}
		
		if(missionObjects.Count > 0){
		//finding wich objects are in range
			float closest = Mathf.Infinity;
			MissionObjectBase closestMib = null;
			foreach(var mib in missionObjects)
			{
				float dist = Vector3.SqrMagnitude(mib.transform.position - this.transform.position);
				if(dist  < closest)
				{
					closest = dist;
					closestMib = mib;
					
				}
			}
			//check the max radius
			
			if(closest < rescueRadius * rescueRadius)//check if the mib is within the rescueRadius
			{
				
				
				
				//check if the mib is positioned withing the hoverprecision of the helicopter
				Vector3 vecToMib = Vector3.Normalize( closestMib.transform.position - transform.position);
				float d = Vector3.Dot(vecToMib, Vector3.down);
			
				if(d > hoverPrecision)
				{
					//the mib is in rescue zone of the helicopter
					if(debugLines)Debug.DrawLine(transform.position, closestMib.transform.position, Color.green);
					
					//the mission object is given a chance to fail the rescue;
					rescueReport += "Helicopter side = success";
					rescueSuccess = closestMib.AttemptRescue();
					rescueReport += ", Mib side = " + (rescueSuccess? "success " : "failed ");
					
				}
				else
				{
					// the mib is not in the rescue zone of the helicopter, it is too far to a side
					if(debugLines)Debug.DrawLine(transform.position, closestMib.transform.position, Color.red);	
					rescueReport += "Rescue failed (The mission object is not in the correct position, hoverprecision)";
				}
				
				
			}
			else
			{
				rescueReport += "Rescue failed (There are no rescuables in range, rescueRadius)";
			}
			
		}
		else{
			rescueReport += "Rescue failed (There are no rescuables)";
		}
		Debug.Log(rescueReport);
		 return rescueSuccess;
		
	}
	void Update ()
	{
		if(debugLines)Debug.DrawRay (transform.position, velocity, Color.cyan);
		transform.position += velocity * Time.deltaTime; //misschien niet de deltatime hier maar in de controller. waarschijn lijk wel though
		velocity *= 1 - drag * Time.deltaTime;
		
		if(this.GetComponent<ControlKinect>())
			skelWrap.pollSkeleton();
			
		hoverPrecision = Mathf.Clamp(hoverPrecision, -1.0f, 1.0f);
		if(Input.GetKeyDown(KeyCode.Alpha2))AttemptRescue();
	}

}
