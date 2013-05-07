using UnityEngine;
using System.Collections;

public class Button3D : MonoBehaviour {

	public delegate void Callback(string command);
	Callback callback = null;
	string command;
	
	
	
	private bool _isHovered; // is the player pointer at this button
	
	public static Button3D CreateButton(string text, string command, Callback c, Vector3 pos, Vector3 rot, string prefabName = "button3DPrefab"){
		GameObject go = (GameObject)Instantiate(Resources.Load(prefabName), pos, Quaternion.identity);
		go.transform.eulerAngles = rot;
		Button3D b = go.GetComponent<Button3D>();
		b.textMesh.text = text;
		b.command = command;
		b.callback = c;
		return b;
	}
	
	public void StartHover(){
		_isHovered = true;
		//do fancy graphic stuff here
	}
	public void EndHover(){
		_isHovered = false;
		//make button normal again
	}
	
	public void Activate(){
		callback(command);
	}
	
	private TextMesh _tMesh;
	public TextMesh textMesh{
		get{
			if(_tMesh == null)_tMesh = gameObject.GetComponent<TextMesh>();
			return  _tMesh;
		}
	}
}
