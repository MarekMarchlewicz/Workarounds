/*This is a workaround for issue with cloud build not generating 
* App Bundle files when building Android .apk
* 
* Solution from:
* https://forum.unity.com/threads/creating-android-app-bundles-aab-with-unity-cloud-builds.600583/
*
*/

#if UNITY_CLOUD_BUILD
using System.IO;
using UnityEngine;
 
namespace YourNamespace
{
    public static class AppBundleUnityCloudUtil
    {
        public static void OnPreExport()
        {
            UnityEditor.EditorUserBuildSettings.buildAppBundle = true;
            Debug.Log("AppBundleUnityCloudUtil: Enabled building of an Android App Bundle");
        }
 
        public static void OnPostExport(string exportPath)
        {
            if (!UnityEditor.EditorUserBuildSettings.buildAppBundle)
            {
                Debug.LogErrorFormat("AppBundleUnityCloudUtil: UnityEditor.EditorUserBuildSettings.buildAppBundle is not enabled");
            }
 
            var bundlePath = exportPath.Replace(".apk", ".aab");
            if (File.Exists(bundlePath))
            {
                Debug.LogFormat("AppBundleUnityCloudUtil: Moving {0} to {1}", bundlePath, exportPath);
                File.Move(bundlePath, exportPath);
            }
            else
            {
                Debug.LogErrorFormat("AppBundleUnityCloudUtil: AppBundle file not found in path {0}", bundlePath);
            }
        }
    }
}
#endif
 