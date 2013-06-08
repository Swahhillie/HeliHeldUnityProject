using UnityEngine;
using System.Collections;

/// <summary>
/// The storm class creates the lightnings in the scene.
/// It has a position and a range to create the lightnings in a specific area.
/// </summary>


public class Storm : MonoBehaviour {

	public Vector3 position = new Vector3(0,500,0);
	public float range = 500;
	public float delay = 1;
	public float randomizer=0;
	
	
	private float _startTime=0;
	private float _timer=0;
	
	/// <summary>
	/// Update this instance.
	/// Calls the CreateLighting-function after an amount of time.
	/// It resets the timer to a fixed delay and random value.
	/// </summary>
	void Update () 
	{
		if(_startTime+Time.time>_timer)
		{
			_timer = Time.time+delay+(Random.value*randomizer);
			CreateLightning();
		}
	}
	
	/// <summary>
	/// Creates the lightning at a random position in a range.
	/// </summary>
	
	void CreateLightning()
	{
		Vector3 rndPos = Random.insideUnitSphere;
		GameObject light = new GameObject();
		light.transform.parent = this.transform;
		light.name="Lightning";
		light.transform.position= new Vector3(position.x+(rndPos.x*(range/2)),position.y,position.z+(rndPos.x*(range/2)));
		light.AddComponent<Lightning>();
	}
}
