using UnityEngine;
using System.Collections;

public class RescueNearbyIndicator : MonoBehaviour {
	public Material defaultMat;
	public Material activeMat;
	
	public Renderer rendererToSwap;
	
	private bool _isActivated;
	
	public void Activate()
	{
		if(!_isActivated)rendererToSwap.material = activeMat;
		_isActivated = true;
	}
	public void DeActivate()
	{
		if(_isActivated)rendererToSwap.material = defaultMat;
		_isActivated = false;
	}
}
