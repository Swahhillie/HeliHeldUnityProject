using UnityEngine;
using System.Collections;

public abstract class Visitor {
	public abstract void Visit(Trigger visitable);
	public abstract void Visit(Castaway visitable);
	public abstract void Visit(Ship visitable);
	public abstract void Visit(Button3D visitable);
	public abstract void Visit(Beacon visitable);
}
