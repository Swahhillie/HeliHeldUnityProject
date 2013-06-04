using UnityEngine;
using System.Collections;

public class ControlKeyboard : ControlBase
{
	public float forceThresshold = .3f;
	
	public KeyCode liftKey = KeyCode.F;
	public KeyCode saveKey = KeyCode.R;
	public KeyCode radioKey = KeyCode.Alpha2;
	void Update()
	{
		if(Input.GetKeyDown(liftKey))
		{
			heli.EnterFlyMode();
		}
		else if(Input.GetKeyDown(saveKey))
		{
			heli.EnterSaveMode();
		}
		else if(Input.GetKeyDown(radioKey))
		{
			heli.ToggleRadio();
		}
		Vector3 dir = Vector3.zero;
		
		float steer = Input.GetAxis("Horizontal"); // a && d, left and right
		dir.z = Input.GetAxis("Vertical"); // forward and backwwawrds
		//dir = dir.magnitude > forceThresshold ? Vector3.Normalize(dir) : Vector3.zero;
		
		heli.Steer(steer);
		heli.Accelerate(dir);
		
		
		cursorPosition = Input.mousePosition;
	}
}
