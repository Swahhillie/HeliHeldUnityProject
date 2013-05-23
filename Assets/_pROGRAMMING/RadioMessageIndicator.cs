using UnityEngine;
using System.Collections;

public class RadioMessageIndicator : MonoBehaviour {

    public float speed=1;
    private bool _active=false;
    private Material mat;
	private float startTime=0;
	
	public bool setActive
	{
		get{return _active;}
		set{
			_active = value;
			if(_active)
			{
				startTime=Time.time;
			}
			else
			{
				mat.color = Color.white;	
			}
		}
	}
	void Awake()
	{
		mat = (Material)Instantiate(renderer.material);
		renderer.material = mat;
	}
	// Update is called once per frame
	void Update () 
	{
		if(_active)
		{
            float value = Mathf.Sin((Time.time-startTime) * speed);
            mat.color = new Color(1, value, value, 0);
		}
	}
}
