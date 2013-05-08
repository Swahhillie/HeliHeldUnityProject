using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;

public class LevelSaver : MonoBehaviour {

	// Use this for initialization
	
	public string levelName = "";
	
	public bool shouldSave = false; //when set to true in the inspector it will save the level.
	
	
	public string levelBase = "levelBase.xml";
	public string outputFile = "levelOutput.xml";
	
	void Start () {
		levelName = ""; // stops the editor from remembering
		shouldSave = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(shouldSave == true){
			shouldSave = false;
			if(levelName == ""){
				Debug.LogError("Enter a level name first!");
				return;
			}
			else{
				SaveLevel();
			}
			
		}
	
	}
	private void SaveLevel(){
		Debug.Log("Saving level " + levelName);
		
		
		XmlDocument doc = new XmlDocument();
		doc.Load(Application.dataPath + "\\" +levelBase);
		
		XmlNode root = doc.DocumentElement;
		
		XmlNode testChild = doc.CreateNode(XmlNodeType.Element,"Level", "");
		
		XmlElement tc = (XmlElement)testChild;//
		tc.SetAttribute("name", levelName);
		
		root.AppendChild(testChild);
		
		XMLVisitor xmlVisitor = new XMLVisitor(doc);
		
//		IVisitable[] safeable;
		GameObject[] allGos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		
		int safeLayer = LayerMask.NameToLayer("SaveLayer");
		
		//finding all gos that are in the safelayer and have no parents
		GameObject[] toSaveGo = System.Array.FindAll<GameObject>(allGos, x => x.layer == safeLayer && x.transform.parent == null);
	
		Debug.Log("Gameobjects to save count = " + toSaveGo.Length);
				
		int i = 0;
		foreach(GameObject go in toSaveGo){
			
			go.name = go.name + "_" + i + "_"; //give all gos that will be safed a unique names
			
			i++;
		}
		foreach(GameObject go in toSaveGo){
			xmlVisitor.OpenNewObject(go); // open a new gameobject to append the components to
			
			List<IVisitable> toSave = new List<IVisitable>();
			toSave.AddRange(go.GetInterfacesInChildren<IVisitable>()); // add all visitables to the list of what the visitor will pass
			
			foreach(IVisitable visitable in toSave){
				visitable.AcceptVisitor(xmlVisitor); // add each component to the active gameObject
			}
		
		}
		
		
		
//wrint to the output file. note that the asset database does not update automatically, minimize / reload unity first
		XmlTextWriter writer = new XmlTextWriter(Application.dataPath + "\\" +outputFile, System.Text.Encoding.ASCII);
		writer.Formatting = Formatting.Indented;
		//doc.Save(Application.dataPath + "\\" +outputFile);
		doc.Save(writer);
		writer.Close();
		
		
		//doc.WriteContentTo(writer);
		
	}
}
