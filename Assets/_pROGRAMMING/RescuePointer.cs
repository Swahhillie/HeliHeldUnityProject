using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class RescuePointer : MonoBehaviour {

	
	public Transform startPos;
	public Transform endPos;
	public Transform pointer;
	
	public float min = .6f;
	public float max = 1.0f;
	public float alpha;
	public float result;
	// Update is called once per frame
	void Update () {
		
		float a = (alpha - min) * (max /(max - min));
		result = a;
		pointer.transform.position = Vector3.Lerp(startPos.position, endPos.position, a);
		Debug.DrawLine(startPos.position, endPos.position, Color.magenta);
	}
	
}
