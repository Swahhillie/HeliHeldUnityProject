using UnityEngine;
using System.Collections;
using System.Xml;

public class Message
{
	private string _text;
	private string _audio;
	
	public Message(XmlNode node)
	{
		_text = node["Text"].InnerText;
		_audio = node["Audio"].InnerText;
	}
	
	public string text
	{
		get{return _text;}
	}
		
	public string audio
	{
		get {return _audio;}
	}
}
