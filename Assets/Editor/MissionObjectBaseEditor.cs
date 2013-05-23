using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(MissionObjectBase), true)]
public class MissionObjectBaseEditor : Editor
{

	public override void OnInspectorGUI ()
	{
		//base.OnInspectorGUI ();
		MissionObjectBase mib = target as MissionObjectBase;
		EditorGUILayout.BeginVertical ();
		{
			EditorGUILayout.BeginHorizontal ();
			{
				EditorGUILayout.PrefixLabel ("Spawn");
				mib.spawn = (MissionObjectBase.SpawnType)EditorGUILayout.EnumPopup (mib.spawn);
			}
			EditorGUILayout.EndHorizontal ();
		
			EditorGUILayout.BeginHorizontal ();
			{
				EditorGUILayout.PrefixLabel ("Spawn");
				mib.prefab = EditorGUILayout.ObjectField(mib.prefab, typeof(GameObject), false) as GameObject;
				if(GUI.changed && mib.prefab != null)
				{
					if(Resources.Load(mib.prefab.name) == null)
					{
						mib.prefab = null;
						Debug.LogError("Prefab is not in the resource folder");
					}
				}
			}
			EditorGUILayout.EndHorizontal ();
		}
		EditorGUILayout.EndVertical ();
		
	}
}
