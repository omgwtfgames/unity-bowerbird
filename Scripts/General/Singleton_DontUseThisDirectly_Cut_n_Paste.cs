using UnityEngine;
using System.Collections;

public class Singleton_DontUseThisDirectly_Cut_n_Paste : MonoBehaviour {
	
	// http://clearcutgames.net/home/?p=437
	// Static singleton property
    public static Singleton_DontUseThisDirectly_Cut_n_Paste Instance { get; private set; }
 
    void Awake()
    {
        // First we check if there are any other instances conflicting
        if(Instance != null && Instance != this)
        {
            // If that is the case, we destroy other instances
            Destroy(gameObject);
        }
 
        // Here we save our singleton instance
        Instance = this;
 
        // Furthermore we make sure that we don't destroy between scenes (this is optional)
        DontDestroyOnLoad(gameObject);
    }
	
	// Use this for initialization
	void Start () {
	   
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
