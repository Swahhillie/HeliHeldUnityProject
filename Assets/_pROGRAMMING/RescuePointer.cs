using UnityEngine;
using System.Collections;

public class RescuePointer : MonoBehaviour {

	
	public Transform startPos;
	public Transform endPos;
	public Transform pointer;
	
	public float min = .6;
	public float max = 1.0f;
	public float alpha;
	
	// Update is called once per frame
	void Update () {

		pointer.transform.position = Vector3.Lerp(startPos.position, endPos.position, a);
	}
}
