using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SiteLock : MonoBehaviour {
	public List<string> domains;
	public string redirectUrl;
	
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
