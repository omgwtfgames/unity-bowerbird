// Based on http://wiki.unity3d.com/index.php/TakeScreenshotInEditor 
// (original by Jonathan Czeck (aarku))

using UnityEngine;
using UnityEditor;
 
public class TakeScreenshotInEditor : ScriptableObject
{
    public static string fileName = "Editor Screenshot ";
    public static int startNumber = 1;
 
    [MenuItem ("Custom/Take Screenshot of Game View %^s")]
    static void TakeScreenshot()
    {
        int number = startNumber;
        string name = "" + number;
        string filepath = System.IO.Path.Combine(Application.dataPath, fileName + name + ".png");
		
        while (System.IO.File.Exists(filepath))
        {
            number++;
            name = "" + number;
        }
 
        startNumber = number + 1;
 
        Application.CaptureScreenshot(filepath);
    }
}