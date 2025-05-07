using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MissingScriptScanner : EditorWindow
{
    [MenuItem("Tools/Scan for Missing Scripts")]
    public static void ShowWindow()
    {
        GetWindow<MissingScriptScanner>("Missing Scripts Scanner").Scan();
    }

    void Scan()
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        int missingCount = 0;

        foreach (GameObject go in allObjects)
        {
            // Skippa roba interna all'editor (tipo Gizmos)
            if (EditorUtility.IsPersistent(go.transform.root.gameObject) &&
                !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
            {
                Component[] components = go.GetComponents<Component>();
                for (int i = 0; i < components.Length; i++)
                {
                    if (components[i] == null)
                    {
                        Debug.LogWarning($"🚨 Oggetto con script mancante: {GetFullPath(go)}", go);
                        missingCount++;
                    }
                }
            }
        }

        Debug.Log($"✅ Scansione completata. Script mancanti trovati: {missingCount}");
    }

    string GetFullPath(GameObject go)
    {
        string path = go.name;
        Transform current = go.transform;
        while (current.parent != null)
        {
            current = current.parent;
            path = current.name + "/" + path;
        }
        return path;
    }
}
