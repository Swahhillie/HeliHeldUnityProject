using UnityEngine;
using UnityEditor;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class SaveLevelEditor : EditorWindow
{

	
	private Vector2 _scrollPosition;
	private string _report;
	private string _newMessageName;
	private Dictionary<string, Level> levels;
	[MenuItem("SaveXML/Score Levels")]
	private static void CreateWindowSaveLevel ()
	{
		EditorWindow.GetWindow<SaveLevelEditor> (true, "Score Level Editor");
		
		
	}
	private void OnEnable(){
		levels = ConfigLoader.Instance.levels;
	}

	private void OnGUI ()
	{
		EditorGUILayout.BeginVertical();
		{
			_scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
			foreach(var p in levels){
				Level level = p.Value;
				GUILayout.Label(p.Key, EditorStyles.boldLabel);
				GUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Gold Achievement Minimum");
				level.goldAchievementScore = EditorGUILayout.IntField(level.goldAchievementScore);
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Silver Achievement Minimum");
				level.silverAchievementScore = EditorGUILayout.IntField(level.silverAchievementScore);
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Bronze Achievement Minimum");
				level.bronzeAchievementScore = EditorGUILayout.IntField(level.bronzeAchievementScore);
				GUILayout.EndHorizontal();
				
				level.bronzeAchievementScore = Mathf.Max(0, level.bronzeAchievementScore );
				level.silverAchievementScore = Mathf.Max(level.bronzeAchievementScore, level.silverAchievementScore);
				level.goldAchievementScore = Mathf.Max(level.silverAchievementScore, level.goldAchievementScore);
				
				GUILayout.Space(20);
			}
			
			EditorGUILayout.EndScrollView();
		}
		if(GUILayout.Button("Save")){
			SaveToXml();
		}
		EditorGUILayout.EndVertical();
	}


	private void SaveToXml ()
	{
		Debug.Log("Writing new score values to the levels in xml");
		XmlDocument configXml = ConfigLoader.Instance.GetWorkingDocument();
		//removing the exsiting nodes
		XmlNodeList scoreNodes = configXml.SelectNodes("//Level/Score");
		Debug.Log("Deleting "+ scoreNodes.Count + " score nodes");
		foreach(XmlNode n in scoreNodes){
			n.ParentNode.RemoveChild(n);
		}
		//adding the new score nodes
		Debug.Log("Saving scores to " + levels.Count + " levels");
		foreach(var pair in levels){
			
			Level level = pair.Value;
			XmlNode levelNode = configXml.SelectSingleNode(string.Format("//Level[@name='{0}']", level.levelName));
			Debug.Log("Saving score to"+ level.levelName + ", levelNode != null = "+ (levelNode != null).ToString());
			XmlNode scoreXml = XmlVisitor.CreateAddNode(configXml, "Score", levelNode);
			XmlVisitor.CreateAddNode(configXml, "GoldScoreMinimum", scoreXml).InnerText = level.goldAchievementScore.ToString();
			XmlVisitor.CreateAddNode(configXml, "SilverScoreMinimum", scoreXml).InnerText = level.silverAchievementScore.ToString();
			XmlVisitor.CreateAddNode(configXml, "BronzeScoreMinimum", scoreXml).InnerText = level.bronzeAchievementScore.ToString();
			
		}
		
		string path = XmlVisitor.Write(configXml);
		UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal (path, 0);
		Debug.Log("Written new scores to level objects");
			
	}
	

}
