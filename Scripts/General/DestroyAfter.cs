using UnityEngine;
using System.Collections;

public class DestroyAfter : MonoBehaviour
{

    public enum DisableMethod
    {
        Inactivation,
        Destruction 
    };

    public float time;
    public DisableMethod disableBy;
    public bool detachChildren = false;

    // NOTE: similar functionality is built into Unity - 
    //       just tag any GameObject as "EditorOnly" and it
    //       won't be included in published builds but will
    //       be available in the Editor.
    public bool keepInEditor = false;

    void Awake()
    {
        if (!Application.isEditor || 
            (Application.isEditor && !keepInEditor)) {
                Invoke("DestroyOrDisable", time);
        }
    }
  
    void DestroyOrDisable()
    {
        if (detachChildren) transform.DetachChildren();

        switch (disableBy)
        {
            case DisableMethod.Inactivation:
                gameObject.SetActive(false);
                break;
            case DisableMethod.Destruction:
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }
}
