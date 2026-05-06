using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace _project.Scripts.Tools.Editor
{
    public class ThreadConnectorTool : EditorWindow
    {
        [SerializeField] private GameObject threadPrefab;
        [SerializeField] private Transform parentForLines;

        [MenuItem("Tools/Thread Connector")]
        public static void Open()
        {
            GetWindow<ThreadConnectorTool>("Thread Connector");
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Thread Connector", EditorStyles.boldLabel);

            using (new EditorGUILayout.VerticalScope("box"))
            {
                threadPrefab = (GameObject)EditorGUILayout.ObjectField(
                    new GUIContent("Thread Prefab", "UI Image prefab with UIThreadLine"),
                    threadPrefab,
                    typeof(GameObject),
                    false
                );

                parentForLines = (Transform)EditorGUILayout.ObjectField(
                    new GUIContent("Parent", "Container for created lines"),
                    parentForLines,
                    typeof(Transform),
                    true
                );
            }

            EditorGUILayout.Space();

            using (new EditorGUI.DisabledScope(threadPrefab == null))
            {
                if (GUILayout.Button("Connect Sequentially", GUILayout.Height(30)))
                {
                    ConnectSelected();
                }
            }

            if (threadPrefab == null)
            {
                EditorGUILayout.HelpBox("Assign Thread Prefab", MessageType.Warning);
            }
        }

        private void ConnectSelected()
        {
            List<RectTransform> selected = Selection.transforms
                .Select(t => t.GetComponent<RectTransform>())
                .Where(r => r != null)
                .OrderBy(r => r.GetSiblingIndex())
                .ToList();

            if (selected.Count < 2)
            {
                Debug.LogWarning("Select at least 2 UI elements");
                return;
            }

            Undo.IncrementCurrentGroup();
            int group = Undo.GetCurrentGroup();

            for (int i = 0; i < selected.Count - 1; i++)
            {
                CreateConnection(selected[i], selected[i + 1]);
            }

            Undo.CollapseUndoOperations(group);
        }

        private void CreateConnection(RectTransform a, RectTransform b)
        {
            if (threadPrefab == null)
            {
                Debug.LogError("Thread Prefab is null");
                return;
            }

            GameObject obj = PrefabUtility.InstantiatePrefab(threadPrefab) as GameObject;

            if (obj == null)
            {
                Debug.LogError("Failed to instantiate prefab");
                return;
            }

            Transform parent = parentForLines != null ? parentForLines : a.parent;
            obj.transform.SetParent(parent, false);

            UIThreadLine line = obj.GetComponent<UIThreadLine>();

            if (line == null)
            {
                Debug.LogError("Prefab missing UIThreadLine component");
                Object.DestroyImmediate(obj);
                return;
            }

            line.SetTargets(a, b);

            Undo.RegisterCreatedObjectUndo(obj, "Create Thread");
        }
    }
}