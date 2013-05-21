using UnityEngine;
using System.Collections;
using System.Xml;

public class Message : IVisitable
{
	private string _name;
	private string _text;
	private string _audio;
	
	public Message(XmlNode node)
	{
		_text = node["Text"].InnerText;
<<<<<<< HEAD
		string audioFile = node["Audio"].InnerText;
		_name = node["Name"].InnerText;
		_audio = (AudioClip)Resources.Load(audioFile);
=======
		_audio = node["Audio"].InnerText;
>>>>>>> origin/highlight,-animations-and-sounds
	}
	
	public string name
	{
		get{return _name;}
		set{_name = value;}
	}
	public string text
	{
		get{return _text;}
		set{_text = value;}
	}
		
	public string audio
	{
		get {return _audio;}
		set{_audio = value;}
	}
	
	public void AcceptVisitor (Visitor visitor)
	{
		visitor.Visit(this);
	}
	
}
