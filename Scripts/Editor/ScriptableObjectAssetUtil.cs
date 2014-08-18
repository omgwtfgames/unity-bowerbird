/* From: https://gist.github.com/bcatcho/8638881
   This script provides a context menu item in the Project window (as well as an extra option in the Assets>Create menu) that will instantiate the selected Scriptable object and save it as an asset to a folder of your choosing.
*/
using UnityEditor;
using UnityEngine;
using System;

namespace YourCompanyName.Editor
{
    public class ScriptableObjectAssetUtil
    {
        [MenuItem("Assets/Create/Asset from ScriptableObject", true)]
        private static bool CreateScriptableObjAsAssetValidator()
        {
            var activeObject = Selection.activeObject;
         
            // make sure it is a text asset
            if ((activeObject == null) || !(activeObject is TextAsset))
            {
                return false;
            }
         
            // make sure it is a persistant asset
            var assetPath = AssetDatabase.GetAssetPath(activeObject);
            if (assetPath == null)
            {
                return false;
            }
         
            // load the asset as a monoScript
            var monoScript = (MonoScript)AssetDatabase.LoadAssetAtPath(assetPath, typeof(MonoScript));
            if (monoScript == null)
            {
                return false;
            }
         
            // get the type and make sure it is a scriptable object
            var scriptType = monoScript.GetClass();
            if (scriptType == null || !scriptType.IsSubclassOf(typeof(ScriptableObject)))
            {
                return false;
            }
         
            return true;
        }
      
        [MenuItem("Assets/Create/Asset from ScriptableObject")]
        private static void CreateScriptableObjectAssetMenuCommand(MenuCommand command)
        {
            // we already validated this path, and know these calls are safe
            var activeObject = Selection.activeObject;
            var assetPath = AssetDatabase.GetAssetPath(activeObject);
            var monoScript = (MonoScript)AssetDatabase.LoadAssetAtPath(assetPath, typeof(MonoScript));
            var scriptType = monoScript.GetClass();
         
            // ask for a path to save the asset
            var path = EditorUtility.SaveFilePanelInProject("Save asset as .asset", scriptType.Name + "_asset.asset", "asset", "Please enter a file name");
            
            // catch all exceptions when playing around with assets and files
            try
            {
                var inst = ScriptableObject.CreateInstance(scriptType);
                AssetDatabase.CreateAsset(inst, path); 
            } catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
