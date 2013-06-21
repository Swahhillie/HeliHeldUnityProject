using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;

[CustomEditor(typeof(Trigger))]
public class TriggerEditor : Editor
{
	
	private Trigger trigger;
	
	private void OnEnable ()
	{
		trigger = (Trigger)target;
	}

	private void OnGUI ()
	{
		
	}

	override public void OnInspectorGUI ()
	{
		EditorGUILayout.BeginVertical ();
		{
			if (GUILayout.Button ("Add"))
			{
				trigger.triggers.Add (new TriggerValue (
					new List<EventReaction> (), TriggerType.None, 0, 0, 0, 1));
			}
			for (var i = trigger.triggers.Count -1; i >= 0; i--)
			{
				var tv = trigger.triggers [i];
				TriggerValueInspector (tv);
				if (GUILayout.Button ("Remove", GUILayout.MaxWidth (70)))
				{
					trigger.triggers.RemoveAt (i);
				}
				GUILayout.Space (20);
			}
		}
		EditorGUILayout.EndVertical ();
	}

	private void TriggerValueInspector (TriggerValue tv)
	{
		tv.type = (TriggerType)EditorGUILayout.EnumPopup (tv.type);
		EditorGUILayout.BeginVertical ("Box");
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel ("Repeat Count"); 
		tv.maxRepeatCount = EditorGUILayout.IntField (tv.maxRepeatCount);
		EditorGUILayout.EndHorizontal ();
		switch (tv.type)
		{
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
		if (GUILayout.Button ("Add Reaction"))
		{
			tv.eventReactions.Add (new EventReaction ());
		}
		for (var i = tv.eventReactions.Count -1; i >= 0; i--)
		{

			var evr = tv.eventReactions [i];
			EventReactionInspector (evr);
			if (GUILayout.Button ("Remove", GUILayout.MaxWidth (70)))
			{
				tv.eventReactions.RemoveAt (i);
			}
			GUILayout.Space (20);
			
			
		}
		EditorGUILayout.EndVertical ();
		
	}

	private void EventReactionInspector (EventReaction evr)
	{
		
		evr.type = (EventReaction.Type)EditorGUILayout.EnumPopup (evr.type);
		System.Type allowedListenerType = typeof(TriggeredObject);
		switch (evr.type)
		{
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
			evr.go = (GameObject)EditorGUILayout.ObjectField("Object", evr.go, typeof(GameObject), true );
			allowedListenerType = typeof(LineGuide);
			break;
		case EventReaction.Type.Say:
			LabeledField("MessageName", () => evr.messageName = GUILayout.TextField(evr.messageName));
			allowedListenerType = typeof(Radio);
			break;
		case EventReaction.Type.Spawn:
			evr.go = (GameObject)EditorGUILayout.ObjectField("Resource", evr.go, typeof(GameObject), false);
			break;
		case EventReaction.Type.SpecialScore:
			evr.specialScore = EditorGUILayout.IntField("Score", evr.specialScore);
			allowedListenerType = typeof(Main); // might have to be mainplaceholder
			break;
		case EventReaction.Type.StartTimer:
			evr.time = EditorGUILayout.FloatField("Time", evr.time);
			allowedListenerType = typeof(Trigger);
			break;
		}
		
		if(GUILayout.Button("Add Listener", GUILayout.MaxWidth(70))){
			evr.listeners.Add(null);
		}
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.Space(20);
		EditorGUILayout.BeginVertical("Box");
		for(int i = evr.listeners.Count -1; i>= 0; i--)
		{
			evr.listeners[i] = (TriggeredObject)EditorGUILayout.ObjectField(evr.listeners[i], allowedListenerType, true);
			if(GUILayout.Button("Remove Listener", GUILayout.MaxWidth(70)))
			{
				evr.listeners.RemoveAt(i);
			}
		}
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();
	}

	private void LabeledField (string label, System.Action action)
	{
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel (label);
		action ();
		EditorGUILayout.EndHorizontal ();
		
	}
}
