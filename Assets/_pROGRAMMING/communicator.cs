using UnityEngine;
using System.Collections;

public class communicator : MonoBehaviour {
	// Server communicatie links
	private string score_uploaden_url = "http://192.168.137.1/addscore.php?";
	
	// Gebruiker variabelen
	public string UserName;
	public string RFIDTAG;
	public string Gender;
	public string UserKleur;
	public float Score = 0;
	
	// Game variabelen
	public bool[] minigames = new bool[7];
	
	// Binnengehaalde variabelen
	private GameObject Backbone_instance;
	public dummyscript BackboneScript;
	
	void Awake () {
		
	}
	
	// Start functie
	void Start () {
		Backbone_instance = GameObject.Find("Backbone");
		BackboneScript = Backbone_instance.GetComponent<dummyscript>();
		AssignUserVariables();
		ReadPlayedGames();
	}
	
	// Update functie
	void Update () {
		
	}
	
	void AssignUserVariables(){
		UserName = BackboneScript.UserName;
		RFIDTAG = BackboneScript.RFIDTAG;
		Gender = BackboneScript.Gender;
		UserKleur = BackboneScript.UserKleur;
	}
	
	void ReadPlayedGames(){
		for(var i = 0; i<=6;i++){
			minigames[i] = BackboneScript.minigamesplayed[i];
		}
	}
	
	public void TestFunctie (string zender) {
		print("Groeten uit "+zender+"!");	
	}
	
	public IEnumerator ScoreUploaden (float minigame, float score) {
		print("SIMULATIE: Bezig met score uploaden ("+score+" punten naar minigame #"+minigame+") ...");
		
        string post_url = score_uploaden_url + "tag=" + RFIDTAG + "&score=" + score + "&minigame=" +minigame;

        WWW www = new WWW(post_url);
		yield return www;
		
        if (www.error != null)
        {
            print("SIMULATIE: Score succesvol ge-upload naar de database!");
        }
		else{
			print("SIMULATIE: Score succesvol ge-upload naar de database!");
		}
    }
}