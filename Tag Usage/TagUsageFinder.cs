using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class TagUsageFinder : EditorWindow
{
    private string selectedTag = string.Empty;
    private List<string> tagLocations = new List<string>();
    private bool tagScanCompleted = false;
    private bool includeProjectAssets = false;
    private bool searchInScripts = false;
    private Vector2 scrollPosition;
    private string[] availableTags;

    [MenuItem("Tools/Tag Usage Finder")]
    public static void ShowWindow()
    {
        GetWindow<TagUsageFinder>("Tag Usage Finder");
    }

    private void OnEnable()
    {
        availableTags = UnityEditorInternal.InternalEditorUtility.tags;
    }

    private void OnGUI()
    {
        GUILayout.Label("Select the tag to find its usage:", EditorStyles.boldLabel);

        // Use EditorGUILayout.Popup to get the selected index
        int selectedIndex = EditorGUILayout.Popup("Tag Name:", Array.IndexOf(availableTags, selectedTag), availableTags);

        // Update selectedTag using the selected index
        if (selectedIndex >= 0 && selectedIndex < availableTags.Length)
        {
            selectedTag = availableTags[selectedIndex];
        }

        includeProjectAssets = EditorGUILayout.Toggle("Include Project Assets:", includeProjectAssets);
        searchInScripts = EditorGUILayout.Toggle("Search in Scripts:", searchInScripts);

        if (GUILayout.Button("Find Tag Usage"))
        {
            FindTagUsage(selectedTag);
        }

        if (tagScanCompleted)
        {
            GUILayout.Label("Locations using tag '" + selectedTag + "':");
            if (tagLocations.Count == 0)
            {
                GUILayout.Label("No usage found.");
            }
            else
            {
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(200));
                foreach (var location in tagLocations)
                {
                    GUILayout.Label(location);
                }
                EditorGUILayout.EndScrollView();
            }
        }
    }

    private void FindTagUsage(string tag)
    {
        tagLocations.Clear();
        tagScanCompleted = false;

        if (string.IsNullOrWhiteSpace(tag))
        {
            Debug.LogWarning("Tag name is empty or whitespace.");
            return;
        }

        // Scan all scenes
        foreach (var scenePath in EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path))
        {
            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            foreach (var go in scene.GetRootGameObjects())
            {
                CheckTagUsage(go, tag, "Scene: " + scenePath);
            }
        }

        // Include project assets if checkbox is checked
        if (includeProjectAssets)
        {
            var allPrefabs = AssetDatabase.FindAssets("t:Prefab")
                .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                .Select(path => AssetDatabase.LoadAssetAtPath<GameObject>(path));

            foreach (var prefab in allPrefabs)
            {
                CheckTagUsage(prefab, tag, "Prefab: " + AssetDatabase.GetAssetPath(prefab));
            }
        }

        // Search within scripts if checkbox is checked
        if (searchInScripts)
        {
            var scriptFiles = Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories);
            var tagRegex = new Regex(@"\btag\s*=\s*\""([^\""]+)\"""); // Improved regex for capturing dynamic tags
            foreach (var file in scriptFiles)
            {
                string content = File.ReadAllText(file);
                if (tagRegex.IsMatch(content) && tagRegex.Match(content).Groups[1].Value == tag)
                {
                    tagLocations.Add("Script: " + Path.GetFileName(file));
                }
            }
        }

        tagScanCompleted = true;
    }

    private void CheckTagUsage(GameObject gameObject, string tag, string locationIdentifier)
    {
        if (gameObject.tag == tag)
        {
            tagLocations.Add(locationIdentifier + " -> " + GetGameObjectPath(gameObject));
        }

        foreach (Transform child in gameObject.transform)
        {
            CheckTagUsage(child.gameObject, tag, locationIdentifier);
        }
    }

    private string GetGameObjectPath(GameObject gameObject)
    {
        string path = gameObject.name;
        while (gameObject.transform.parent != null)
        {
            gameObject = gameObject.transform.parent.gameObject;
            path = gameObject.name + "/" + path;
        }
        return path;
    }
}
