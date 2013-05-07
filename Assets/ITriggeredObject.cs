using UnityEngine;
using System.Collections;

public interface ITriggeredObject {
	//dennis will implement a reaction class
	void OnTriggered(EventReaction eventReaction, TriggerType triggerType);
}
