using UnityEngine;
using UnityEditor;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class MessageSaverEditor : EditorWindow {

	
	private Vector2 _scrollPosition;
	private string _saveName = "messagesXml";
	private string _report;
	private Dictionary<string, Message> _messages;
	
	[MenuItem("SaveXML/Messages")]
	private static void CreateWindowSaveLevel ()
	{
		EditorWindow.GetWindow<MessageSaverEditor> (true, "Message Editor");
		
		
	}
	
	

	private void OnGUI ()
	{
		EditorGUILayout.BeginVertical();
		_scrollPosition = EditorGUILayout.BeginScrollView (_scrollPosition);
		EditorGUILayout.BeginHorizontal ();
		{
			_saveName = GUILayout.TextField(_saveName, 15);
			
			if (GUILayout.Button ("Load")) {
				LoadMessages();
				
			}
		}
		
		EditorGUILayout.EndHorizontal ();
		
		
		{
			if(_messages != null){
				foreach (var pair in _messages) {
					
					{
						Message message = pair.Value;
						GUILayout.Label(pair.Key);
						message.text = EditorGUILayout.TextField(message.text);

						message.audio = EditorGUILayout.ObjectField(message.audio, typeof(AudioClip), false) as AudioClip;
				
						if(GUI.changed && message.audio != null)
						{
							if(Resources.Load(message.audio.name) == null)
							{
								_report = message.audio.name + " is not in a resource folder!";
							}
						}							
						bool toggle = false;
						if(GUILayout.Toggle(toggle,"Delete"))
						{
							_messages.Remove(pair.Key);
							break;
						}
						
					}
					
				}
			}
			else{
				_report = "Load messages from config file first";
			}
		}
		
		EditorGUILayout.EndScrollView ();
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(_report);
		if(GUILayout.Button("Save"))
		{
			SaveToXml();
		}
		
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();
	}
	private void LoadMessages()
	{
		XmlDocument configXml = new XmlDocument();
		configXml.Load(Application.dataPath + "\\" + "config.XML");
		_messages = ConfigLoader.ParseMessages(configXml);
		_report = "Loaded messages from config file";
	}
	private void SaveToXml()
	{
		if(_messages != null){
			XMLVisitor xmlVisitor = new XMLVisitor(null, XMLVisitor.ToSave.Messages);
			foreach(var pair in _messages)
			{
				pair.Value.AcceptVisitor(xmlVisitor);
			}
			string path = xmlVisitor.Write();
			_report = "Saved Successfully";
			UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(path, 0);
			
		}
	}

}
