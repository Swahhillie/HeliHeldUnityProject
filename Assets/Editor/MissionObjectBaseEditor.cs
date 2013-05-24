
using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomEditor(typeof(MissionObjectBase))]
public class MissionObjectBaseEditor : Editor
{

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		MissionObjectBase mib = target as MissionObjectBase;
		if(GUI.changed && mib.prefab != null)
		{
			if(Resources.Load(mib.prefab.name) == null)
			{
				mib.prefab = null;
				Debug.LogError("Prefab is not in the resource folder");
			}
		}
	}
}
