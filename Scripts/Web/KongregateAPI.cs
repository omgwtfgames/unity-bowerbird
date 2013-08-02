using UnityEngine;
using System.Collections;

public class KongregateAPI : MonoBehaviour {
    // TODO: callback for user signin event
	// http://www.kongregate.com/developer_center/docs/en/using-the-api-with-unity3d
	
	public static bool isKongregate = false;
	public static int userId = 0;
	public static string username = "Guest";
	public static string gameAuthToken = "";
	
	void Awake() {
		// This game object needs to survive multiple levels
		DontDestroyOnLoad(this);
		 
		#if UNITY_WEBPLAYER
		// Begin the API loading process if it is available
		Application.ExternalEval(
		  "if(typeof(kongregateUnitySupport) != 'undefined'){" +
		  " kongregateUnitySupport.initAPI('KongregateAPI', 'OnKongregateAPILoaded');" +
		  "}"
		);
    #endif
	}

	public static void OnKongregateAPILoaded(string userInfoString){
	  // We now know we're on Kongregate
	  isKongregate = true;
	  Debug.Log("KONG ON !");
	 
	  // Split the user info up into tokens
	  string[] param = userInfoString.Split("|"[0]);
	  userId = int.Parse(param[0]);
	  username = param[1];
	  gameAuthToken = param[2];
	}

	public static void submitStat(string name, int value) {
		#if UNITY_WEBPLAYER
		Application.ExternalCall("kongregate.stats.submit", name, value);
    #endif
	}
}
