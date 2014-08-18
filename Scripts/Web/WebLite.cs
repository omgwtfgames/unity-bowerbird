// from https://gist.github.com/michaelbartnett/4641633
// By Michael Bartnett, 2013.

// TODO: Write a replacement for the generic delegate Action<WWW>
//       eg, 
//       public delegate void Action<T>(T item);
//
//       so that we can define a callback like:
//
//       void WhenDone(WWW www) { // do something };
//       Action<WWW> whenDoneCallback = new Action<WWW>(WhenDone);
//
//       whenDoneCallback is what we would pass into WebLite.Get as the 
//       onComplete callback

using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Text;


/// <summary>
/// Bare minimum wrapper for UnityEngine.WWW.
///
/// You want form data? Pass that shit in yourself.
/// </summary>
public class WebLite : MonoBehaviour
{
    public void Post(string uri, string body, Action<WWW> onComplete=null, Hashtable headers=null)
    {
        Post(uri, Encoding.UTF8.GetBytes(body), onComplete, headers);
    }

    public void Post(string uri, byte[] body, Action<WWW> onComplete=null, Hashtable headers=null)
    {
        //DebugUtils.Assert(body != null, "Must supply POST data.");
        //Create a request to DB server address with attached post form data 
        StartCoroutine(WaitForResponse(new WWW(uri, body, headers), onComplete));
    }

    public WWW PostBlocking(string uri, string body, Hashtable headers=null)
    {
        return PostBlocking(uri, Encoding.UTF8.GetBytes(body), headers);
    }

    public WWW PostBlocking(string uri, byte[] body, Hashtable headers=null)
    {
        //DebugUtils.Assert(body != null, "Must supply POST data.");
        var www = new WWW(uri, body, headers);
        while (!www.isDone) { }
        return www;
    }

    public void Get(string uri, Action<WWW> onComplete)
    {
        //Create a request to server address for get 
        StartCoroutine(WaitForResponse(new WWW(uri), onComplete));
    }

    public void Get(string uri, byte[] query, Action<WWW> onComplete)
    {
        StartCoroutine(WaitForResponse(new WWW(uri, query), onComplete));
    }

    public WWW GetBlocking(string uri)
    {
        var www = new WWW(uri);
        while (!www.isDone) { }
        return www;
    }

    public WWW GetBlocking(string uri, byte[] query)
    {
        var www = new WWW(uri, query);
        while (!www.isDone) { }
        return www;
    }

    private IEnumerator WaitForResponse(WWW www, Action<WWW> onComplete)
    {
        yield return www;

        if (www.error != null) {
            Debug.LogWarning(string.Format("Error: {0} URL:{1}", www.error, www.url));
        }

        if (onComplete != null) { onComplete(www); }
    }

    public static WebLite Instance {
        get {
            if (_instance == null) {
                var go = new GameObject("WebLite");
                _instance = go.AddComponent<WebLite>();
    			DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }
    private static WebLite _instance;
}
