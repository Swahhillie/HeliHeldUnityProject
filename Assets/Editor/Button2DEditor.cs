using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

[CustomEditor(typeof(Button2D))]
public class Button2DEditor : Editor {
	private int selected = 0;
	private Button2D b = null;
	private bool chooseFunction = false;
	List<MethodInfo> functionsInMenu2D = new List<MethodInfo>();
	
	public void OnEnable()
	{
		b = (Button2D)target;
		
		//get the function names from the menu2d class
		MethodInfo[] menuFunctions = typeof(Menu2D).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
		
		//add all the names to a list for use by the list box
		//System.Array.ForEach<MethodInfo>(menuFunctions, x=>functionsInMenu2D.Add(x.Name));
		functionsInMenu2D.AddRange(menuFunctions);
		
		selected = functionsInMenu2D.FindIndex(x => x== b.activateFunction);
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
			chooseFunction = GUILayout.Toggle(chooseFunction, "Choose Function");
			if(chooseFunction){
				var names = new string[functionsInMenu2D.Count];
				for(int i= 0; i < names.Length; i++)names[i] = functionsInMenu2D[i].Name;
				
				selected = 	GUILayout.SelectionGrid(selected, names , 1);
				b.activateFunction = functionsInMenu2D[selected];
				
			}
			else{
				
				GUILayout.BeginHorizontal();
				{
					EditorGUILayout.PrefixLabel("FunctionToCall");
					if(b.activateFunction == null){
						EditorGUILayout.LabelField("Not set");
					}
					else{
						EditorGUILayout.LabelField(b.activateFunction.Name);	
					}
					
				}
				GUILayout.EndHorizontal();
			}
			
		}
		GUILayout.EndVertical();
		
		
	}
}
