using UnityEngine;
using System.Collections;

public class gridObject
{
	public int id;
	public string name;
	public Vector3 position;
	public int radius;
	
	// Use this for initialization
	void Start ()
	{
		id = 0;
		name = "";
		position = new Vector3(0,0,0);
		radius = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
