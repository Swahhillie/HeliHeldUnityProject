using UnityEngine;
using System.Collections;

public class Button3D : MonoBehaviour, IVisitable {

	public enum Type
	{
		LoadMenu,
		LoadLevel
	}
	
	public delegate void Callback(Button3D thisObj);
	Callback callback = null;
	public string command;
	public Button3D.Type type;
		
	public static Button3D CreateButton(Button3D.Type type, string text, string command, Callback c, Vector3 pos, Vector3 rot, string prefabName){
		
		GameObject go = (GameObject)Instantiate(Resources.Load(prefabName), pos, Quaternion.identity);
		go.transform.eulerAngles = rot;
		Button3D b = go.GetComponent<Button3D>();
		b.type = type;
		b.textMesh.text = text;
		b.command = command;
		b.callback = c;
		return b;
	}
	

	public void Activate(){
		callback(this);
	}
	
	private TextMesh _tMesh;
	public TextMesh textMesh{
		get{
			if(_tMesh == null)_tMesh = gameObject.GetComponent<TextMesh>();
			return  _tMesh;
		}
	}
	public void AcceptVisitor(Visitor v){
		v.Visit(this);
	}
}
