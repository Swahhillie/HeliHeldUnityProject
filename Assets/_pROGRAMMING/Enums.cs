using UnityEngine;
using System.Collections;

public enum Reaction{
	None,
	Destroy,
	Spawn,
	Rescued,
	OutOfLive
}

public enum TriggerType{
	None,
	OnTriggerEnter,
	OnTriggerExit,
	OnDeath,
	OnSpawn,
	OnRescued,
	OnOutOfLive,
	Counting,
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