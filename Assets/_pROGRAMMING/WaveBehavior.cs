using UnityEngine;
using System.Collections;
/// <summary>
/// The WaveBehavior simulates the object on a wave.
/// </summary>


public class WaveBehavior: MonoBehaviour 
{
	private float deltaRot=0;
	private float speed=0;
	private float verticalMovement=0;
	private float offset=0;
	private Vector3 position;
	
	private Vector2 length;
	private Vector2 direction;
	
	/// <summary>
	/// Saves the position of the Object and calls the getWaveInformation-function 
	/// after that it calls the CalcOffset-function.
	/// </summary>
	
	void Awake()
	{
		position = this.transform.position;
		getWaveInformation();
		CalcOffset();
	}
	/// <summary>
	/// Gets the wave information form the temporary wave class.
	/// It will be in the levelinformation later.
	/// </summary>
	void getWaveInformation()
	{
		//has to be changed in final version!
		
		Wave wave = GameObject.Find("Wave").GetComponent<Wave>();
		deltaRot = wave.deltaRot;
		speed = wave.speed;
		verticalMovement = wave.verticalMovement;
		length = wave.length;
		direction = wave.direction;
		
		direction.Normalize();
	}
	
	/// <summary>
	/// Calculates the offset depending on the object's position and the wave length and direction.
	/// </summary>
	void CalcOffset()
	{
		offset=0;
		if(length.x!=0)
		{
			offset += (this.gameObject.transform.position.x/length.x)*direction.x;
		}
		if(length.y!=0)
		{
			offset += (this.gameObject.transform.position.z/length.y)*direction.y;
		}
		offset*=length.magnitude;
	}
	
	/// <summary>
	/// Calculates a new offset if the objects position has changed in the meantime.
	/// Sets the object to the correct height and rotation it should have on the wave.
	/// </summary>
	void Update () 
	{
		if(position!=this.transform.position)
		{
			CalcOffset();	
		}
		this.gameObject.transform.localPosition = new Vector3(0,Mathf.Cos((Time.time*speed)-offset)*verticalMovement,0);
		this.gameObject.transform.localEulerAngles = (new Vector3(Mathf.Sin(Time.time*speed-offset)*deltaRot*direction.x,0,Mathf.Sin(Time.time*speed-offset)*deltaRot*direction.y));
	}
}


