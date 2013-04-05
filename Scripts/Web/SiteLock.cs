using UnityEngine;
using System;
using System.Collections;

public class SiteLock : MonoBehaviour {
	public string domain;
	public string redirectUrl;
	
	void Awake () {
		Application.ExternalEval("if(document.location.host != '"+domain+
			                     "') { document.location='"+redirectUrl+
			                     "'; }");
	}
}	
