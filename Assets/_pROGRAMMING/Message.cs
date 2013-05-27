using UnityEngine;
using System.Collections;
using System.Xml;

public class Message : IVisitable
{
	private string _name;
	private string _text;
	private AudioClip _audio;
	private bool _isWarning = false;
	
	public Message (string name)
	{
		_name = name;
	}
	
	public Message (XmlNode node)
	{
<<<<<<< HEAD
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
=======
		_text = node ["Text"].InnerText;
>>>>>>> Doxygen and Message.IsWarning

		string audioFile = node ["Audio"].InnerText;
		_name = node ["Name"].InnerText;
		_audio = (AudioClip)Resources.Load (audioFile);
		if (node ["IsWarning"] != null)
			_isWarning = bool.Parse (node ["IsWarning"].InnerText);

>>>>>>> Fixes github diff text
	}
	
	public string name {
		get{ return _name;}
		set{ _name = value;}
	}

	public string text {
		get{ return _text;}
		set{ _text = value;}
	}
		
	public AudioClip audio {
		get { return _audio;}
		set{ _audio = value;}
	}

	public bool isWarning {
		get{ return _isWarning;}
		set{ _isWarning = value;}
	}
	
	public void AcceptVisitor (Visitor visitor)
	{
		visitor.Visit (this);
	}
	
}
