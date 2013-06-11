using UnityEngine;
using System.Collections;

public class CloudBehavior : MonoBehaviour
{
	GameObject player;
	//List<GameObject> clouds;
	public float speed;
	
	void Start ()
	{
	player = GameObject.Find("Player");
//		clouds = new List<GameObject>();
//		clouds.Add(transform.FindChild("Sphere1"));
//		clouds.Add(transform.FindChild("Sphere2"));
//		clouds.Add(transform.FindChild("Sphere3"));
	}
	
	void Update ()
	{
		this.transform.position = player.transform.position;
		transform.Rotate(speed * Time.deltaTime * Vector3.up);
		
		
	}
}
