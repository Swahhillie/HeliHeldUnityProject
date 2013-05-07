using UnityEngine;
using System.Collections;

public class RadioTextHandler : MonoBehaviour
{
	TextMesh textMesh;
	
	void Start ()
	{
		textMesh = this.GetComponent<TextMesh>();
		textMesh.font.material.color = Color.green;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	public void setText( string aText )
	{
		string radioText = "";
		for(int i = 0; i < aText.Length; i+=16)
		{
			int end = 16;
			if(i+16 > aText.Length)
				end = aText.Length-i;
			string tempStr = aText.Substring(i, end);
			tempStr += "\n";
			radioText += tempStr;
		}
		textMesh.text = radioText;
	}
}
