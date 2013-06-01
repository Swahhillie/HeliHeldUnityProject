using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(GUITexture))]
public class GUITextureEditor : Editor {

	override public void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		GUITexture g = (GUITexture)target;
		EditorGUILayout.BeginVertical();
		{
			if(GUILayout.Button("Reset Inset")){
				if(g.texture != null)
				{
					g.pixelInset = new Rect(0,0, g.texture.width, g.texture.height);
				}
			}
		}
		EditorGUILayout.EndVertical();
		
	}
}
