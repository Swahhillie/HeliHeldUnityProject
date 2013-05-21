using UnityEngine;
using System.Collections;
using System.Xml;

public class Message : IVisitable
{
	private string _name;
	private string _text;
	private AudioClip _audio;
	
	public Message(string name)
	{
		_name = name;
	}
	
	public Message(XmlNode node)
	{
		_text = node["Text"].InnerText;
<<<<<<< HEAD
<<<<<<< HEAD
		_audio = node["Audio"].InnerText;
=======
		string audioFile = node["Audio"].InnerText;
		_name = node["Name"].InnerText;
		_audio = (AudioClip)Resources.Load(audioFile);
>>>>>>> Message editor utility added, UT for ParseVec3
=======

		string audioFile = node["Audio"].InnerText;
		_name = node["Name"].InnerText;
		_audio = (AudioClip)Resources.Load(audioFile);

>>>>>>> Fixes github diff text
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
