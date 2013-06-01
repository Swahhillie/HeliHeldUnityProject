using UnityEngine;
using System.Collections;
using System.Reflection;
[System.Serializable]
public class ButtonActivateEventArgs : System.EventArgs
{

	private MethodInfo _function = null;
	
	public MethodInfo Function {
		get{ return _function;}
		set {
			if (_function != value) {
				Debug.Log("Changed button event function to -> " + value);
			}
			_function = value;
		}
		
	}
}
