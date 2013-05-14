using UnityEngine;
using System.Collections;

public class ControlKeyboard : ControlBase
{
	public float forceThresshold = .3f;
	void Update()
	{
		Vector3 dir = Vector3.zero;
		
		float steer = Input.GetAxis("Horizontal"); // a && d, left and right
		dir.z = Input.GetAxis("Vertical"); // forward and backwwawrds
		dir = dir.magnitude > forceThresshold ? Vector3.Normalize(dir) : Vector3.zero;
		
		heli.Steer(steer);
		heli.Accelerate(dir);
		
		if(Input.GetKeyDown(KeyCode.Alpha1))
			heli.ActivateRadio();
		else if(Input.GetKeyDown(KeyCode.Alpha2))
			heli.DeactivateRadio();
		
		cursorPosition = Input.mousePosition;
	}
}
