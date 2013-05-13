using UnityEngine;
using System.Collections;

public class dummyscript : MonoBehaviour {
	
	// Gebruikersvariabelen zoals later wordt doorgegeven de originele backbone
	public string UserName = "JAN";
	public string RFIDTAG = "0400dee0462642012";
	public string Gender = "M";
	public string UserKleur = "BLAUW";
	public bool[] minigamesplayed = new bool[7];
	
	// Wakker worden en niet dood gaan
	void Awake() {
    	DontDestroyOnLoad(transform.gameObject);
    }
	
	// Start functie
	void Start () {
		
	}
	
	// Update functie
	void Update () {
		
	}
}
