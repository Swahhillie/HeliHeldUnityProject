using UnityEngine;
using System;
using System.Collections;
using System.IO;

public class TextureSaver : MonoBehaviour 
{
	public Texture2D texture;
	
	// Use this for initialization
	void Start () 
	{
		SaveTextureToFile();
	}
	
	public void SaveTextureToFile()
	{ 
		var bytes = texture.EncodeToPNG(); 
		File.WriteAllBytes(Application.dataPath + "/exported_texture.png", bytes);	
	}
}
