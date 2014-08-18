using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SiteLock : MonoBehaviour {
	public string[] domains = { "kongregate.com", "gamejolt.com", "gamejolt.net", "itch.io" };
	public string redirectUrl = "http://omgwtfgames.com";
	
	void Awake () {
    #if UNITY_WEBPLAYER
		string jsarray = "[";
		foreach (string domain in domains) {
			jsarray += "'"+domain+"',";
		}
		jsarray += "]";
		Application.ExternalEval("function contains(a, obj) { " +
			                       "var i = a.length; " +
			                       "while (i--) { if (a[i] === obj) { return true;}} " +
			                       "return false;" +
			                      "} "+
			                     "if(contains("+jsarray+", document.location.host)) {} else { document.location='"+redirectUrl+"'; }");
    #endif
    }
}
