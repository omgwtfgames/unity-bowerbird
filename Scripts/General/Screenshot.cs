using UnityEngine;
using System.IO;
using System.Collections;

public class Screenshot : MonoBehaviour {
  private int count = 0;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	/*
	 *    http://jon-martin.com/?p=114
	 */
	public Texture2D ScaleTexture(Texture2D source,int targetWidth,int targetHeight) {
       Texture2D result=new Texture2D(targetWidth,targetHeight,source.format,true);
       Color[] rpixels=result.GetPixels(0);
       float incX=((float)1/source.width)*((float)source.width/targetWidth);
       float incY=((float)1/source.height)*((float)source.height/targetHeight);
       for(int px=0; px<rpixels.Length; px++) {
               rpixels[px] = source.GetPixelBilinear(incX*((float)px%targetWidth),
                                 incY*((float)Mathf.Floor(px/targetWidth)));
       }
       result.SetPixels(rpixels,0);
       result.Apply();
       return result;
	}
	
	// Based on: http://wiki.unity3d.com/index.php?title=TakeScreenshot
    // ******  Notice : It doesn't works in Web Player environment.  ******
	// ******    It works in PC environment.                         ******
	// Default method have some problem, when you take a Screen shot for your game. 
	// So add this script.
	// CF Page : http://technology.blurst.com/unity-jpg-encoding-javascript/
	// made by Jerry ( sdragoon@nate.com ) 
    IEnumerator TakeScreenshot() {
     			
		// wait for graphics to render
        yield return new WaitForEndOfFrame();
 
        // create a texture to pass to encoding
        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
 
        // put buffer into texture
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();
 
        // split the process up--ReadPixels() and the GetPixels() call inside of the encoder are both pretty heavy
        yield return 0;
 
        byte[] bytes = texture.EncodeToPNG();
 
        // save our test image (could also upload to WWW)
		string fn = Path.Combine(Application.persistentDataPath, "screenshot-" + count + ".png");
        File.WriteAllBytes(fn, bytes);
		Debug.Log ("Wrote screenshot to:" + fn);
        count++;
 
        // Added by Karl. - Tell unity to delete the texture, by default it seems to keep hold of it and memory crashes will occur after too many screenshots.
        DestroyObject( texture );
 
        //Debug.Log( Application.dataPath + "/../testscreen-" + count + ".png" );
    }
}
