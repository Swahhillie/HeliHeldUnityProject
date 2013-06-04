using UnityEngine;
using System.Collections;

public class TerrainCloner : MonoBehaviour {

	public Vector3 AdjustPosition;
	public Material CloneMaterial;
	public int Layer;
	
	void Awake()
	{
		Terrain clone = (Terrain)Instantiate(this.gameObject);
		clone.transform.position = this.transform.position+AdjustPosition;
		clone.renderer.material = CloneMaterial;
		clone.gameObject.layer = 1<<Layer;
	}
	
}
