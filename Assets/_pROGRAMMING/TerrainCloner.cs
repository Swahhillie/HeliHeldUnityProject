using UnityEngine;
using System.Collections;

public class TerrainCloner : MonoBehaviour {

	public Vector3 AdjustPosition;
	public Material CloneMaterial;
	
	void Awake()
	{
		Terrain clone = (Terrain)Instantiate(this.gameObject);
		clone.transform.position = this.transform.position+AdjustPosition;
		clone.renderer.material = CloneMaterial;
	}
	
}
