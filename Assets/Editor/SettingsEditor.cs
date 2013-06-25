using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Xml;

public class SettingsEditor : EditorWindow {
	
	XmlDocument _document;
	List<string> _settingNames;
	List<string> _settingValues;
	XmlNode _settingsNode;
	
	[MenuItem("SaveXML/Game Settings")]
	static void OpenSettingsEditorWindow()
	{
		SettingsEditor settingsEditor = GetWindow<SettingsEditor>();
		settingsEditor.GetSettings();
	}
	
	
	private Vector2 _scrollPosition;
	private string _newSettingName;
	void OnGUI()
	{
		EditorGUILayout.BeginVertical();
		EditorGUILayout.BeginScrollView(_scrollPosition);
		{
			for(var i = 0; i < _settingNames.Count; i++)
			{
				//_settings[settingPair.Key] = EditorGUILayout.TextField(settingPair.Key, settingPair.Value);
				_settingValues[i] = EditorGUILayout.TextField(_settingNames[i], _settingValues[i]);

			}
		}
		EditorGUILayout.Separator();
		_newSettingName = EditorGUILayout.TextField("Name", _newSettingName);
		if(EditorGUILayout.Toggle("AddSetting", false))
		{
			if(!_settingNames.Contains(_newSettingName))//prevent duplicate names
			{
				_settingNames.Add(_newSettingName);
				_settingValues.Add("");
				_newSettingName = "";
			}
			else
			{
				Debug.LogWarning("There is already a node named " + _newSettingName);
			}
			
		}
		
		EditorGUILayout.EndScrollView();
		if(GUILayout.Button("Save"))
		{
			SaveSettings();
		}
		if(EditorGUILayout.Toggle("Reload", false))
		{
			GetSettings();
		}
		EditorGUILayout.EndVertical();
	}
	public void GetSettings()
	{	
		//initialize the values
		_document = ConfigLoader.Instance.GetWorkingDocument();
		var settingsNodeList = _document.DocumentElement.SelectNodes("Settings/Setting");
		_settingNames = new List<string>();
		_settingValues = new List<string>();
		foreach(XmlNode n in settingsNodeList)
		{
			_settingNames.Add(n.Attributes[0].Value);
			_settingValues.Add(n.Attributes[1].Value);
		}
		_settingsNode = _document.DocumentElement.SelectSingleNode("Settings");
	}
	void SaveSettings()
	{
		//remove exsisting node
		
		if(_settingsNode != null)
			_document.DocumentElement.RemoveChild(_settingsNode);
		
		//add new node
		_settingsNode = _document.CreateNode(XmlNodeType.Element, "Settings", null);
		_document.DocumentElement.AppendChild(_settingsNode);
		
		for(var i = 0; i < _settingNames.Count; i++)
		{
			XmlNode newNode = _document.CreateNode(XmlNodeType.Element, "Setting", null);
			_settingsNode.AppendChild(newNode);
			var ele = (XmlElement)newNode;
			ele.SetAttribute("name", _settingNames[i]);
			ele.SetAttribute("value", _settingValues[i]);
			
		}
		ConfigLoader.Instance.SaveDocument(_document);
	}
	
}
