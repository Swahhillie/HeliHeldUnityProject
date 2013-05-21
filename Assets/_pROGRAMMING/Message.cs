using UnityEngine;
using System.Collections;
using System.Xml;

public class Message : IVisitable
{
	private string _name;
	private string _text;
	private AudioClip _audio;
	
	public Message(XmlNode node)
	{
		_text = node["Text"].InnerText;

		string audioFile = node["Audio"].InnerText;
		_name = node["Name"].InnerText;
		_audio = (AudioClip)Resources.Load(audioFile);

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
		
	public AudioClip audio
	{
		get {return _audio;}
		set{_audio = value;}
	}
	
	public void AcceptVisitor (Visitor visitor)
	{
		visitor.Visit(this);
	}
	
}
