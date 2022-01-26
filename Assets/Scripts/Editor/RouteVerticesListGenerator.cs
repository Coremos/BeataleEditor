﻿using Beatale.Route;
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
        reorderableList.drawHeaderCallback = DrawHeader;
        reorderableList.drawElementCallback = DrawElement;
        reorderableList.onSelectCallback = OnSelect;
        reorderableList.onCanRemoveCallback = OnCanRemove;
        reorderableList.onRemoveCallback = OnRemove;
        reorderableList.onAddDropdownCallback = OnAddDropdown;
    }

    private void OnRemove(ReorderableList list)
    {
        if (EditorUtility.DisplayDialog("Warning!",
            "Are you sure you want to delete the wave?",
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
        EditorGUI.PropertyField(rect, element);
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
        routeVerticesProperty.InsertArrayElementAtIndex(routeVerticesProperty.arraySize + 1);

        reorderableList.index = routeVerticesProperty.arraySize++;
        var element = routeVerticesProperty.GetArrayElementAtIndex(reorderableList.index);
        
        GameObject routeVertexObject = new GameObject("RouteVertex" + routeVerticesProperty.arraySize);
        routeVertexObject.transform.parent = routeSpline.transform;
        RouteVertex routeVertex = routeVertexObject.AddComponent<RouteVertex>();

        routeVertex.Position = Vector3.zero;
        routeVertex.Direction1 = Vector3.right * 0.1f;
        routeVertex.Direction2 = -Vector3.right * 0.1f;

        //routeVerticesProperty.InsertArrayElementAtIndex(routeVerticesProperty.arraySize);
        //routeVerticesProperty.GetArrayElementAtIndex(routeVerticesProperty.arraySize - 1).objectReferenceValue = routeVertex;
        inspectorObject.ApplyModifiedProperties();
    }
}