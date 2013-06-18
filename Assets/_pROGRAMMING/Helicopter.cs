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
	
	/// <summary>
	/// The range the save reticule can move over the infrared screen.
	/// </summary>
	public float saveReticuleRange = 3.0f;
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
		Fly,
		Save
	}
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
	private RescueNearbyIndicator rescueNearbyIndicator;
	public Transform joystick;
	public Transform saveReticle;
	
	public void Start ()
	{
		rescueNearbyIndicator = GetComponentInChildren<RescueNearbyIndicator> ();
		radio = (Radio)FindObjectOfType(typeof(Radio));
		InitializeControls ();
		
		if (heliSettings.rescueRadius < heliSettings.avoidRadius)
		{
			Debug.LogWarning (@"Helicopter avoid radius is bigger than rescue radius, 
			Objects on terrain will not get rescued because the helicopter will fly to high");
		}
	}
	
	private void InitializeControls ()
	{
		SetState(Helistate.Fly);
		
		// disable the keyboard if connect is there and vice versa
		if (skelWrap.devOrEmu.device.connected)
		{
			controlType = ControlType.Kinect;
			gameObject.GetComponent<ControlKeyboard> ().enabled = false;
				
		}
		else
		{
			controlType = ControlType.Keyboard;
			gameObject.GetComponent<ControlKinect> ().enabled = false;
		}
	}
	
	public float closestPoint = 0;
	public float distanceToWater = 0;

	//should work now. try it, so long as poll skelleton is false
	public void Steer (float x)
	{
		if(camAnimation.isPlaying) return;
		
		if (state == Helistate.Fly)
		{
			transform.Rotate (Vector3.up, x * heliSettings.rotationSpeed * Time.deltaTime); //rotate over the y axis
		
			Quaternion goalRotation = Quaternion.AngleAxis (x * heliSettings.leanSpeed * Time.deltaTime, transform.forward) * transform.rotation; //lean over to a side
			transform.rotation = Quaternion.Slerp (goalRotation, Quaternion.FromToRotation (transform.up, Vector3.up) * transform.rotation, Time.deltaTime * heliSettings.rightSpeed);// bring upright overtime
			//joystick.localEulerAngles = new Vector3(0, 0, joystick.localEulerAngles.z + x);
			
			float angle = 0;
			if (Mathf.Abs (x) > heliSettings.stickRotationThresshold)
			{
				angle = -x * heliSettings.stickRotationRange;
			}
			
			float joystickSpeed = 1.0f;
			Quaternion joystickGoal = Quaternion.AngleAxis (angle, Vector3.forward);
			joystick.localRotation = Quaternion.Lerp (joystick.localRotation, joystickGoal, Time.deltaTime * joystickSpeed);
		} else if (state == Helistate.Save)
		{
			Accelerate (new Vector3 (x, 0, 0));
			
		}
		
		
	}
	
	/// <summary>
	/// Accelerate the specified direction.
	/// </summary>
	/// <param name='direction'>
	/// Direction.
	/// </param>
	public void Accelerate (Vector3 direction)
	{
		if(camAnimation.isPlaying) return;
		
		direction = transform.TransformDirection (direction); // make the direction in local space
		AddAvoidForce (ref direction);
		
		if(controlType == ControlType.Keyboard)		
			direction = Vector3.Normalize (direction);
		
		velocity += heliSettings.acceleration * Time.deltaTime * direction;
		
		velocity = Vector3.ClampMagnitude (velocity, heliSettings.maxSpeed);
	}

	private void AddAvoidForce (ref Vector3 direction)
	{
		RaycastHit[] hits = Physics.SphereCastAll (transform.position, heliSettings.avoidRadius, Vector3.down, heliSettings.avoidRadius, terrainLayer.value);
		float lastDistance = Mathf.Infinity;
		RaycastHit closestHit = new RaycastHit ();// = System.Array.FindLast<RaycastHit>(hits, x => x.distance < lastDistance);
		foreach (RaycastHit h in hits)
		{
			if (h.distance < lastDistance)
			{
				closestHit = h;
				lastDistance = h.distance;
			}
		}
		//no hits below helicopter
		if (hits.Length > 0)
		{
			//k			Debug.Log(closestHit.transform.name);
			if (debugLines)
			{
				Debug.DrawLine (transform.position, closestHit.point, Color.red);
			}
			closestPoint = closestHit.distance;
		}
					
		distanceToWater = transform.position.y - waterHeight;
		if (distanceToWater < heliSettings.avoidRadius)
		{//avoid water first
			if (debugLines)
			{
				Debug.DrawLine (transform.position, transform.position - new Vector3 (0, distanceToWater, 0), Color.magenta);
			}
			direction += Vector3.up; //water is always below the helicopter
			closestPoint = distanceToWater;
		} else if (closestHit.distance < heliSettings.avoidRadius && hits.Length > 0)
		{ // if there are hits below helicopter, move away from those hits
			direction += Vector3.Normalize (transform.position - closestHit.point); //directly away from closest hit
			closestPoint = closestHit.distance;
		} else
		{
			direction += Vector3.down * heliSettings.decendRate;	
		}
			
			
	}

	private void OnDrawGizmos ()
	{
		Gizmos.color = Color.cyan;
		if (debugLines)
		{
			Gizmos.DrawWireSphere (transform.position, heliSettings.rescueRadius);
		}
	}
	
	/// <summary>
	/// Rescues the routine.
	/// </summary>
	/// <returns>
	/// The routine.
	/// </returns>
	private IEnumerator RescueRoutine ()
	{
		
		if (rescueing == true)
		{
			Debug.LogError ("Multiple rescue routines are running at the same time! Respect the guard!");
			
		} else
		{
			rescueing = true;
			float elaspedTime = 0;
			while (dotToNearest > heliSettings.hoverPrecision)
			{
				elaspedTime += Time.deltaTime;
				if (elaspedTime > rescueTime)
				{
					Debug.Log ("Rescue timer gone off");
					MakeRescue ();
					break;
				} else
				{
					yield return null;
				}
			}
			rescueing = false;
		}
		
		
	}
	
	/// <summary>
	/// Finds the nearest in range.
	/// </summary>
	bool FindNearestInRange<T> (List<T> toSearch, Vector3 fromPos, float radius, out T result) where T : Component
	{
		
		
		result = null;
		float closest = radius * radius;
		foreach (var mib in toSearch)
		{
			if (mib.gameObject.activeSelf == false)
			{
				continue;
			} //skip over disabled level object
			Transform tr = mib.transform;
			float dist = Vector3.SqrMagnitude (tr.position - fromPos);
			if (dist < closest)
			{
				closest = dist;
				result = mib;
				
			}
		}
		return result != null;
		
	}
	/// <summary>
	/// Gets the dot to rescuable.
	/// </summary>
	/// <returns>
	/// The dot to rescuable.
	/// </returns>
	/// <param name='missionObject'>
	/// Mission object.
	/// </param>
	float GetDotToRescuable (Transform missionObject)
	{
		
		
		//check if the mib is positioned withing the hoverprecision of the helicopter
		Vector3 vecToMib = Vector3.Normalize (missionObject.position - transform.position);
		return Vector3.Dot (vecToMib, Vector3.down);		

	}
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update ()
	{
		/*START TEMP*/
		if(Input.GetKeyDown(KeyCode.C))
		{
			Debug.Log("Testing animation!");
			if(state == Helistate.Fly)
			{
				state = Helistate.Save;
				camAnimation.PlayQueued (toSavePosition, QueueMode.PlayNow);
			}
			else
			{
				state = Helistate.Fly;
				camAnimation.PlayQueued (toPilotPosition, QueueMode.PlayNow);
			}
		}
		/*END TEMP*/
		
		if (debugLines)
		{
			Debug.DrawRay (transform.position, velocity, Color.cyan);
		}
		transform.position += velocity * Time.deltaTime; //misschien niet de deltatime hier maar in de controller. waarschijn lijk wel though
		
		if (state == Helistate.Save && controlType == ControlType.Kinect)
		{
			velocity = new Vector3 (0.0f, 0.0f, 0.0f);
		} else
		{
			velocity *= 1 - heliSettings.drag * Time.deltaTime;
		}
		
		if (controlType == ControlType.Kinect)
		{
			skelWrap.pollSkeleton ();
		}
			
		heliSettings.hoverPrecision = Mathf.Clamp (heliSettings.hoverPrecision, -1.0f, 1.0f);
		
		
		if (FindNearestInRange <MissionObjectBase> (GetRescuables (), transform.position, heliSettings.rescueRadius, out nearestRescuable))
		{
			dotToNearest = GetDotToRescuable (nearestRescuable.transform);
			rescueNearbyIndicator.Activate ();
		} else
		{
			rescueNearbyIndicator.DeActivate ();
			dotToNearest = 0;
		}

		
		if (state == Helistate.Save)
		{
			
			if (!rescueing)
			{
				StartCoroutine (RescueRoutine ());
			}
			
		}
		UpdateSaveRecticule ();
	}
	
	/// <summary>
	/// Updates the save recticule.
	/// </summary>
	public void UpdateSaveRecticule ()
	{
		if (nearestRescuable != null)
		{
			Vector3 dirToRescuable = nearestRescuable.transform.position - transform.position;
			
			/*if(dotToNearest > heliSettings.hoverPrecision)
				saveReticle.renderer.enabled = false;
			else
				saveReticle.renderer.enabled = true;*/
			
			Vector3 dir = Vector3.Normalize (dirToRescuable);
			
			// forward component
			Vector3 difForward = Vector3.Dot (dir, Vector3.forward) * Vector3.forward;
			// right component
			Vector3 difSideways = Vector3.Dot (dir, Vector3.right) * Vector3.right;
			//combine components
			Vector3 dif = (difForward + difSideways);
			
			//dif *= heliSettings.saveReticuleRange;
			
			Debug.DrawRay (transform.position, dif * 5, Color.yellow);
			
			//saveReticle.localPosition = dif;
			saveReticle.rotation = Quaternion.LookRotation(dif);
		}
	}
	
	/// <summary>
	/// Makes the rescue.
	/// </summary>
	public void MakeRescue ()
	{
		nearestRescuable.AttemptRescue ();
	}
	
	/// <summary>
	/// Gets the rescuables.
	/// </summary>
	/// <returns>
	/// The rescuables.
	/// </returns>
	public List<MissionObjectBase> GetRescuables ()
	{
		List<MissionObjectBase> missionObjects = new List<MissionObjectBase> ();
		if (ConfigLoader.Instance.activeLevel != null)
		{
			//all game objects in the level
			List<GameObject> gos = ConfigLoader.Instance.activeLevel.levelElements;
			
			foreach (var go in gos)
			{
				//all rescuable components on a gameobject
				MissionObjectBase[] mibs = go.GetComponentsInChildren<MissionObjectBase> ();
				
				//filter mission objects that are not rescuable
				missionObjects.AddRange (System.Array.FindAll<MissionObjectBase> (mibs, x => x.rescuable == true));
			}
		}
		
		return missionObjects;
	}
	/// <summary>
	/// Enters the save mode.
	/// </summary>
	public void EnterSaveMode ()
	{
		if ( nearestRescuable != null )
		{//check if above mission object
			SetState (Helistate.Save);
		}
	}
	/// <summary>
	/// Enters the fly mode.
	/// </summary>
	public void EnterFlyMode ()
	{
		SetState (Helistate.Fly);
		
	}
	/// <summary>
	/// Determines whether this instance is saving.
	/// </summary>
	/// <returns>
	/// <c>true</c> if this instance is saving; otherwise, <c>false</c>.
	/// </returns>
	public bool IsSaving()
	{
		return state == Helistate.Save;
	}
	/// <summary>
	/// Determines whether this instance is flying.
	/// </summary>
	/// <returns>
	/// <c>true</c> if this instance is flying; otherwise, <c>false</c>.
	/// </returns>
	public bool IsFlying()
	{
		return state == Helistate.Fly;
	}
	/// <summary>
	/// Activates the radio.
	/// </summary>
	public void ActivateRadio ()
	{
		if(!radio.radioIsActive)
			radio.SetRadio (true);
	}
	
	public void DeactivateRadio ()
	{
		if(radio.radioIsActive)
			radio.SetRadio (false);
	}
	
	public void ToggleRadio ()
	{
		radio.ToggleHud ();
	}
	
	public void GiveExitWarning()
	{
		radio.Warning(true,"Weet je zeker dat je wilt stoppen?");
	}
	
	/// <summary>
	/// Sets the state.
	/// </summary>
	/// <param name='aState'>
	/// A state.
	/// </param>
	private void SetState (Helistate aState)
	{
		if (state == Helistate.Fly && aState == Helistate.Save)
		{
			camAnimation.PlayQueued (toSavePosition, QueueMode.PlayNow);
		}
		if (state == Helistate.Save && aState == Helistate.Fly)
		{
			camAnimation.PlayQueued (toPilotPosition, QueueMode.PlayNow);
		}
		
		state = aState;
	}
}