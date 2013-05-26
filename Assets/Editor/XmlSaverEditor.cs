
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class XmlSaverEditor : EditorWindow
{
	
	private GameObject[] _selectedGos = new GameObject[0];
	
	
	private static XMLVisitor.ToSave target;
	
	
	
	[MenuItem("SaveXML/Settings")]
	private static void CreateWindowSaveLevel ()
	{
		EditorWindow.GetWindow<XmlSaverEditor> (true, "Changing save settings");
		
	}
	
	/*
	public static string levelBase = "levelBase.xml";
	public static string outputFile = "levelOutput.xml";
	public static string configFile = "config.xml";
	public static bool useExsitingConfig = false;
	public static bool overridePreviousLevelWithName = false;
	*/
	
	private void OnGUI(){
		EditorGUILayout.BeginVertical();
		{
			EditorGUILayout.BeginHorizontal();
			{
				XMLVisitor.useExsitingConfig = GUILayout.Toggle(XMLVisitor.useExsitingConfig, "Use exsiting config");
			}
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.PrefixLabel("levelOutput");
				XMLVisitor.outputFile = EditorGUILayout.TextField(XMLVisitor.outputFile);
			}
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.PrefixLabel("ConfigFile to use as base");
				XMLVisitor.configFile = EditorGUILayout.TextField(XMLVisitor.configFile);
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			{
				XMLVisitor.overridePreviousLevelWithName = GUILayout.Toggle(XMLVisitor.overridePreviousLevelWithName, "Override existing level in config file");
			}
			EditorGUILayout.EndHorizontal();		
				
			
		}
		EditorGUILayout.EndVertical();
	}

	
}
