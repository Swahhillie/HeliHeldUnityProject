using UnityEngine;
using System.Collections;

public class Hud_test : MonoBehaviour {

	void Start () 
	{
		HudScript test = this.GetComponent<HudScript>();
		test.message="This is the Hud test.\nIt will store the last message\nand on ToggleHud()\nit will activate or deactivate the Hud\nThis message is stored in the T_hud class.\nYou can toogle the hud with 'A'\nThe background is a plane \nand can easily be replaced\nThe text formating is in the string so far.";
		//test.ToggleHud();
		//test.message="short text\ntest test";
	}
	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.A))
		{
			HudScript test = this.GetComponent<HudScript>();
			test.ToggleHud();
		}
		
	}

}
