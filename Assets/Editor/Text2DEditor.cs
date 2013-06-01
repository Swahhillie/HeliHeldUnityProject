using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

[CustomEditor(typeof(Text2D))]
public class Text2DEditor : Editor {
	private int selected = 0;
	private Text2D txt = null;
	private bool choose = false;
	List<FieldInfo> fields = new List<FieldInfo>();
	
	public void OnEnable()
	{
		txt = (Text2D)target;
		
		//get the function names from the menu2d class
		FieldInfo[] gameStatsFields = typeof(GameStats).GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
		
		//add all the names to a list for use by the list box
		//System.Array.ForEach<MethodInfo>(menuFunctions, x=>functionsInMenu2D.Add(x.Name));
		fields.AddRange(gameStatsFields);
		
		selected = fields.FindIndex(x => x.Name== txt.fieldToRead);
		if(selected == -1)
		{
			selected = 0;
		}

	}
	override public void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		
		GUILayout.BeginVertical();
		{
			choose = GUILayout.Toggle(choose, "Choose Field");
			if(choose){
				var names = new string[fields.Count];
				for(int i= 0; i < names.Length; i++)names[i] = fields[i].Name;
				
				selected = 	GUILayout.SelectionGrid(selected, names , 1);
				txt.fieldToRead = fields[selected].Name;
				
			}
			else{
				
				GUILayout.BeginHorizontal();
				{
					EditorGUILayout.PrefixLabel("Field To Print");
					if(txt.fieldToRead == null){
						EditorGUILayout.LabelField("Not Set");
					}
					else{
						EditorGUILayout.LabelField(txt.fieldToRead);	
					}
					
				}
				GUILayout.EndHorizontal();
			}
			
		}
		GUILayout.EndVertical();
		
		
	}
}
