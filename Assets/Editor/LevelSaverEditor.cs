using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class LevelSaverEditor : EditorWindow
{
	
	private GameObject[] _selectedGos = new GameObject[0];
	
	
	private static XMLVisitor.ToSave target;
	private string saveName = "newXml";
	
	
	[MenuItem("SaveXML/Level")]
	private static void CreateWindowSaveLevel ()
	{
		EditorWindow.GetWindow<LevelSaverEditor> (true, "Saving a level");
		target = XMLVisitor.ToSave.Level;
	}

	[MenuItem("SaveXML/Menu")]
	private static void CreateWindowSaveMenu ()
	{
		EditorWindow.GetWindow<LevelSaverEditor> (true, "Saving a menu");
		target = XMLVisitor.ToSave.Menu;
	}
	[MenuItem("SaveXML/Level", true)]
	private static bool ShowWindow ()
	{
		return true;
	}
	
	private void OnSelectionChange ()
	{
		if (target == XMLVisitor.ToSave.Level) { 
			int targetLayer = LayerMask.NameToLayer ("SaveLayer");
			_selectedGos = System.Array.FindAll<GameObject> (Selection.gameObjects, (go => go.layer == targetLayer && go.transform.parent == null));
		} else if (target == XMLVisitor.ToSave.Menu) {
			_selectedGos = System.Array.FindAll<GameObject> (Selection.gameObjects, go => go.GetComponent<Button3D>() != null);
		}
		Repaint ();
	}

	private Vector2 _scrollPosition;

	private void OnGUI ()
	{
		_scrollPosition = EditorGUILayout.BeginScrollView (_scrollPosition);
		EditorGUILayout.BeginHorizontal ();
		{
			saveName = GUILayout.TextField(saveName, 15);
			
			if (GUILayout.Button ("Save")) {
				SaveToXml ();
				
			}
		}
		EditorGUILayout.EndHorizontal ();
		{
			foreach (var go in _selectedGos) {
				EditorGUILayout.BeginHorizontal ();
				{
					EditorGUILayout.PrefixLabel (go.name);
				}
				EditorGUILayout.EndHorizontal ();
			}
		}
		EditorGUILayout.EndScrollView ();
	}

	private void SaveToXml ()
	{
		int i = 0;
		
		//System.Array.ForEach<GameObject>(toSaveGo, (x) => {x.name = go.name + " " + i.ToString("000") + " "; i++;});
		foreach (var go in _selectedGos) {
			
			go.name = go.name + "_" + i.ToString ("000") + "_"; //give all gos that will be safed a unique name
			
			i++;
		}
		XMLVisitor xmlVisitor = new XMLVisitor (saveName, target);
		foreach (var go in _selectedGos) {
			
			if(target == XMLVisitor.ToSave.Level)xmlVisitor.OpenNewObject(go);
			System.Array.ForEach<IVisitable> (go.GetInterfacesInChildren<IVisitable> (), visitable => visitable.AcceptVisitor (xmlVisitor));
			
		}
		//cleanup names
		foreach (var go in _selectedGos) {
			go.name = go.name.Substring (0, go.name.Length - 5); // leave the number out of it
		}
		string resultPath = xmlVisitor.Write();
		OpenOutputFile(resultPath);
	}
	private void OpenOutputFile(string path)
	{
		UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(path, 0);
	}
}
