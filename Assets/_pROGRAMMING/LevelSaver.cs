using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;

public class LevelSaver : MonoBehaviour
{

	// Use this for initialization
	
	public string levelName = "";
	public string levelBase = "levelBase.xml";
	public string outputFile = "levelOutput.xml";
	private List<Button3D> _menuToSave;
	public string menuName = "";
	public bool displaySaveMenu = false;
	public Rect saveMenuRect = new Rect(0,0,200,300);
	private string _feedback = "Save Menu";
	private string _textInput = "";
	private bool _drawHelp = true;
	private enum MenuState{
		Closed,
		SelectingWhatToSave,
		SavingLevel,
		SavingMenu
	}
	private MenuState _state = MenuState.SelectingWhatToSave;
	void Start ()
	{
		levelName = ""; // stops the editor from remembering
		menuName = "";		
		
	}
	
	// Update is called once per frame
	void Update ()
	{

		//SaveLevel();


		//SaveMenu ();

	
	}
	void OnGUI(){
		if(displaySaveMenu){
			DrawSaveMenu();
		}
	}
	
	private void DrawSaveMenu(){
		
		GUILayout.BeginArea(saveMenuRect);
		GUILayout.Label(_feedback);
		if(_state == MenuState.SelectingWhatToSave){
			if(GUILayout.Button("Write level file")){
				_state = MenuState.SavingLevel;
			}
			else if(GUILayout.Button("Write menu file")){
				_state = MenuState.SavingMenu;
			}
		}
		else if(_state == MenuState.SavingLevel){
			_feedback = "Saving a level, enter a name";
			_textInput = GUILayout.TextField(_textInput, 15);
			if(GUILayout.Button("Save level")){
				levelName = _textInput;
				SaveLevel();
				_feedback = "Saved the level to " + Application.dataPath + "\\" +  outputFile;
				_state = MenuState.SelectingWhatToSave;
			}
			BackButton();
		}
		else if(_state == MenuState.SavingMenu){
			_feedback = "Saving a menu, enter a name";
			_textInput = GUILayout.TextField(_textInput, 15);
			if(GUILayout.Button("Save Menu")){
				menuName = _textInput;
				SaveMenu();
				_feedback = "Saved the menu to " + Application.dataPath + "\\" + outputFile;
				_state = MenuState.SelectingWhatToSave;
			}
			BackButton();
			
		}
		else{
		
		}
		
		_drawHelp = GUILayout.Toggle(_drawHelp,"Help");
		if(_drawHelp){
			GUILayout.TextArea(HelpText());
			
		}
		GUILayout.EndArea();
	}
	private string HelpText(){
		if(_state == MenuState.SavingLevel){
			return @"A level consist of castaways, triggers and ships.
				Make sure that object with those scripts on them do not have parents.
				To create a new castaway/ship drag and drop a castaway/ship from the prefab folder into the scene.
				To create a trigger, create a new gameobject and click addcomponent, type in 'trigger'.
				Go to the inspector of the gameobject, you can now set  the values of the  trigger.";
		}
		if(_state == MenuState.SavingMenu){
			return @"To create a menu, drag and drop Button3D prefabs into the scene.
			Position them where you like. Set the variables in the inspector of the gameobject.
			command is  the function that will be called if the button is clicked. 
			Ask a programmer what functions there are to call.";
		}
		return @"Select a target to save";
	}
	private void BackButton(){
		if(GUILayout.Button("Back")){
			_state = MenuState.SelectingWhatToSave;
		}
	}
	private void SaveLevel ()
	{
		if (levelName == "") {
			Debug.LogError ("Enter a level name first!");
			return;
		}
		Debug.Log ("Saving level " + levelName);
		
		
		XmlDocument doc = new XmlDocument ();
		doc.Load (Application.dataPath + "\\" + levelBase);
		
		XmlNode root = doc.DocumentElement;
		
		XmlNode testChild = doc.CreateNode (XmlNodeType.Element, "Level", "");
		
		XmlElement tc = (XmlElement)testChild;//
		tc.SetAttribute ("name", levelName);
		
		root.AppendChild (testChild);
		
		XmlNode objectsXml = doc.CreateNode (XmlNodeType.Element, "Objects", "");
		testChild.AppendChild(objectsXml);
		XMLVisitor xmlVisitor = new XMLVisitor (doc);
		
//		IVisitable[] safeable;
		GameObject[] allGos = GameObject.FindObjectsOfType (typeof(GameObject)) as GameObject[];
		
		int safeLayer = LayerMask.NameToLayer ("SaveLayer");
		
		//finding all gos that are in the safelayer and have no parents
		GameObject[] toSaveGo = System.Array.FindAll<GameObject> (allGos, x => x.layer == safeLayer && x.transform.parent == null);
	
		Debug.Log ("Gameobjects to save count = " + toSaveGo.Length);
				
		int i = 0;
		
		//System.Array.ForEach<GameObject>(toSaveGo, (x) => {x.name = go.name + " " + i.ToString("000") + " "; i++;});
		foreach (GameObject go in toSaveGo) {
			
			go.name = go.name + "_" + i.ToString ("000") + "_"; //give all gos that will be safed a unique names
			
			i++;
		}
		foreach (GameObject go in toSaveGo) {
			xmlVisitor.OpenNewObject (go); // open a new gameobject to append the components to
			
			List<IVisitable> toSave = new List<IVisitable> ();
			toSave.AddRange (go.GetInterfacesInChildren<IVisitable> ()); // add all visitables to the list of what the visitor will pass
			
			foreach (IVisitable visitable in toSave) {
				visitable.AcceptVisitor (xmlVisitor); // add each component to the active gameObject
			}
		
		}
		
		//cleanup names
		foreach (GameObject go in toSaveGo) {
			go.name = go.name.Substring (0, go.name.Length - 5); // leave the number out of it
		}
		
		//wrint to the output file. note that the asset database does not update automatically, minimize / reload unity first
		XmlTextWriter writer = new XmlTextWriter (Application.dataPath + "\\" + outputFile, System.Text.Encoding.ASCII);
		writer.Formatting = Formatting.Indented;
		//doc.Save(Application.dataPath + "\\" +outputFile);
		doc.Save (writer);
		writer.Close ();
		
	
		
		//doc.WriteContentTo(writer);
		
	}
	
	private void SaveMenu ()
	{
		if (menuName == "") {
			Debug.LogError ("Enter a menu name first");
			return;
		}
		Debug.Log ("Saving menu " + menuName);
		
		
		XmlDocument doc = new XmlDocument ();
		doc.Load (Application.dataPath + "\\" + levelBase);
		
		XmlNode root = doc.DocumentElement;
		
		XmlNode testChild = doc.CreateNode (XmlNodeType.Element, "Menu", "");
		
		
		XmlElement tc = (XmlElement)testChild;//
		tc.SetAttribute ("name", menuName);
		
		root.AppendChild (testChild);
		
		XMLVisitor xmlVisitor = new XMLVisitor (doc);
		_menuToSave = new List<Button3D>(FindObjectsOfType(typeof(Button3D)) as Button3D[]);
		_menuToSave.ForEach (x => x.AcceptVisitor (xmlVisitor));
		
		XmlTextWriter writer = new XmlTextWriter (Application.dataPath + "\\" + outputFile, System.Text.Encoding.ASCII);
		writer.Formatting = Formatting.Indented;
		//doc.Save(Application.dataPath + "\\" +outputFile);
		doc.Save (writer);
		writer.Close ();
	}
}
