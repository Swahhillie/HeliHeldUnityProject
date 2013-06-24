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
	private Vector2 scrollPos;
	
	private void OnGUI(){
		EditorGUILayout.BeginScrollView(scrollPos);
		EditorGUILayout.BeginVertical();
		{
//			_levelToLoad = EditorGUILayout.TextField(_levelToLoad);
//			if(GUILayout.Button("Load"))
//			{
//				
//				ConfigLoader.Instance.LoadLevel(_levelToLoad);
//				Debug.Log("Loaded level " + _levelToLoad);
//			}
			foreach(var lvlPair in ConfigLoader.Instance.levels)
			{
				if(GUILayout.Button(lvlPair.Key))
				{
					ConfigLoader.Instance.LoadLevel(lvlPair.Key);
					Close();
					
				}
			}
		}
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndScrollView();
	}
	
}
