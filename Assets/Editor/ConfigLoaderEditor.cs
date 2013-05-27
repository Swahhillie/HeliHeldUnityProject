using UnityEngine;
using UnityEditor;
using System.Collections;

public class ConfigLoaderEditor : EditorWindow {
	
	[MenuItem("SaveXML/Load level")]
	private static void CreateXmlLoaderWindow(){
		EditorWindow.GetWindow<ConfigLoaderEditor>(true, "Load an XML level");
	}
	private string _levelToLoad = "";
	
	private void OnGUI(){
		EditorGUILayout.BeginVertical();
		{
			_levelToLoad = EditorGUILayout.TextField(_levelToLoad);
			if(GUILayout.Button("Load"))
			{
				ConfigLoader configLoader = new ConfigLoader(true);
				configLoader.LoadLevel(_levelToLoad);
				Debug.Log("Loaded level " + _levelToLoad);
			}
		}
		EditorGUILayout.EndVertical();
	}
}
