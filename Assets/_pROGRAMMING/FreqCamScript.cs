using UnityEngine;
using System.Collections;

public class FreqCamScript : MonoBehaviour {

	GameObject mainCam;
	// Use this for initialization
	void Start () {
		mainCam = GameObject.Find("Camera");
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = mainCam.transform.position;
		this.transform.rotation = Quaternion.Euler(new Vector3(90,180+mainCam.transform.rotation.eulerAngles.y,0));
	}
}
