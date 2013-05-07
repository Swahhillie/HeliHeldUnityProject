using UnityEngine;
using System.Collections;

public class Storm : MonoBehaviour {

	public Vector3 Position = new Vector3(0,500,0);
	public float Range = 500;
	public float Strength = 0.5f; 
	
	// Update is called once per frame
	void Update () 
	{
		if(Random.value<Strength)
		{
			GameObject light = new GameObject();
			light.transform.parent = this.transform;
			light.name="Lightning";
			light.transform.position=new Vector3(Position.x+Random.value*Range,Position.y,Position.z+Random.value*Range);
			light.AddComponent<Lightning>();
		}
	}
}
