using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lightning_old : MonoBehaviour {

	private GameObject cloudeff;
	private GameObject beam;
	private GameObject env;
	private List<GameObject> lightning = new List<GameObject>();
	Vector3 target;
	public Material mat;
	
	void Start()
	{
		RaycastHit hit;
		Physics.Raycast(this.transform.position,-Vector3.up,out hit);
		target = hit.point;
		//this.gameObject.transform.position.y=height;
		this.cloudeff = createLightObject("cloudeff",LightType.Spot,4,target.magnitude,150,new Vector3(0,target.magnitude/5,0),new Vector3(90,0,0));
		//this.beam = createLightObject("beameff",LightType.Spot,8,target.magnitude,6,new Vector3(0,0,0),new Vector3(90,0,0));
		this.env = createLightObject("enveff",LightType.Point,6,target.magnitude*5,6,new Vector3(0,-target.magnitude/2,0),new Vector3(0,0,0));
		
		mat = (Material)Resources.Load("lightning_material",typeof(Material));
		
		generateLightning();
		StartCoroutine("createLightning");
	}
	
	GameObject createLightObject(string name,LightType type,float intensity,float range,float spotangle,Vector3 pos,Vector3 rot)
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
	
	void generateLightning()
	{
		Vector3 temp = this.transform.position;
		Vector3 targettemp = target;
		
		int x=0;
		int maxelements =(int) (6+Random.value*5);
		do
		{
			++x;
			int y=0;
			Vector3 path = temp-targettemp;
			int forcs =(int) (1+Random.value*3);
			do
			{
				path = temp-path.normalized*Random.value*200;
				path.x=path.x+((Random.value*400)-200);
				path.z=path.z+((Random.value*400)-200);
				
				GameObject element = new GameObject();
				element.transform.parent=this.transform;
				element.transform.localPosition=this.transform.position;
				element.name="E"+x.ToString()+"."+y.ToString();
				LineRenderer line;
				line = element.AddComponent<LineRenderer>();
				line.renderer.enabled=false;
				line.material=mat;
				++y;
				line.SetWidth(30,30);
				if(y<forcs||x==maxelements)
				{
					line.SetWidth(30,5);
				}
				line.SetPosition(0,temp);
				line.SetPosition(1,path);
				this.lightning.Add(element);
			}
			while(y<forcs);
			temp=path;
		}
		while(x<maxelements);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.E))
		{
			//generate Lightning
			Debug.Log("There will be light(ning) !");
			StartCoroutine("createLightning");
		}
	}
	IEnumerator createLightning()
	{
			cloudEffect();
			yield return new WaitForSeconds (0.1f);
			lightningEffect();
			//beamEffect();
			yield return new WaitForSeconds (0.1f);			
			envEffect();
			yield return new WaitForSeconds (0.3f);
			lightningEffect();
			//beamEffect();
			cloudEffect();
			envEffect();
			Destroy (this.gameObject);
	}
	
	
	void cloudEffect()
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
	void envEffect()
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
	
	void lightningEffect()
	{
		if(lightning[0].GetComponent("LineRenderer").renderer.enabled)
		{
			for(int x = lightning.Count-1;x>=0;--x)
			{
				lightning[x].GetComponent("LineRenderer").renderer.enabled=false;	
			}
		}
		else
		{
			for(int x = lightning.Count-1;x>=0;--x)
			{
				lightning[x].GetComponent("LineRenderer").renderer.enabled=true;	
			}
		}
	}
}
