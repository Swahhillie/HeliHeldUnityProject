using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lightning : MonoBehaviour {

	//private GameObject cloudeff;
	//private GameObject beam;
	//private GameObject env;
	
	public float radius = 50;
	private List<LineRenderer> lightning = new List<LineRenderer>();

	private Material mat;
	
	void Start()
	{

		//this.cloudeff = CreateLightObject("cloudeff",LightType.Spot,4,target.magnitude,150,new Vector3(0,target.magnitude/5,0),new Vector3(90,0,0));

		//this.env = CreateLightObject("enveff",LightType.Point,6,target.magnitude*5,6,new Vector3(0,-target.magnitude/2,0),new Vector3(0,0,0));
		
		mat = (Material)Resources.Load("lightning_material",typeof(Material));
		
		GenerateLightning();
		
		StartCoroutine("CreateLightning");
	}
	
	void GenerateLightning()
	{

		
		int maxelements =(int) (6+Random.value*5);
		//use for loop
		
		GameObject mainLightning = new GameObject();
		mainLightning.transform.parent=this.transform;
		mainLightning.transform.localPosition=this.transform.position;
		//mainLightning.name="E"+i.ToString()+"."+h.ToString();
		LineRenderer line = mainLightning.AddComponent<LineRenderer>();
		line.renderer.enabled=false;
		line.material=mat;
		line.SetVertexCount(maxelements);
		Vector3 tempPos = transform.position;
		for(int i = 0;i<maxelements;++i)
		{	
			//Random direction
			Vector3 point = Random.insideUnitSphere * radius;
			point += tempPos + Vector3.down * radius;
			line.SetPosition(i,point);
			
			
			//set a new starting point
			tempPos=point;
			//add LineRenderer to list
			this.lightning.Add(line);
		}
	}
	

	IEnumerator CreateLightning()
	{
			//CloudEffect();
			//yield return (null);
			LightningEffect();
			//beamEffect();
			yield return (null);			
			//EnvEffect();
			yield return new WaitForSeconds (0.5f);
			LightningEffect();
			//beamEffect();
			//CloudEffect();
			//EnvEffect();
			Destroy (this.gameObject);
	}
	
	
	/*void CloudEffect()
	{
		
		if(this.cloudeff.light.enabled)
		{
			this.cloudeff.light.enabled=false;
		}
		else
		{
			this.cloudeff.light.enabled=true;	
		}
	}
	
	void beamEffect()
	{
		if(beam.light.enabled)
		{
			beam.light.enabled=false;
		}
		else
		{
			beam.light.enabled=true;	
		}
	}
	void EnvEffect()
	{
		if(env.light.enabled)
		{
			env.light.enabled=false;
		}
		else
		{
			env.light.enabled=true;	
		}
	}*/
	
	void LightningEffect()
	{
		if(lightning[0].renderer.enabled)
		{
			for(int x = lightning.Count-1;x>=0;--x)
			{
				lightning[x].renderer.enabled=false;	
			}
		}
		else
		{
			for(int x = lightning.Count-1;x>=0;--x)
			{
				lightning[x].renderer.enabled=true;	
			}
		}
	}
}
