// BatchAddComponentsWindow.cs
// 指定Tagのオブジェクトに、複数のMonoBehaviourを一括追加するエディター拡張
// - 既に付いているコンポーネントはスキップ
// - Undo対応
// - 非アクティブ含める / 子階層も対象 などのオプション付き

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BatchAddComponentsWindow : EditorWindow
{
    [SerializeField] private string targetTag = "Untagged";

    [SerializeField] private bool includeInactive = true;
    [SerializeField] private bool includeChildren = false;

    // 追加したい「スクリプトの型」を保持（MonoScriptはEditorで扱いやすい）
    [SerializeField] private List<MonoScript> scriptsToAdd = new List<MonoScript>();

    private Vector2 scroll;

    [MenuItem("Tools/Batch Add Components")]
    public static void Open()
    {
        var w = GetWindow<BatchAddComponentsWindow>();
        w.titleContent = new GUIContent("Batch Add Components");
        w.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Target", EditorStyles.boldLabel);

        // Tag選択（UnityのTag名をPopupで出す）
        targetTag = DrawTagPopup("Tag", targetTag);

        includeInactive = EditorGUILayout.ToggleLeft("Include Inactive Objects", includeInactive);
        includeChildren = EditorGUILayout.ToggleLeft("Include Children", includeChildren);

        EditorGUILayout.Space(8);

        EditorGUILayout.LabelField("Scripts To Add (MonoBehaviour)", EditorStyles.boldLabel);

        // リスト表示
        using (var sv = new EditorGUILayout.ScrollViewScope(scroll, GUILayout.Height(200)))
        {
            scroll = sv.scrollPosition;

            int removeIndex = -1;
            for (int i = 0; i < scriptsToAdd.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                scriptsToAdd[i] = (MonoScript)EditorGUILayout.ObjectField(
                    scriptsToAdd[i], typeof(MonoScript), false);

                if (GUILayout.Button("X", GUILayout.Width(24)))
                    removeIndex = i;

                EditorGUILayout.EndHorizontal();

                // 補助表示：そのMonoScriptがMonoBehaviourかどうか
                if (scriptsToAdd[i] != null)
                {
                    var t = scriptsToAdd[i].GetClass();
                    if (t == null || !typeof(MonoBehaviour).IsAssignableFrom(t))
                    {
                        EditorGUILayout.HelpBox(
                            "このスクリプトは MonoBehaviour ではありません。追加対象として無効です。",
                            MessageType.Warning);
                    }
                }
            }

            if (removeIndex >= 0 && removeIndex < scriptsToAdd.Count)
                scriptsToAdd.RemoveAt(removeIndex);
        }

        if (GUILayout.Button("+ Add Slot"))
            scriptsToAdd.Add(null);

        EditorGUILayout.Space(12);

        using (new EditorGUI.DisabledScope(!CanApply()))
        {
            if (GUILayout.Button("Apply To Scene Objects", GUILayout.Height(32)))
            {
                Apply();
            }
        }

        if (!CanApply())
        {
            EditorGUILayout.HelpBox(
                "Tag と、追加したい MonoBehaviour スクリプトを1つ以上指定してください。",
                MessageType.Info);
        }
    }

    private bool CanApply()
    {
        if (string.IsNullOrEmpty(targetTag)) return false;

        // 有効なMonoBehaviour型が1つ以上あるか
        foreach (var ms in scriptsToAdd)
        {
            if (ms == null) continue;
            var t = ms.GetClass();
            if (t != null && typeof(MonoBehaviour).IsAssignableFrom(t))
                return true;
        }
        return false;
    }

    private void Apply()
    {
        // 対象オブジェクト収集
        var targets = FindObjectsWithTagInScene(targetTag, includeInactive);

        // 子階層も対象にする場合、子も拾う
        if (includeChildren)
        {
            var expanded = new HashSet<GameObject>();
            foreach (var go in targets)
            {
                if (go == null) continue;
                expanded.Add(go);

                foreach (Transform t in go.GetComponentsInChildren<Transform>(includeInactive))
                {
                    if (t != null) expanded.Add(t.gameObject);
                }
            }
            targets = new List<GameObject>(expanded);
        }

        // 追加する型一覧を確定
        var typesToAdd = new List<Type>();
        foreach (var ms in scriptsToAdd)
        {
            if (ms == null) continue;
            var t = ms.GetClass();
            if (t == null) continue;
            if (!typeof(MonoBehaviour).IsAssignableFrom(t)) continue;
            typesToAdd.Add(t);
        }

        if (typesToAdd.Count == 0)
        {
            Debug.LogWarning("追加できる MonoBehaviour がありません。");
            return;
        }

        int addedCount = 0;
        int skippedCount = 0;

        Undo.IncrementCurrentGroup();
        int undoGroup = Undo.GetCurrentGroup();
        Undo.SetCurrentGroupName("Batch Add Components");

        try
        {
            foreach (var go in targets)
            {
                if (go == null) continue;

                // Prefabインスタンスにも付けられるが、ケースによっては注意が必要
                // （Prefabアセット自体を書き換えるわけではない）
                foreach (var type in typesToAdd)
                {
                    if (go.GetComponent(type) != null)
                    {
                        skippedCount++;
                        continue;
                    }

                    Undo.AddComponent(go, type);
                    addedCount++;
                }
            }
        }
        finally
        {
            Undo.CollapseUndoOperations(undoGroup);
        }

        Debug.Log($"BatchAddComponents: Added={addedCount}, Skipped(already had)={skippedCount}, Targets={targets.Count}");
    }

    private static List<GameObject> FindObjectsWithTagInScene(string tag, bool includeInactiveObjects)
    {
        // FindGameObjectsWithTag は「非アクティブ」を拾えないため、全Transform走査で拾う
        var results = new List<GameObject>();

        var allTransforms = Resources.FindObjectsOfTypeAll<Transform>();
        foreach (var tr in allTransforms)
        {
            if (tr == null) continue;
            var go = tr.gameObject;

            // シーン上のオブジェクトだけ（Project内アセットやPrefabアセットを除外）
            if (EditorUtility.IsPersistent(go)) continue;

            // 非アクティブを含めないならactiveInHierarchyでフィルタ
            if (!includeInactiveObjects && !go.activeInHierarchy) continue;

            // Tag一致
            if (go.CompareTag(tag))
                results.Add(go);
        }

        return results;
    }

    private static string DrawTagPopup(string label, string currentTag)
    {
        var tags = UnityEditorInternal.InternalEditorUtility.tags;
        int index = Array.IndexOf(tags, currentTag);
        if (index < 0) index = 0;

        int newIndex = EditorGUILayout.Popup(label, index, tags);
        return tags[Mathf.Clamp(newIndex, 0, tags.Length - 1)];
    }
}
#endif
