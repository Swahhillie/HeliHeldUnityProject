using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class RescuePointer : MonoBehaviour {

	
	public Transform target;
	public Helicopter helicopter;
	public Transform targetOverload;

		
	void Update()
	{
		AimReticule();
	}
	void AimReticule()
	{
		if(helicopter == null) return;
		if(helicopter.nearestRescuable != null)target = helicopter.nearestRescuable.transform;
		if(targetOverload != null) target = targetOverload;
		if (target != null)
		{
			
			Vector3 dirToRescuable = target.position - helicopter.transform.position;
			dirToRescuable = helicopter.transform.InverseTransformDirection(dirToRescuable);
			
			if(helicopter.dotToNearest > helicopter.heliSettings.hoverPrecision)
				this.renderer.enabled = false;
			else
				this.renderer.enabled = true;
			
			Vector3 dir = Vector3.Normalize (dirToRescuable);
			
			// forward component
			Vector3 difForward = Vector3.Dot (dir, Vector3.forward) * Vector3.forward;
			// right component
			Vector3 difSideways = Vector3.Dot (dir, Vector3.right) * Vector3.right;
			//combine components
			Vector3 dif = Vector3.Normalize((difForward + difSideways));
			
			//dif *= heliSettings.saveReticuleRange;
			
			Debug.DrawRay (helicopter.transform.position, dif * 5, Color.yellow);
			
			//saveReticle.localPosition = dif;
			transform.localRotation = Quaternion.LookRotation(dif);
		}
	}
	
}
