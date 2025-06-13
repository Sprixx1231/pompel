using UnityEngine;
using UnityEditor;

public class PlayerPrefsClearer : MonoBehaviour
{
    [MenuItem("Tools/Clear PlayerPrefs")]
    public static void ClearAllPlayerPrefs()
    {
        if (EditorUtility.DisplayDialog(
                "Clear PlayerPrefs",
                "Are you sure you want to delete all PlayerPrefs?",
                "Yes", "No"))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("PlayerPrefs cleared.");
        }
    }
}
