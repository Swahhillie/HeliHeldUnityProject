using UnityEngine;
using System.Collections;
using System.Reflection;
[RequireComponent(typeof(GUIText))]
public class Text2D : MonoBehaviour {
	
	[HideInInspector]
	public string fieldToRead;
	
	
	public string format = "{0}";
	private GameStats gameStats;
	private FieldInfo fieldInfo;
	
	
	public void Start()
	{
		gameStats = ((Menu2D)FindObjectOfType(typeof(Menu2D))).gameStats;
		fieldInfo = typeof(GameStats).GetField(fieldToRead);
	}
	public void Update()
	{
		guiText.text = string.Format(format, fieldInfo.GetValue(gameStats).ToString());
	}
}
