using UnityEngine;
using System.Collections;

public abstract class TriggeredObject : MonoBehaviour{
	//dennis will implement a reaction class
	public abstract void OnTriggered(EventReaction eventReaction);
}
