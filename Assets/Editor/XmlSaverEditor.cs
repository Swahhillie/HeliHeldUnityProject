
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class XmlSaverEditor : EditorWindow
{	
	
	private static XmlVisitor.ToSave target;
	
	
	
	[MenuItem("SaveXML/Settings")]
	private static void CreateWindowSaveLevel ()
	{
		EditorWindow.GetWindow<XmlSaverEditor> (true, "Changing save settings");
		
	}
	private void Awake()
	{
		XmlVisitor.configFile = EditorPrefs.GetString("XmlVisitorConfig", "config.xml");
		XmlVisitor.outputFile = EditorPrefs.GetString("XmlVisitorOutput", "outputFile.xml");
		XmlVisitor.overridePreviousLevelWithName = EditorPrefs.GetBool("XmlVisitorOverride", true);
		XmlVisitor.useExsitingConfig = EditorPrefs.GetBool("XmlVisitorUseExisting", true);
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
				XmlVisitor.useExsitingConfig = GUILayout.Toggle(XmlVisitor.useExsitingConfig, "Use exsiting config");
			}
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.PrefixLabel("levelOutput");
				XmlVisitor.outputFile = EditorGUILayout.TextField(XmlVisitor.outputFile);
			}
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.PrefixLabel("ConfigFile to use as base");
				XmlVisitor.configFile = EditorGUILayout.TextField(XmlVisitor.configFile);
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			{
				XmlVisitor.overridePreviousLevelWithName = GUILayout.Toggle(XmlVisitor.overridePreviousLevelWithName, "Override existing level in config file");
			}
			EditorGUILayout.EndHorizontal();		
				
			
		}
		EditorGUILayout.EndVertical();
	}
	private void OnLostFocus()
	{
		SaveSettings();
	}
	private void OnDestroy()
	{
		SaveSettings();
	}
	private void SaveSettings()
	{
			
		Debug.Log("saving settings to prefs");
		EditorPrefs.GetString("XmlVisitorConfig", XmlVisitor.configFile);
		EditorPrefs.GetString("XmlVisitorOutput", XmlVisitor.outputFile );
		EditorPrefs.GetBool("XmlVisitorOverride", XmlVisitor.overridePreviousLevelWithName);
		EditorPrefs.GetBool("XmlVisitorUseExisting", XmlVisitor.useExsitingConfig);
		
	}

	
}
