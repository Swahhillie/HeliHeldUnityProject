using UnityEngine;
using System.Collections;

public class wave : MonoBehaviour 
{
	public float deltaRot;
	public float speed;
	public float verticalMovement;
	private float offset;
	private Vector3 position;
	
	public Vector2 length;
	public Vector2 direction;

	void Awake()
	{
		position = this.transform.position;
		CalcOffset();
	}

	
	void CalcOffset()
	{
		length = new Vector2(0.4f,0.4f);
		direction = new Vector2(1f,1f);
		offset=0;
		if(length.x!=0)
		{
			offset += (this.gameObject.transform.position.x/(length.x/1000))*direction.x;
		}
		if(length.y!=0)
		{
			offset += (this.gameObject.transform.position.z/(length.y/1000))*direction.y;
		}
		offset*=length.magnitude;
	}
	
	
	void Update () 
	{
		if(position!=this.transform.position)
		{
			CalcOffset();	
		}
		deltaRot=10;
		speed =2;
		verticalMovement=1;
		this.gameObject.transform.localPosition = new Vector3(0,Mathf.Cos(((Time.time*speed)-offset)*verticalMovement),0);
		//this.gameObject.transform.localEulerAngles = (new Vector3(0,0,Mathf.Sin(((Time.time*speed)-offset)*deltaRot)));
		//Rotation has to be fixed
	}
}


