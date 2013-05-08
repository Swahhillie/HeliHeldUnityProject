using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using System;
using System.Linq;

public static class GameObjectExtend
{

	//lookat that fancy shit :)
	public static T GetOrAddComponent<T> (this GameObject go) where T : Component
	{
		//get the component if it exsists. otherwise it is added
		T comp = go.GetComponent<T> ();
		
		if (comp == null) {
		
			comp = go.AddComponent<T> ();
		}
		
		return comp;
	}
	/// <summary>

	/// Returns all monobehaviours (casted to T)

	/// </summary>

	/// <typeparam name="T">interface type</typeparam>

	/// <param name="gObj"></param>

	/// <returns></returns>

	public static T[] GetInterfaces<T> (this GameObject gObj)
	{

		if (!typeof(T).IsInterface)
			throw new SystemException ("Specified type is not an interface!");

		var mObjs = gObj.GetComponents<MonoBehaviour> ();

 

		return (from a in mObjs where a.GetType ().GetInterfaces ().Any (k => k == typeof(T)) select (T)(object)a).ToArray ();

	}

 

	/// <summary>

	/// Returns the first monobehaviour that is of the interface type (casted to T)

	/// </summary>

	/// <typeparam name="T">Interface type</typeparam>

	/// <param name="gObj"></param>

	/// <returns></returns>

	public static T GetInterface<T> (this GameObject gObj)
	{

		if (!typeof(T).IsInterface)
			throw new SystemException ("Specified type is not an interface!");

		return gObj.GetInterfaces<T> ().FirstOrDefault ();

	}

 

	/// <summary>

	/// Returns the first instance of the monobehaviour that is of the interface type T (casted to T)

	/// </summary>

	/// <typeparam name="T"></typeparam>

	/// <param name="gObj"></param>

	/// <returns></returns>

	public static T GetInterfaceInChildren<T> (this GameObject gObj)
	{

		if (!typeof(T).IsInterface)
			throw new SystemException ("Specified type is not an interface!");

		return gObj.GetInterfacesInChildren<T> ().FirstOrDefault ();

	}

 

	/// <summary>

	/// Gets all monobehaviours in children that implement the interface of type T (casted to T)

	/// </summary>

	/// <typeparam name="T"></typeparam>

	/// <param name="gObj"></param>

	/// <returns></returns>

	public static T[] GetInterfacesInChildren<T> (this GameObject gObj)
	{

		if (!typeof(T).IsInterface)
			throw new SystemException ("Specified type is not an interface!");

 

		var mObjs = gObj.GetComponentsInChildren<MonoBehaviour> ();

 

		return (from a in mObjs where a.GetType ().GetInterfaces ().Any (k => k == typeof(T)) select (T)(object)a).ToArray ();

	}

}

public class ConfigLoader : MonoBehaviour
{
	
	public string configFilePath = "config.xml";
	private string config = "";
	static private Dictionary<string, string> settings = new Dictionary<string, string> ();
	static private Dictionary<string, Level> levels = new Dictionary<string, Level> ();
	static private Dictionary<string, Menu> menus = new Dictionary<string, Menu> ();
	public static Dictionary<string, TriggerType> triggerTypes = new Dictionary<string, TriggerType> (); // use this to quickly get Action enum value from the string representation
	public static Dictionary<string, Reaction> reactionTypes = new Dictionary<string, Reaction> ();
	public static ConfigLoader instance = null;
	public Level activeLevel;
	
	void Awake ()
	{
		if (instance != null) {
			Debug.LogError ("THERE ALREADY IS A CONFIGLOADER!!!!!");
			return;
		} else {
			instance = this;
		}
		
		//migrate these to the enums class, this will make it easier to serialize data
		triggerTypes ["None"] = TriggerType.None;
		triggerTypes ["TriggerEnter"] = TriggerType.OnTriggerEnter;
		triggerTypes ["TriggerExit"] = TriggerType.OnTriggerExit;
		triggerTypes ["Death"] = TriggerType.OnDeath;
		triggerTypes ["Spawn"] = TriggerType.OnSpawn;
		triggerTypes ["Rescue"] = TriggerType.OnRescued;
		triggerTypes ["OutOfLive"] = TriggerType.OnOutOfLive;
		triggerTypes ["Activate"] = TriggerType.OnActivate;
		
		reactionTypes ["None"] = Reaction.None;
		reactionTypes ["Destroy"] = Reaction.Destroy;
		reactionTypes ["Spawn"] = Reaction.Spawn;
		reactionTypes ["Rescued"] = Reaction.Rescued;
		reactionTypes ["OutOfLive"] = Reaction.OutOfLive;


		
		StreamReader reader = new StreamReader (Application.dataPath + "\\" + configFilePath);
		config = reader.ReadToEnd ();
		reader.Close ();
		
		XmlDocument xml = new XmlDocument ();
		xml.LoadXml (config);
		ParseSettings (xml);
		
		//create the levels and list them, does not create any yet.
		ParseLevels (xml);
		
		
		ParseMenus (xml);
		
		menus ["MainMenu"].LoadMenu ();
		
	}

	public void LoadLevel (string name)
	{
		
		if (activeLevel != null) {
			Debug.Log ("Unloading level " + activeLevel.levelName);
			activeLevel.UnLoadLevel ();	
		}
		Debug.Log ("Loading level" + name);
		activeLevel = levels [name];
		activeLevel.LoadLevel ();
		Debug.Log ("Loaded level success");
	}

	void ParseSettings (XmlDocument xml)
	{
		XmlNodeList settingsList = xml.GetElementsByTagName ("Setting");
		foreach (XmlNode setting in settingsList) {
			settings [setting.Attributes ["name"].InnerText] = setting.Attributes ["value"].InnerText;
		}
		
		string debugStr = "";
		foreach (KeyValuePair<string, string> p in settings) {
			debugStr += ("Setting " + p.Key + " has value " + p.Value + "\n");	
		}
		Debug.LogWarning ("Settings loaded" + debugStr);
	}

	void ParseLevels (XmlDocument xml)
	{
		//create all the levels
		string report = "";
		XmlNodeList levelList = xml.GetElementsByTagName ("Level");
		foreach (XmlNode lvlXml in levelList) {
			string lName = lvlXml.Attributes ["name"].InnerText;
			Level newLvl = new Level (lName, lvlXml);
			levels [lName] = newLvl;
			report += "Created level(" + lName + ")\n";
			//fill a single level with gos;
		}
		Debug.Log (report);
		

	
	}

	void ParseMenus (XmlDocument xml)
	{
		XmlNodeList menuList = xml.GetElementsByTagName ("Menu");
		foreach (XmlNode menuXml in menuList) {
			string menuName = menuXml.Attributes ["name"].InnerText;
			Menu m = new Menu (menuName, menuXml);
			menus [menuName] = m;
			
		}
	}

	private void Update ()
	{
		//TEMPORARY
		foreach (KeyValuePair<string, Menu> p in menus) {
			p.Value.Update ();
		}
	}
	//util
	public static Vector3 ParseVec3 (string s)
	{
		//Split by comma and create a vector 3
		s.Replace("(", null);
		s.Replace(")", null);
		string[] elements = s.Split (',');
		if (elements.Length != 3)
			Debug.LogError ("Passed Vector3 that does not have 3 elements");
			
		return new Vector3 (float.Parse (elements [0]), float.Parse (elements [1]), float.Parse (elements [2]));
	}
	//---------------------- getting settings from the settings file and putting reading them as proper values
	public static bool GetValue (string fieldName, ref string target)
	{
		return settings.TryGetValue (fieldName, out target);
	}

	public static bool GetValue (string fieldName, ref float target)
	{
		string str;
		if (settings.TryGetValue (fieldName, out str)) {
			target = float.Parse (str);
			return true;
		} else {
			Debug.LogError ("Couldnt get setting " + fieldName + " from settings map ");
			return false;
		}
	}

	public static bool GetValue (string fieldName, ref int target)
	{
		string str;
		if (settings.TryGetValue (fieldName, out str)) {
			target = int.Parse (str);
			return true;				
		} else {
			Debug.LogError ("Couldnt get setting " + fieldName + " from settings map ");
			return false;
		}
	}
	
	public static bool GetValue (string fieldName, ref bool target)
	{
		string str;
		if (settings.TryGetValue (fieldName, out str)) {
			target = bool.Parse (str);
			return true;	
		} else {
			Debug.LogError ("Couldnt get setting " + fieldName + " from settings map ");
			return false;
		}
	}

	
}

