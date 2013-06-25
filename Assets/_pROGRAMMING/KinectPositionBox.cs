using UnityEngine;
using System.Collections;

public class KinectPositionBox : MonoBehaviour
{
	public bool Activated = false;
	
	public float DeactivationTime = 4.0f;
	public float ExitTime = 30.0f;
	public string OutTheBoxText = "Ga bij de groene stip staan.";
	private string labelString;
	public GUIStyle labelStyle;
	private float timeSpend = 0.0f;
	private float exitTime = 0.0f;
	
	public SkeletonWrapper skelWrap;
	
	public Texture2D kinPosBGTex;
	public Texture2D kinPosPlayerTex;
	
	//limits are -1 to 1
	public float sidesLimit = 0.5f;
	public float proximityLimit = 0.0f;
	public float distanceLimit = -0.9f;
	
	public Vector2 boxPos = new Vector2 (50, 50);
	private Vector2 boxSize;
	private Vector2 idealKinectPosition;
	
	private Vector3 playerPos = new Vector3();
	private Vector2 playerOnScreen = new Vector2();
	
	private KinectGestures kg;
	
	void Start()
	{
		kg = new KinectGestures(skelWrap);
		
		kinPosBGTex = Resources.Load ("KinectPositionBG", typeof(Texture2D)) as Texture2D;
		kinPosPlayerTex = Resources.Load ("KinectPositionPlayer", typeof(Texture2D)) as Texture2D;
		
		boxSize = new Vector2 (Screen.width-boxPos.x*2, Screen.height-boxPos.y*2);
		idealKinectPosition = new Vector2(Screen.width/2, Screen.height/4);
	}
	
	void Update()
	{
		if(!skelWrap.devOrEmu.device.connected) return;
		
		playerPos = kg.GetPlayerPosition();
		
		//limits
		if (playerPos.x > 1) {
			playerPos.x = 1;
		}
		if (playerPos.x < -1) {
			playerPos.x = -1;
		}
		if (playerPos.z > 1) {
			playerPos.z = 1;
		}
		if (playerPos.z < -1) {
			playerPos.z = -1;
		}
		
		playerOnScreen = new Vector2 (playerPos.x * (boxSize.x * 0.5f), playerPos.z * (boxSize.y * 0.5f));
		playerOnScreen += idealKinectPosition;
		playerOnScreen.y = Screen.height - playerOnScreen.y;
		
		if( 
			playerPos.x > sidesLimit || 
			playerPos.x < -sidesLimit || 
			playerPos.z > proximityLimit || 
			playerPos.z < distanceLimit
			//|| playerPos == Vector3.zero
		   )
		{
			Activated = true;
			labelString = OutTheBoxText;
			timeSpend = 0.0f;
			exitTime += Time.deltaTime;
			if(exitTime > ExitTime)
			{
				Debug.Log("No-one found for a while. Exit to menu");
				Main main = (Main)FindObjectOfType(typeof(Main));
				main.ReturnToMaster();
				exitTime = 0.0f;
			}
		}
		else
		{
			exitTime = 0.0f;
			timeSpend += Time.deltaTime;
			if(timeSpend > DeactivationTime)
			{
				Activated = false;
			}
			labelString = (DeactivationTime-timeSpend).ToString("0.0");
		}
	}
	
	void OnGUI()
	{
		if(Activated && skelWrap.devOrEmu.device.connected)
		{
			GUI.DrawTexture (new Rect (boxPos.x, boxPos.y, boxSize.x, boxSize.y), kinPosBGTex);
			GUI.Label(new Rect( Screen.width/4, 10, 600, 100 ), labelString, labelStyle);
			GUI.DrawTexture (new Rect(playerOnScreen.x-kinPosPlayerTex.width/2, playerOnScreen.y-kinPosPlayerTex.height/2, kinPosPlayerTex.width, kinPosPlayerTex.height), kinPosPlayerTex);
		}
	}
}
