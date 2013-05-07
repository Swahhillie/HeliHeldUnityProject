using UnityEngine;
using System.Collections;

public interface ITriggeredObject {

	void OnTriggered(EventReaction eventReaction, TriggerType triggerType);
}
