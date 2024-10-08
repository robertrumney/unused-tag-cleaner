using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class UnusedTagCleaner : EditorWindow
{
    private List<string> unusedTags = new List<string>();
    private bool scanCompleted = false;

    [MenuItem("Tools/Unused Tags Remover")]
    public static void ShowWindow()
    {
        GetWindow<UnusedTagCleaner>("Unused Tags Remover");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Scan for Unused Tags"))
        {
            ScanForUnusedTags();
        }

        if (scanCompleted)
        {
            GUILayout.Label("Unused Tags:");
            foreach (var tag in unusedTags)
            {
                GUILayout.Label(tag);
            }

            if (GUILayout.Button("Remove All Unused Tags"))
            {
                RemoveUnusedTags();
            }
        }
    }

    private void ScanForUnusedTags()
    {
        var allTags = new List<string>(UnityEditorInternal.InternalEditorUtility.tags);
        var usedTags = new HashSet<string>();

        foreach (var scenePath in EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path))
        {
            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            foreach (var go in scene.GetRootGameObjects())
            {
                GetUsedTags(go, usedTags);
            }
        }

        unusedTags = allTags.Except(usedTags).ToList();
        scanCompleted = true;
    }

    private void GetUsedTags(GameObject gameObject, HashSet<string> usedTags)
    {
        usedTags.Add(gameObject.tag);
        foreach (Transform child in gameObject.transform)
        {
            GetUsedTags(child.gameObject, usedTags);
        }
    }

    private void RemoveUnusedTags()
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");

        for (int i = tagsProp.arraySize - 1; i >= 0; i--)
        {
            SerializedProperty tagProp = tagsProp.GetArrayElementAtIndex(i);
            if (unusedTags.Contains(tagProp.stringValue))
            {
                tagsProp.DeleteArrayElementAtIndex(i);
            }
        }

        tagManager.ApplyModifiedProperties();
        AssetDatabase.SaveAssets();

        Debug.Log("Unused tags removed.");
        unusedTags.Clear();
        scanCompleted = false;
    }
}
