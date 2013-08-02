// Based on http://wiki.unity3d.com/index.php/TakeScreenshotInEditor 
// (original by Jonathan Czeck (aarku))

using UnityEngine;
using UnityEditor;

public class TakeScreenshotInEditor : ScriptableObject
{
    public static string suffix = "Editor Screenshot ";
    public static int startNumber = 1;
 
    [MenuItem ("Custom/Take Screenshot of Game View %^s")]
    static void TakeScreenshot()
    {
        int number = startNumber;
        string date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:FFF ");
		string filepath = System.IO.Path.Combine(Application.dataPath, date + suffix + number.ToString() + ".png");
		
		    /*
        // disabled file existence check and added timestamp instead,
        // since this seems to get into an infinite loop
        // if called when the editor is paused.
        while (System.IO.File.Exists(filepath))
        {
            number++;
            name = "" + number;
			      if (number > 999) break;
        }
       */
 
        startNumber = number + 1;
 
        Application.CaptureScreenshot(filepath);
    }
}
