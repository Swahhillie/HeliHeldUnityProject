using UnityEngine;
using System.Collections;
using System.Xml;

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
		
				
		XmlNode obj = doc.CreateNode(XmlNodeType.Element, "Object", "");
		obj.InnerText = "hallo, this is an object node";
		testChild.AppendChild(obj);
		root.AppendChild(testChild);
		
		
		XmlWriter writer = new XmlTextWriter(Application.dataPath + "\\" +outputFile, System.Text.Encoding.ASCII);
		//doc.Save(Application.dataPath + "\\" +outputFile);
		doc.Save(writer);
		writer.Close();
		
		//doc.WriteContentTo(writer);
		
	}
}
