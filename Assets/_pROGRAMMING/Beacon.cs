using UnityEngine;
using System.Collections;

public class Beacon : MissionObjectBase {

	protected override void AwakeConcrete ()
	{
		
	}
	protected override void UpdateConcrete ()
	{
		
	}
	public override void AcceptVisitor (Visitor v)
	{
		v.Visit(this);
	}
}
