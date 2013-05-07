using UnityEngine;
using System.Collections;

public class XMLVisitor : Visitor {
	
	override public void Visit(Trigger v){
		Debug.Log("Visiting a trigger");
	}
	
	override public void Visit(Castaway v){
		Debug.Log("Visiting a Castaway");
	}
	
	override public void Visit(Ship v){
		Debug.Log("Visiting a Ship");
	}
}
