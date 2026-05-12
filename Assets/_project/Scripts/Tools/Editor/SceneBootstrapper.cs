using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class SceneBootstrapper
{
    private const string MenuPath = "Tools/Force Boot Scene";
    private const string PrefKey = "ForceBootScene_Enabled";
    private const string ScenePath = "Assets/_project/Scenes/InitialScene.unity";

    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        ApplyBootScene(IsForceEnabled());
    }

    [MenuItem(MenuPath)]
    public static void ToggleAction()
    {
        bool newState = !IsForceEnabled();
        EditorPrefs.SetBool(PrefKey, newState);
        ApplyBootScene(newState);
    }

    [MenuItem(MenuPath, true)]
    public static bool ToggleActionValidate()
    {
        Menu.SetChecked(MenuPath, IsForceEnabled());
        return true;
    }

    private static void ApplyBootScene(bool enabled)
    {
        if (enabled)
        {
            SceneAsset scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath);
            if (scene != null)
            {
                EditorSceneManager.playModeStartScene = scene;
            }
        }
        else
        {
            EditorSceneManager.playModeStartScene = null;
        }
    }

    private static bool IsForceEnabled()
    {
        return EditorPrefs.GetBool(PrefKey, false);
    }
}