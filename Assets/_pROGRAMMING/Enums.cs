using UnityEngine;
using System.Collections;

public enum Reaction{
	None,
	Destroy,
	Spawn,
	Rescued
}

public enum TriggerType{
	None,
	OnTriggerEnter,
	OnTriggerExit,
	OnDestroy,
	OnSpawn,
	OnRescued,
	Counting,
	Timer,
	OnActivate
}
public enum MissionObject{
	None,
	Ship,
	Castaway
}

public enum Language{
	Dutch,
	English
}