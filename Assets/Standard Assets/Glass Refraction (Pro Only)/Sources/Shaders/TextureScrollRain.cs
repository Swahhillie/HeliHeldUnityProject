using UnityEngine;
using System.Collections;

public class TextureScrollRain : MonoBehaviour {

	// Use this for initialization
	public float scrollSpeed = 1.0f;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		renderer.material.SetTextureOffset("_BumpMap", Time.time * Vector2.up * scrollSpeed);// += ;
	}
}
