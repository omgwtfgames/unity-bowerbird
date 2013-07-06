using UnityEngine;
using System.Collections;

public class ConsolePopup : MonoBehaviour {
	public bool showButton = true;
	bool showConsole = false;
	string codeString = "";
	public bool enabled = false;
	
	KeyCode[] konami = {KeyCode.UpArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.DownArrow, 
		                KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.RightArrow,
	                    KeyCode.B, KeyCode.A };
	int konami_index = 0;
	
	void Update() {

	}
	
	void OnGUI() {
		Event e = Event.current;
		
		// konami code console activation
		if (!enabled && e.type == EventType.KeyUp && e.keyCode != konami[konami_index]) {
			konami_index = 0;
		}
		if (!enabled && e.keyCode == konami[konami_index] && e.type == EventType.KeyUp) {
			  konami_index++;
		}
		if (konami_index > konami.Length - 1) {
			enabled = true;
			showConsole = true;
		}

		if (enabled && e.keyCode == KeyCode.BackQuote && e.type == EventType.KeyDown) {
			showConsole = !showConsole;
			GUI.UnfocusWindow();// FocusControl("");
			if (!showConsole) codeString = "";
			return;
		}

		if (showConsole) {
			//Script editor GUI
			//GUI.skin = editorStyle;
			GUILayout.BeginArea(new Rect(50, Screen.height - 60, Screen.width-50, 50));
			GUILayout.BeginVertical();
			GUI.SetNextControlName("console");

			codeString = GUILayout.TextField(codeString, GUILayout.ExpandHeight(true));
			if (codeString.Equals("`")) codeString = ""; // get rid of the stray ` that is caught from opening
			GUI.FocusControl("console");
			GUILayout.BeginHorizontal();
			//running = GUILayout.Toggle(running, "Running");
			if (showButton) {
				if (GUILayout.Button("Execute")) Submit();
			}
			if (e.keyCode == KeyCode.Return && e.type == EventType.KeyUp) Submit();
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}

	}
	
	void Submit() {
		Debug.Log("Executing: " + codeString);
		gameObject.SendMessage("Execute", codeString, SendMessageOptions.DontRequireReceiver);
		codeString = "";
	}
}
