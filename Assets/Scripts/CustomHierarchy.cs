using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
 
[InitializeOnLoad]
public class CustomHierarchy
{
    static CustomHierarchy()
    {
        // EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUIPrefab;
        EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
    }

    private static void HandleHierarchyWindowItemOnGUI(int instanceid, Rect selectionrect)
    {
        var obj = EditorUtility.InstanceIDToObject(instanceid);
        if (obj != null)
        {
            var go = obj.GameObject();
            var hierarchyTag = go.GetComponent<HierarchyTag>();
            if (hierarchyTag != null)
            {
                Color fontColor = hierarchyTag.Text;
                Color backgroundColor = hierarchyTag.Background;
                if (Selection.instanceIDs.Contains(instanceid))
                {
                    fontColor = hierarchyTag.TextSelected;
                    backgroundColor = hierarchyTag.BackgroundSelected;
                }

                Rect offsetRect = new Rect(selectionrect.position, selectionrect.size);
                EditorGUI.DrawRect(selectionrect, backgroundColor);
                EditorGUI.LabelField(offsetRect, $"[ {obj.name} ]", new GUIStyle()
                    {
                        normal = new GUIStyleState() {textColor = fontColor},
                        fontStyle = FontStyle.Bold,
                        alignment = hierarchyTag.TextAnchor,
                        wordWrap = false,
                        richText = true
                    }
                );
            }
        }
    }

    private static void HandleHierarchyWindowItemOnGUIPrefab(int instanceID, Rect selectionRect)
    {
        Color fontColor = Color.yellow;
        Color backgroundColor = Color.black;
 
        var obj = EditorUtility.InstanceIDToObject(instanceID);
        if (obj != null)
        {
            var prefabType = PrefabUtility.GetPrefabType(obj);
            if (prefabType == PrefabType.PrefabInstance)
            {
                if (Selection.instanceIDs.Contains(instanceID))
                {
                    fontColor = Color.white;
                    backgroundColor = new Color(0.24f, 0.48f, 0.90f);
                }
 
                Rect offsetRect = new Rect(selectionRect.position, selectionRect.size);
                EditorGUI.DrawRect(selectionRect, backgroundColor);
                EditorGUI.LabelField(offsetRect, obj.name, new GUIStyle()
                    {
                        normal = new GUIStyleState() { textColor = fontColor },
                        fontStyle = FontStyle.Bold,
                        alignment = TextAnchor.MiddleLeft
                    }
                );
            }
        }
    }
    
}