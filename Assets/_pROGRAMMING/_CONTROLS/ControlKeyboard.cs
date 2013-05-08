using UnityEngine;
using System.Collections;

public class ControlKeyboard : ControlBase
{
	void Update()
	{
		Vector3 dir = Vector3.zero;
		
		float steer = Input.GetAxis("Horizontal"); // a && d, left and right
		dir.z = Input.GetAxis("Vertical"); // forward and backwwawrds
		dir = dir.magnitude > .3 ? Vector3.Normalize(dir) : Vector3.zero;
		
		heli.Steer(steer);
		heli.Accelerate(dir);
	}
}
