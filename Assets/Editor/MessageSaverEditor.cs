using UnityEngine;
using UnityEditor;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class MessageSaverEditor : EditorWindow
{

	
	private Vector2 _scrollPosition;
	private string _saveName = "messagesXml";
	private string _report;
	private string _newMessageName;
	private Dictionary<string, Message> _messages;
	
	[MenuItem("SaveXML/Messages")]
	private static void CreateWindowSaveLevel ()
	{
		EditorWindow.GetWindow<MessageSaverEditor> (true, "Message Editor");
		
		
	}

	private void OnGUI ()
	{
		EditorGUILayout.BeginVertical ();
		{
			_scrollPosition = EditorGUILayout.BeginScrollView (_scrollPosition);
			{
				EditorGUILayout.BeginHorizontal ();
				{
					_saveName = GUILayout.TextField (_saveName, 15);
			
					if (GUILayout.Button ("Load")) {
						LoadMessages ();
				
					}
				}
		
				EditorGUILayout.EndHorizontal ();
		
		
				{
					if (_messages != null) {
						foreach (var pair in _messages) {
					
							{
								Message message = pair.Value;
								GUILayout.Label (pair.Key);
								message.text = EditorGUILayout.TextField (message.text);

								message.audio = EditorGUILayout.ObjectField (message.audio, typeof(AudioClip), false) as AudioClip;
				
								if (GUI.changed && message.audio != null) {
									if (Resources.Load (message.audio.name) == null) {
										_report = message.audio.name + " is not in a resource folder!";
									}
								}
								message.isWarning = GUILayout.Toggle(message.isWarning, "IsWarning");
																						
								
								if (GUILayout.Button ("Delete", GUILayout.MaxWidth(50))) {
									_messages.Remove (pair.Key);
									break;
								}
						
							}
					
						}
						EditorGUILayout.BeginHorizontal ();{
							_newMessageName = EditorGUILayout.TextField (_newMessageName);
							if (GUILayout.Button ("Add")) {
								if(_newMessageName.Length < 1){
									_report = "Give the new Message a name";
								}
								else
								{
								if (_messages.ContainsKey (_newMessageName)) {
										_report = "There is already a message named '" + _newMessageName + "'";	
									} else {
										_messages.Add (_newMessageName, new Message (_newMessageName));
										_report = "New message created named '" + _newMessageName + "'";
										_newMessageName = "";
									}
								}
							}
						}
						EditorGUILayout.EndHorizontal ();
					} else {
						_report = "Load messages from config file first";
					}
				}
				
				
			}
			EditorGUILayout.EndScrollView ();
		
			EditorGUILayout.BeginHorizontal ();
			{
				GUILayout.Label (_report);
				if (GUILayout.Button ("Save")) {
					SaveToXml ();
				}
			}
			EditorGUILayout.EndHorizontal ();
		}
		EditorGUILayout.EndVertical ();
	}

	private void LoadMessages ()
	{
		XmlDocument configXml = new XmlDocument ();
		configXml.Load (Application.dataPath + "\\" + "config.XML");
		_messages = ConfigLoader.ParseMessages (configXml);
		_report = "Loaded messages from config file";
	}

	private void SaveToXml ()
	{
		if (_messages != null) {
			XmlVisitor xmlVisitor = new XmlVisitor (null, XmlVisitor.ToSave.Messages);
			foreach (var pair in _messages) {
				pair.Value.AcceptVisitor (xmlVisitor);
			}
			string path = XmlVisitor.Write (xmlVisitor.Document);
			_report = "Saved Successfully";
			UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal (path, 0);
			
		}
	}

}
