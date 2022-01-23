using Beatale.Route;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(RouteSpline))]
public class RouteSplineEditor : Editor
{
    private RouteSpline routeSpline;
    private ReorderableList reorderableList;

    private void OnSceneGUI()
    {
        Tools.current = Tool.None;
        var spline = (RouteSpline)target;
        Handles.DrawLine(new Vector3(10, 10, 10), new Vector3(20, 20, 20));
    }

    private void OnEnable()
    {
        var property = serializedObject.FindProperty("RouteVertices");
        reorderableList = new ReorderableList(serializedObject, property);
        reorderableList.drawElementCallback = (rect, index, isActive, isFocused) =>
        {
            var element = property.GetArrayElementAtIndex(index);
            rect.height -= 4;
            rect.y += 2;
            EditorGUI.PropertyField(rect, element);
        };

        AddCallback(property);
    }

    private void AddCallback(SerializedProperty property)
    {
        reorderableList.onAddCallback += (list) => {
            list.index = property.arraySize++;
            var element = property.GetArrayElementAtIndex(list.index);
            element.stringValue = "New String " + list.index;
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        reorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}