using UnityEngine;
using System.Collections;

public class DirectionArrow : MonoBehaviour
{
	public GameObject target;
	
	private GameObject infraredCam;
	
	void Start ()
	{
		target = GameObject.Find("LightHouse");
		infraredCam = GameObject.Find("InfraredCam");
	}
	
	void Update ()
	{
		/*Vector2 targetPos = new Vector2(target.transform.position.x, target.transform.position.z);
		Vector2 infraredPos = new Vector2(infraredCam.transform.position.x, infraredCam.transform.position.z);
		
		float dotDifference = Vector2.Dot(targetPos, infraredPos);
		
		float result = dotDifference / targetPos.magnitude * infraredPos.magnitude;
		
		this.transform.localEulerAngles = new Vector3(0, result, 0);*/
		
		Vector3 targetPos = infraredCam.camera.WorldToScreenPoint(target.transform.position);
		Vector3 infraredPos = infraredCam.camera.WorldToScreenPoint(infraredCam.transform.position);
		Vector3 difference = Vector3.Normalize(infraredPos-targetPos);
		
		this.transform.localEulerAngles = new Vector3(0, difference.y, 0);
	}
}
