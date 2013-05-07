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
	OnActivate
}
public enum MissionObject{
	None,
	Ship,
	Castaway
}