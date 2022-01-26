using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class RouteVerticesListGenerator
{
    private ReorderableList reorderableList;
    private SerializedObject inspectorObject;
    private SerializedProperty routeVerticesProperty;

    public RouteVerticesListGenerator(SerializedObject serializedObject, SerializedProperty serializedProperty)
    {
        inspectorObject = serializedObject;
        routeVerticesProperty = serializedProperty;
    }

    public ReorderableList GenerateList()
    {
        InitializeReorderableList();
        return reorderableList;
    }

    public void InitializeReorderableList()
    {
        reorderableList = new ReorderableList(inspectorObject, routeVerticesProperty);
        reorderableList.drawHeaderCallback = DrawHeader;
        reorderableList.drawElementCallback = DrawElement;
        reorderableList.onAddCallback += OnAdd;

        //reorderableList.drawElementBackgroundCallback = (rect, index, isActive, isFocused) =>
        //{
        //    if (Event.current.type == EventType.Repaint)
        //    {
        //        EditorStyles.miniButton.Draw(rect, false, isActive, isFocused, false);
        //    }
        //};
    }

    private void DrawHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, routeVerticesProperty.displayName);
    }

    private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        var element = routeVerticesProperty.GetArrayElementAtIndex(index);
        EditorGUI.PropertyField(rect, element);
    }

    private void OnAdd(ReorderableList list)
    {
        list.index = routeVerticesProperty.arraySize++;
        var element = routeVerticesProperty.GetArrayElementAtIndex(list.index);
        element.stringValue = "New String " + list.index;
    }
}
