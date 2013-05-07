using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour
{
	Transform mainCam;
	Transform stick;
	GameObject IRCam;
	GameObject fuelMeter;
	
	private KinectMouse mouse;
	
	public float fuel;
	
	bool drawCastawayGUI;
	
	enum State { 
		IDLE,
		LIFT,
		FLY,
		SWITCH_SAVE,
		SAVE,
		SWITCH_FLY 
	};
	
	State state;
	
	Vector3 rot;
	float flyHeight;
	float flyHeightMax;
	
	public SkeletonWrapper skelWrap;
	public GUIStyle GUIstyle;
	
	float speed;
	
	float kispeed;
	float kirot;
	
	float spotCounter;
	
	bool UsingKinect;
	
	void Start ()
	{
		stick = transform.FindChild("ControllerStick");
		mainCam = transform.FindChild("Camera");
		IRCam = GameObject.Find("InfraredCam");
		fuelMeter = GameObject.Find("FuelMeter");
		
		mouse = GameObject.Find("Mouse").GetComponent<KinectMouse>();
		
		mainCam.animation["SwitchSave2"].wrapMode = WrapMode.Once;
		mainCam.animation["SwitchPilot"].wrapMode = WrapMode.Once;
		
		state = State.IDLE;
		
		rot = new Vector3(5,0,0);
		flyHeight = 36.0f;
		flyHeightMax = 100;
		speed = 4.0f;
		
		drawCastawayGUI = false;
		
		spotCounter = 0;
		
		UsingKinect = false;
		
		fuel = 100;
	}
	
	void Update ()
	{
		if(skelWrap.pollSkeleton())
			UsingKinect = true;
		
		this.transform.position = new Vector3(transform.position.x, flyHeight, transform.position.z);
		this.transform.rotation = Quaternion.Euler(rot);
		
		stick.localRotation = Quaternion.Euler(new Vector3(rot.z*0.5f,270,-rot.x*0.5f));
		
		//Controls();
		switch(state)
		{
			case State.IDLE :
			{
				StartCoroutine(Wait(5.0f));
				break;
			}
			case State.LIFT :
			{
				liftUp();
				break;
			}
			case State.FLY :
			{
				if(fuel > 0)
					fuel -= 0.01f;
				else
					Application.LoadLevel(Application.loadedLevel);
				fuelMeter.transform.localScale = new Vector3(1.0f, (fuel*0.01f), 1.0f);
				if(UsingKinect)
					KinectControls();
				else
					Controls();
				break;
			}
			case State.SAVE :
			{
				saveControls();
				break;
			}
			
			case State.SWITCH_SAVE:
			{
				mainCam.animation.Play("SwitchSave2");
				state = State.SAVE;
				break;
			}
			
			case State.SWITCH_FLY:
			{
				mainCam.animation.Play("SwitchPilot");
				state = State.FLY;
				break;
			}
		}
	}
	
	void setSaveMode(bool mode)
	{
		if(mode==true && state == State.FLY)
		{
			state=State.SWITCH_SAVE;
		}
		else if(state == State.SAVE)
		{
			state=State.SWITCH_FLY;
		}
	}
	private const int PLAYER_0 = 0;
	void saveControls()
	{
		leveling();
		if(Input.GetKey(KeyCode.Return) && !UsingKinect)
		{
			setSaveMode(false);
		}
		if(UsingKinect)
		{
			float dist = (skelWrap.bonePos[PLAYER_0, 11]-skelWrap.bonePos[0, 7]).magnitude;
			if(dist < 0.3f)
			{
				setSaveMode(false);	
			}
		}
		
		float aX = (this.transform.position.x - (IRCam.camera.GetScreenHeight()/2)*0.2f) + mouse.position.y*0.2f; //dafuq?
		float aZ = (this.transform.position.z + (IRCam.camera.GetScreenWidth()/2)*0.1f) - mouse.position.x*0.1f;
		
		IRCam.transform.position = new Vector3(aX, this.transform.position.y-2, aZ);
		
		Ray ray = new Ray(IRCam.transform.position,Vector3.down);
		RaycastHit hit;
		
		Debug.DrawRay(ray.origin, ray.direction*1000, Color.red);
		
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
		{
			Debug.DrawLine (ray.origin, hit.point);
			if(hit.collider.name == "castaway")
			{
				drawCastawayGUI = true;
				spotCounter++;
				if(spotCounter > 30)// ?????????? 30 frames?? thats a shitty way  to track this
				{
					Destroy(hit.collider.gameObject);
					Castaway script = (Castaway)hit.collider.transform.parent.GetComponent(typeof(Castaway));// you just destroyed it and now you are accessing its parent. very unsafe
//					script.SetSaved();
					spotCounter = 0;
					drawCastawayGUI = false;
				}
			}
			else
			{
				spotCounter=0;
				drawCastawayGUI = false;
			}
		}
	}
	
	void OnGUI ()
	{
		
	}
	
	void KinectControls()
	{
	//MAGICAL NUMBERS EVERYWHERE!
		//kinect hands check
		float dist = (skelWrap.bonePos[0, 11]-skelWrap.bonePos[0, 7]).magnitude;
		if(dist < 0.3f)
		{
			kispeed = (skelWrap.bonePos[0, 11].y - skelWrap.bonePos[0,2].y) + 0.2f;
			kispeed *= -15.0f;
			
			kirot = (skelWrap.bonePos[0,11].x - skelWrap.bonePos[0,2].x);
			kirot *= 3.0f;
			
			//rotate
			if(rot.x+kispeed*0.6f < 20 && rot.x+kispeed*0.6f > -10)
				rot.x += kispeed*0.6f;
			
			//moving with ki-vars
			this.transform.Translate(0,0,kispeed);
			
			rot.y += kirot;
				
			if(rot.z > -45 && rot.z < 45)
				rot.z -= kirot;
		}
		else if(state == State.FLY)
		{
			setSaveMode(true);
			kispeed = 0;
			kirot = 0;
		}
		else
		{
			kispeed = 0;
			kirot = 0;
		}
		
		leveling();
	}
	
	void leveling()
	{
		if(rot.z > 0)
			rot.z -= 0.4f;
		if(rot.z < 0)
			rot.z += 0.4f;
	}
	
	
	void Controls()
	{
		if(Input.GetKey("space") && state == State.FLY)
		{
			setSaveMode(true);
		}
		
		if(Input.GetKey("left") && state == State.FLY)
		{
			turnLeft();
		}
		
		if(Input.GetKey("right") && state == State.FLY)
		{
			turnRight();
		}
		
		if(Input.GetKey("up") && state == State.FLY)
		{
			moveForward();
		}
		
		if(Input.GetKey("down") && state == State.FLY)
		{
			moveBackward();
		}
		
		controlsCheck();
	}
	
	void turnLeft()
	{
		rot.y -= 0.8f;
			
		if(rot.z < 40)
		{
			rot.z += 0.5f;
		}
	}
	
	void turnRight()
	{
		rot.y += 0.8f;
			
		if(rot.z > -40)
		{
			rot.z -= 0.5f;
		}
	}
	
	void moveForward()
	{
		if(rot.x < 25)
			rot.x += 0.4f;
		
		this.transform.Translate(0,0,speed);
	}
	
	void moveBackward()
	{
		if(rot.x > -5)
			rot.x -= 0.4f;
		
		this.transform.Translate(0,0,-(speed*0.5f));//magic *.5?
	}
	
	void controlsCheck()
	{
	//THE MAGIC, this is not framerate independent
		if(!Input.GetKey("left") && !Input.GetKey("right"))
		{
			if(rot.z > 0)
				rot.z -= 0.45f;
			if(rot.z < 0)
				rot.z += 0.45f;
		}
		
		if(!Input.GetKey("up") && !Input.GetKey("down"))
		{
			if(rot.x > 10)
				rot.x -= 0.6f;
			if(rot.x < 10)
				rot.x += 0.6f;
		}
	}
	
	void liftUp()
	{
		flyHeight += 0.8f;
		if(flyHeight > flyHeightMax)
		{
			flyHeight = flyHeightMax;
			state = State.FLY;
		}
	}
	
	IEnumerator Wait(float duration)
    {
        yield return new WaitForSeconds(duration);
		state = State.LIFT;
    }
}
