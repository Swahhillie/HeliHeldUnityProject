using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lightning : MonoBehaviour {

	private GameObject cloudeff;
	private GameObject env;
	private AudioClip thunderSound;
	public float radius = 7;
	public int maxElements = 150;
	
	private float startWidth=6;
	private float endWidth=1;
	
	private List<LineRenderer> lightning = new List<LineRenderer>();

	private Material mat;
	
	void Start()
	{

		this.cloudeff = CreateLightObject("cloudeff",LightType.Spot,4,200,150,new Vector3(0,200/5,0),new Vector3(90,0,0));
		this.env = CreateLightObject("enveff",LightType.Point,6,1000,6,new Vector3(0,-200/2,0),new Vector3(0,0,0));
		mat = (Material)Resources.Load("lightning_material",typeof(Material));
		GenerateLightning();
		StartCoroutine("CreateLightning");
	}
	
	GameObject CreateLightObject(string name,LightType type,float intensity,float range,float spotangle,Vector3 pos,Vector3 rot)
	{
		GameObject obj = new GameObject();
		obj.transform.parent = this.transform;
		obj.name=name;
		obj.transform.localPosition =  pos;
		obj.transform.rotation = Quaternion.Euler(rot);
		obj.AddComponent("Light");
		Component man;
		man = obj.GetComponent("Light");
		man.light.type = type;
		man.light.shadows = LightShadows.Soft;
		man.light.enabled = false;
		man.light.intensity = intensity;
		man.light.range= range;
		man.light.spotAngle=spotangle;
		return obj;
	}
	
	
	
	void GenerateLightning()
	{
		//use for loop
		GameObject mainLightning = new GameObject();
		mainLightning.transform.parent=this.transform;
		mainLightning.transform.localPosition=this.transform.position;
		//mainLightning.name="E"+i.ToString()+"."+h.ToString();
		LineRenderer line = mainLightning.AddComponent<LineRenderer>();
		line.renderer.enabled=false;
		line.material=mat;
		line.SetVertexCount(maxElements);
		line.SetWidth(startWidth,endWidth);
		
		thunderSound = (AudioClip)Resources.Load("Lightning.ogg");
		
		Vector3 tempPos = transform.position;
		for(int i = 0;i<maxElements;++i)
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
			CloudEffect();
			yield return (null);
			LightningEffect();
			yield return (null);			
			EnvEffect();
			yield return new WaitForSeconds (0.2f);
			LightningEffect();
			GenerateLightning();
			LightningEffect();
			yield return new WaitForSeconds (0.2f);
			LightningEffect();
			CloudEffect();
			EnvEffect();
			Destroy (this.gameObject);
	}
	
	
	void CloudEffect()
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
	}
	
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
			AudioSource audio = this.gameObject.GetOrAddComponent<AudioSource>();
			audio.PlayOneShot(thunderSound);
		}
	}
}
