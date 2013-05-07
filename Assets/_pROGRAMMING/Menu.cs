using UnityEngine;
using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

[System.SerializableAttribute()]
public class Menu
{

	private string menuName = "";
	private XmlNode menuXml;
	private static float buttonTimer;
	private GameObject menuElement;
	private KinectMouse mouse;
	private List<Button3D> buttons = new List<Button3D>();
	public LayerMask buttonLayer;
	private int hoveringOver = -1;
	private bool _isActive = false;
	bool isStarted = false;
	private float hoverStart; // time at wich the player started hovering over a button;
	
	void Start ()
	{
		buttonLayer.value = 1 << LayerMask.NameToLayer ("Buttons");
		Main script = GameObject.Find ("Main").GetComponent<Main> ();
		//createMenu(script.loadName);
		
		ConfigLoader.GetValue ("buttonTime", ref buttonTimer);
		mouse = GameObject.Find ("Mouse").GetComponent<KinectMouse> ();
		
		
	}

	public Menu (string name, XmlNode menuXml)
	{
		menuName = name;
		this.menuXml = menuXml;
	}

	void ClickButton (string command)
	{
		if (command == "LoadLevel1") 
		{
			UnLoadMenu ();
			ConfigLoader.instance.StartCoroutine(SwitchSceneAndLoad("LevelDesign", "level3"));
		}
		Debug.Log (command);
	}
	
	IEnumerator SwitchSceneAndLoad(string scene, string level){
		Application.LoadLevel(scene);
		yield return null; //wait a frame so the scene can load
		ConfigLoader.instance.LoadLevel (level); //load the objects
	}
	void UnLoadMenu ()
	{
		for (int i = buttons.Count -1; i >= 0; i--) {
			GameObject.Destroy (buttons [i].gameObject);
		}
		buttons.Clear ();
		_isActive = false;
	}

	public void LoadMenu ()
	{
		XmlNodeList buttonsList = menuXml.ChildNodes;
		Debug.Log("Loading menu " + menuName + " with " + buttonsList.Count + " buttons");
		foreach (XmlNode buttonXml in buttonsList) {
			Vector3 pos = ConfigLoader.ParseVec3 (buttonXml ["Pos"].InnerText);
			Vector3 rot = ConfigLoader.ParseVec3 (buttonXml ["Rot"].InnerText);
			string label = buttonXml ["Label"].InnerText;
			string command = buttonXml ["Function"].InnerText;
			buttons.Add (Button3D.CreateButton (label, command, ClickButton, pos, rot));
		}
		_isActive = true;
	}

	public void Update ()
	{
		if(isStarted == false){
			isStarted = true;
			Start();
		}
		if (_isActive == true) {
			Ray ray = Camera.main.ScreenPointToRay (mouse.position);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, Mathf.Infinity, buttonLayer.value)) {
				
				Button3D b = hit.collider.gameObject.GetComponent<Button3D> ();
				int id = b.gameObject.GetInstanceID ();
				if (hoveringOver != id) {
					hoverStart = Time.time;
					hoveringOver = id;
				} else if (hoverStart + buttonTimer < Time.time) {
					b.Activate ();
					hoverStart = Time.time;
					hoveringOver = -1;
				}
			
			}
		}
	}

	public bool isActive {
		get{ return _isActive;}
	}
	/*void CreateMenu(string interfce)
	{
		if(menuElement!=null)
		{
			Destroy(menuElement);	
		}
		menuElement = new GameObject("Menu");	
		font = (Font)Resources.Load("lgs");
		List<string> menu = Fileparser.getObject(interfce);
		
		for (int i = 0; i < menu.Count; i++)
		{		
			string[] data = menu[i].Split(';');
			switch(data[0])
			{
				case "button":
				{
					GameObject button = GameObject.CreatePrimitive(PrimitiveType.Cube);
					button.transform.parent=menuElement.transform;
					
					for(int g=data.Length-1;g>0;--g)
					{
						string[] keys=data[g].Split('=');
						switch(keys[0])
						{
							case "pos":
							{
								string[] values = keys[1].Split(',');
								if(values.Length==3)
								{
									button.transform.position = new Vector3(float.Parse(values[0]),float.Parse(values[1]),float.Parse(values[2]));
									button.layer=8;
								}	
								break;
							}
							case "rot":
							{
								string[] values = keys[1].Split(',');
								if(values.Length==3)
								{
									button.transform.rotation = Quaternion.Euler(new Vector3(float.Parse(values[0]),float.Parse(values[1]),float.Parse(values[2])));
								}	
								break;
							}
							case "scale":
							{
								string[] values = keys[1].Split(',');
								if(values.Length==3)
								{
									button.transform.localScale = new Vector3(float.Parse(values[0]),float.Parse(values[1]),float.Parse(values[2]));
								}	
								break;
							}
							case "action":
							{
								ButtonAction script = (ButtonAction)button.AddComponent(typeof(ButtonAction));
								script.setAction(keys[1]);
								break;
							}
							case "label":
							{
								string[] values = keys[1].Split(',');
								if(values.Length==1)
								{
									button.name=values[0];
									GameObject label = new GameObject("Label");
									label.transform.parent = button.transform;
									label.layer=8;
									label.transform.localPosition=new Vector3(0,0,0);
									label.transform.localRotation = new Quaternion(0,0,0,0);
									MeshRenderer render = (MeshRenderer)label.AddComponent(typeof(MeshRenderer));
									render.material = font.material;
									
									TextMesh text = (TextMesh)label.AddComponent(typeof(TextMesh));
									text.anchor = TextAnchor.MiddleCenter;
									text.text = values[0].ToString();
									text.alignment = TextAlignment.Center;
									text.fontSize = 200;
									text.font = font;
									float x = 0.01f;	//button.transform.localScale.x/(text.fontSize/200);
									float y = 0.1f;		//button.transform.localScale.y/(text.fontSize/2000);
									label.transform.localScale =  new Vector3(x,y,1f);
								}	
								break;
							}
						}
					}
					break;
				}
			}
		}
	}
	*/
	/*void Update()
	{
		Ray ray = Camera.main.ScreenPointToRay(mouse.position);
		RaycastHit hit;
		if(Physics.Raycast(ray,out hit,1000,1<<8))
		{
			btime+=Time.deltaTime;
			if(btime>=buttontimer)
			{
				ButtonAction script =hit.collider.gameObject.GetComponent<ButtonAction>();
				string action = script.getAction();
				string[] data = action.Split(',');
				switch(data[0])
				{
					case "interface":
					{
						if(Fileparser.checkObjectKey(data[1]))
						{
							btime=0;
							createMenu(data[1]);
						}
						else
						{
							Debug.Log("Menu does not exist");	
						}
						break;
					}
					case "level":
					{
						if(Fileparser.checkObjectKey(data[1]))
						{
							btime=0;
							Main mscript = (Main)GameObject.Find("Main").GetComponent(typeof(Main));
							mscript.loadName = (data[1]);
							Application.LoadLevel("LevelDesign");
						}
						else
						{
							Debug.Log("Level does not exist");	
						}
						break;
					}
					case "editor":
					{
						btime=0;
						Application.LoadLevel("PlacingObjects");
						break;
					}
					default:
					{
						Debug.Log("unknown Interface or level");
						break;
					}
					
				}
			}
		}
		else
		{
			btime = 0;	
		}
	}
	*/
	
}
