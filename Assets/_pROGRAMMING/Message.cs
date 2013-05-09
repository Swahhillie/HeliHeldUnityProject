using UnityEngine;
using System.Collections;
using System.Xml;

public class Message
{
	private string _text;
	private AudioClip _audio;
	
	public Message(XmlNode node)
	{
		_text = node["Text"].InnerText;
		string audioFile = "_ART/Sounds/"+node["Audio"].InnerText+".wav";
		
		_audio = (AudioClip)Resources.Load(audioFile);
	}
	
	public string text
	{
		get{return _text;}
	}
		
	public AudioClip audio
	{
		get {return _audio;}
	}
}
