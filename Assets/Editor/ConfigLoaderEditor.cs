using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Xml;
using System.Text;

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
				
				ConfigLoader.Instance.LoadLevel(_levelToLoad);
				Debug.Log("Loaded level " + _levelToLoad);
			}
		}
		EditorGUILayout.EndVertical();
	}
	
}
