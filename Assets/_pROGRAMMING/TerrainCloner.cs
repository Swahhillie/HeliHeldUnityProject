using UnityEngine;
using System.Collections;

public class TerrainCloner : MonoBehaviour {

	public Vector3 AdjustPosition;
	public Material CloneMaterial;
<<<<<<< HEAD
	public int Layer;
=======
>>>>>>> Latest version
	
	void Awake()
	{
		Terrain clone = (Terrain)Instantiate(this.gameObject);
		clone.transform.position = this.transform.position+AdjustPosition;
		clone.renderer.material = CloneMaterial;
<<<<<<< HEAD
		clone.gameObject.layer = 1<<Layer;
=======
>>>>>>> Latest version
	}
	
}
