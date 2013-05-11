using UnityEngine;
using System.Collections;

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
	
	private Vector3 velocity;
	
	void Awake ()
	{
		ControlBase cb = null;
		try {
			if (skelWrap.pollSkeleton ()) {
				cb = gameObject.AddComponent<ControlKinect> ();
			}
		} catch {
			cb = gameObject.AddComponent<ControlKeyboard> ();
		}
		
		cb.heli = this; //give controller a reference to this for controls
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
			Debug.DrawLine (transform.position, closestHit.point, Color.red);
			closestPoint = closestHit.distance;
		}
			
		distanceToWater = transform.position.y - waterHeight;
		if (distanceToWater < avoidRadius) {//avoid water first
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

	public LayerMask terrainLayer;

	void Update ()
	{
		Debug.DrawRay (transform.position, velocity, Color.cyan);
		transform.position += velocity * Time.deltaTime; //misschien niet de deltatime hier maar in de controller. waarschijn lijk wel though
		velocity *= 1 - drag * Time.deltaTime;
		
	}

}
