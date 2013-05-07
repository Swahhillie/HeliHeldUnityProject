using UnityEngine;
using System.Collections;

public interface IVisitable {

	void AcceptVisitor(Visitor visitor);
	
}
