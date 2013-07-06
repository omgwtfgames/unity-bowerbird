using UnityEngine;
using System.Collections;

public class ConsoleMacros : MonoBehaviour {

	void Start () { }
	
	void Execute(string command) {
		string[] parts = command.Split(new char[] {' '}, 2);
		string macro = parts[0];
		string param = parts[1];
		
		if (macro.Equals("echo")) Echo(param);
	}
	
	void Echo(string text) {
		Debug.Log(text);
	}
	
}
