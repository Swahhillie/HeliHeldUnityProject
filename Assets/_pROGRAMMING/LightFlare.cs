using UnityEngine;
using System.Collections;

public class LightFlare : MonoBehaviour {

	public float repeatCd = 10.0f;
	public float lastFire = 0;
	public float force = 10.0f;
	public float straightUpFactor = 5.0f;
	public ForceMode forceMode = ForceMode.Impulse;
	public Rigidbody effect;
	
	private bool _activated = true;
	
	
	protected void Update()
	{
		
		
		if(_activated){
			float timeRemaining = lastFire + repeatCd - Time.time;
			if(timeRemaining < 0)
			{
				FireBeacon();
			}
			else
			{
				 // temp
				 trailRenderer.time = timeRemaining;
				
			}
		}
		
		
	}
	
	private void FireBeacon()
	{
		Vector3 forceDir = Vector3.Normalize(Random.onUnitSphere + Vector3.up * straightUpFactor);
		effect.transform.position = this.transform.position;
		effect.rigidbody.velocity = Vector3.zero;
		effect.rigidbody.AddForce(forceDir * force, forceMode);
		lastFire = Time.time;
		trailRenderer.time = 0;
		
		
	}
	private TrailRenderer _tr = null;
	public TrailRenderer trailRenderer{
		get{
			if(_tr == null)
			{
				_tr = gameObject.GetComponentInChildren<TrailRenderer>();

			}
			return _tr;
		}
	}
}
