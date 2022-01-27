using Beatale.Route;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class RouteVerticesListGenerator
{
    private RouteSpline routeSpline;
    private ReorderableList reorderableList;
    private SerializedObject inspectorObject;
    private SerializedProperty routeVerticesProperty;

    public RouteVerticesListGenerator(SerializedObject serializedObject, SerializedProperty serializedProperty, RouteSpline parentRouteSpline)
    {
        inspectorObject = serializedObject;
        routeVerticesProperty = serializedProperty;
        routeSpline = parentRouteSpline;
    }

    public ReorderableList GenerateList()
    {
        InitializeReorderableList();
        return reorderableList;
    }

    public void InitializeReorderableList()
    {
        reorderableList = new ReorderableList(inspectorObject, routeVerticesProperty);
        reorderableList.elementHeight = 80.0f;
        reorderableList.drawHeaderCallback = DrawHeader;
        reorderableList.drawElementCallback = DrawElement;
        reorderableList.onSelectCallback = OnSelect;
        reorderableList.onCanRemoveCallback = OnCanRemove;
        reorderableList.onRemoveCallback = OnRemove;
        reorderableList.onAddDropdownCallback = OnAddDropdown;
    }

    private void OnRemove(ReorderableList list)
    {
        if (EditorUtility.DisplayDialog("Warning",
            "Delete " + list.serializedProperty.GetArrayElementAtIndex(list.index).objectReferenceValue.name + "?",
            "Yes",
            "No"))
        {
            ReorderableList.defaultBehaviours.DoRemoveButton(list);
        }
    }

    private bool OnCanRemove(ReorderableList list)
    {
        return list.count > 2;
    }

    private void OnSelect(ReorderableList list)
    {
        Debug.Log("OnSelectCallBack");
        var routeVertex = routeVerticesProperty.GetArrayElementAtIndex(list.index).objectReferenceValue;
        EditorGUIUtility.PingObject(routeVertex);
    }

    private void DrawHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, routeVerticesProperty.displayName);
    }

    private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        var element = routeVerticesProperty.GetArrayElementAtIndex(index);
        //var vertex = routeSpline.RouteVertices[index];
        var vertex = (RouteVertex)element.objectReferenceValue;
        var serializedObject = new SerializedObject(vertex);
        var vertexTypeProperty = serializedObject.FindProperty("VertexType");
        var gameObjectProperty = serializedObject.FindProperty("gameObject");
        var positionProperty = serializedObject.FindProperty("position");
        var direction1Property = serializedObject.FindProperty("direction1");
        var direction2Property = serializedObject.FindProperty("direction2");

        var vertexTypeRect = new Rect(rect.x, rect.y, 100.0f, EditorGUIUtility.singleLineHeight);
        var gameObjectRect = new Rect(rect.x + 100.0f, rect.y, rect.width - 100.0f, EditorGUIUtility.singleLineHeight);
        var positionRect = new Rect(rect.x, rect.y + 20, rect.width, EditorGUIUtility.singleLineHeight);
        var direction1Rect = new Rect(rect.x, rect.y + 40, rect.width, EditorGUIUtility.singleLineHeight);
        var direction2Rect = new Rect(rect.x, rect.y + 60, rect.width, EditorGUIUtility.singleLineHeight);

        EditorGUI.PropertyField(vertexTypeRect, vertexTypeProperty, GUIContent.none);
        EditorGUI.PropertyField(gameObjectRect, element, GUIContent.none);
        EditorGUI.PropertyField(positionRect, positionProperty, GUIContent.none);
        EditorGUI.PropertyField(direction1Rect, direction1Property, GUIContent.none);
        EditorGUI.PropertyField(direction2Rect, direction2Property, GUIContent.none);

        vertex.VertexType = (VertexType)vertexTypeProperty.enumValueIndex;
        vertex.Position = positionProperty.vector3Value;
        vertex.Direction1 = direction1Property.vector3Value;
        vertex.Direction2 = direction2Property.vector3Value;
    }

    private void OnAddDropdown(Rect buttonRect, ReorderableList list)
    {
        var menu = new GenericMenu();
        menu.AddItem(new GUIContent("Add RouteVertex"), false, AddVertex);
        menu.AddSeparator("");
        menu.AddDisabledItem(new GUIContent("Example 2"));
        menu.DropDown(buttonRect);
    }

    private void AddVertex()
    {
        reorderableList.index = routeVerticesProperty.arraySize++;
        var element = routeVerticesProperty.GetArrayElementAtIndex(reorderableList.index);
        
        GameObject routeVertexObject = new GameObject("RouteVertex" + routeVerticesProperty.arraySize);
        routeVertexObject.transform.parent = routeSpline.transform;
        RouteVertex routeVertex = routeVertexObject.AddComponent<RouteVertex>();

        routeVertex.Position = Vector3.zero;
        routeVertex.Direction1 = Vector3.right * 0.1f;
        routeVertex.Direction2 = -Vector3.right * 0.1f;

        //routeVerticesProperty.InsertArrayElementAtIndex(routeVerticesProperty.arraySize);
        routeVerticesProperty.GetArrayElementAtIndex(routeVerticesProperty.arraySize - 1).objectReferenceValue = routeVertex;

        inspectorObject.ApplyModifiedProperties();
    }
} 