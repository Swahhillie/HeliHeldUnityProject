using UnityEngine;
using System.Collections;

public class OverlayScript : MonoBehaviour {

	// Use this for initialization
	public float rotationSpeed = 360; //in degrees per second
	
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		transform.Rotate(new Vector3(0,1,0) * rotationSpeed * Time.deltaTime);
	}
}
