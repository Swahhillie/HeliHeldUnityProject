using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;

[CustomEditor(typeof(Trigger))]
public class TriggerEditor : Editor
{
	private bool[][] openStates;
	private Trigger trigger;
	
	private void OnEnable ()
	{
		trigger = (Trigger)target;
		
		//create the array that stores the open states of the triggers and event reactions
		CreateOpenStates ();
	}

	private void OnGUI ()
	{
		
	}

	override public void OnInspectorGUI ()
	{
		if (EditorGUILayout.Toggle ("Add", false)) {
			trigger.triggers.Add (new TriggerValue (
					new List<EventReaction> (), TriggerType.None, 0, 0, 0, 1));
				
			CreateOpenStates ();
		}
		EditorGUILayout.BeginVertical ();
		{
			
			for (var i = trigger.triggers.Count -1; i >= 0; i--) {
				EditorGUILayout.BeginVertical("Box");
				EditorGUILayout.LabelField("TriggerValue", EditorStyles.boldLabel);
				EditorGUI.indentLevel ++;
				var tv = trigger.triggers [i];
				TriggerValueInspector (tv, i);
				if (EditorGUILayout.Toggle ("Remove Trigger", false)) {
					trigger.triggers.RemoveAt (i);
					CreateOpenStates ();
				}
				EditorGUI.indentLevel--;
				EditorGUILayout.EndVertical();
				GUILayout.Space (20);

			}
		}
		EditorGUILayout.EndVertical ();
	}

	private void CreateOpenStates ()
	{
		openStates = new bool[trigger.triggers.Count][];
		for (var i = 0; i < trigger.triggers.Count; i++) {
			var tv = trigger.triggers [i];
			openStates [i] = new bool[tv.eventReactions.Count];
		}
	}

	private void TriggerValueInspector (TriggerValue tv, int tvIndex)
	{
		
		tv.type = (TriggerType)EditorGUILayout.EnumPopup (tv.type);
		EditorGUILayout.BeginVertical ();
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel ("Repeat Count"); 
		tv.maxRepeatCount = EditorGUILayout.IntField (tv.maxRepeatCount);
		EditorGUILayout.EndHorizontal ();
		switch (tv.type) {
		case TriggerType.Counting:
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.PrefixLabel ("Count to trigger"); 
			tv.countToTrigger = EditorGUILayout.IntField (tv.countToTrigger);
			EditorGUILayout.EndHorizontal ();
			break;
		case TriggerType.OnActivate:
			break;
		case TriggerType.OnTriggerEnter:
			
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.PrefixLabel ("Radius");
			tv.radius = EditorGUILayout.FloatField (tv.radius);
			EditorGUILayout.EndHorizontal ();
			break;
		case TriggerType.OnTriggerExit:
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.PrefixLabel ("Radius");
			tv.radius = EditorGUILayout.FloatField (tv.radius);
			EditorGUILayout.EndHorizontal ();
			break;
			
		case TriggerType.Timer:
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.PrefixLabel ("TimeToTrigger");
			tv.timeToTrigger = EditorGUILayout.FloatField (tv.timeToTrigger);
			EditorGUILayout.EndHorizontal ();
			break;
		}
		if (EditorGUILayout.Toggle ( "Add Reaction", false)) {
			tv.eventReactions.Add (new EventReaction ());
			CreateOpenStates ();
		}
		for (var i = tv.eventReactions.Count -1; i >= 0; i--) {
			
			
			openStates [tvIndex] [i] = EditorGUILayout.Foldout (openStates [tvIndex] [i], "EventReaction");
			EditorGUI.indentLevel ++;
			if (openStates [tvIndex] [i]) {
				var evr = tv.eventReactions [i];
				EventReactionInspector (evr);
				if (EditorGUILayout.Toggle ( "Remove Reaction", false)) {
					tv.eventReactions.RemoveAt (i);
					CreateOpenStates ();
				}
			}
			EditorGUI.indentLevel --;
		}
		EditorGUILayout.EndVertical ();
		
	}

	private void EventReactionInspector (EventReaction evr)
	{
		
		evr.type = (EventReaction.Type)EditorGUILayout.EnumPopup (evr.type);
		System.Type allowedListenerType = typeof(TriggeredObject);
		switch (evr.type) {
		case EventReaction.Type.Animate:
			LabeledField ("MessageName", () => evr.messageName = GUILayout.TextField (evr.messageName));
			allowedListenerType = typeof(HelmetAnimationHandler);
			break;
		case EventReaction.Type.Count:
			break;
		case EventReaction.Type.Displace:
			evr.pos = EditorGUILayout.Vector3Field ("Pos", evr.pos);
			break;
		case EventReaction.Type.LineGuide:
			evr.go = (GameObject)EditorGUILayout.ObjectField ("Object", evr.go, typeof(GameObject), true);
			allowedListenerType = typeof(LineGuide);
			break;
		case EventReaction.Type.Say:
			LabeledField ("MessageName", () => evr.messageName = EditorGUILayout.TextField (evr.messageName));
			allowedListenerType = typeof(Radio);
			break;
		case EventReaction.Type.Spawn:
			evr.go = (GameObject)EditorGUILayout.ObjectField ("Resource", evr.go, typeof(GameObject), false);
			break;
		case EventReaction.Type.SpecialScore:
			evr.specialScore = EditorGUILayout.IntField ("Score", evr.specialScore);
			allowedListenerType = typeof(Main); // might have to be mainplaceholder
			break;
		case EventReaction.Type.StartTimer:
			evr.time = EditorGUILayout.FloatField ("Time", evr.time);
			allowedListenerType = typeof(Trigger);
			break;
		}
		
		if (EditorGUILayout.Toggle ("Add Listener", false)) {
			evr.listeners.Add (null);
		}
		
		EditorGUILayout.BeginVertical ("label");
		//EditorGUILayout.PropertyField(evr.listeners, true);
		
		for (int i = evr.listeners.Count -1; i>= 0; i--) {
			
			evr.listeners [i] = (TriggeredObject)EditorGUILayout.ObjectField (evr.listeners [i], allowedListenerType, true);
			if (EditorGUILayout.Toggle ("Remove Listener",false)) {
				evr.listeners.RemoveAt (i);
			}
			
		}
		EditorGUILayout.EndVertical ();
	}

	private void LabeledField (string label, System.Action action)
	{
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel (label);
		action ();
		EditorGUILayout.EndHorizontal ();
		
	}
}
