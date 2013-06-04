using UnityEngine;
using System.Collections;
[System.Serializable]
public class HelmetElement : System.Object
{
	public Vector2 position;
	public Vector2 size;
	public Texture[] image;
	public float animationSpeed;

	
	private int _anim=0;
	
	public int anim
	{
		get{return _anim;}
	}
	
	private bool _active=true;
	
	public bool active
	{
		get{return _active;}
		set{_active=value;
			if(!_active)
			{
				_anim = 0;
			}
		}
	}
	
	public Texture activeImage
	{
		get{return image[_anim];}	
	}
	
	public void animateElement()
	{
		if(_active)
		{
			_anim = (int)Mathf.Clamp(Mathf.Sin(Time.time)*image.Length,0,image.Length-1);			
		}
	}
	
}

public class Helmet : MonoBehaviour 
{
	public MovieTexture BackgroundImage;
	public HelmetElement[] HelmetElements;
	
		
	void OnGUI()
	{
		GUI.Label(new Rect(0,0,Screen.width,Screen.height),new GUIContent(BackgroundImage));
		for(int i=0;i<HelmetElements.Length;++i)
		{
			GUI.Label(new Rect(HelmetElements[i].position.x,HelmetElements[i].position.y,HelmetElements[i].size.x,HelmetElements[i].size.y),new GUIContent(HelmetElements[i].activeImage));
		}	
	}
	
	void Update()
	{
		for(int i=0;i<HelmetElements.Length;++i)
		{
			HelmetElements[i].animateElement();
		}
	}
}
