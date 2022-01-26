using Beatale.Route;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(RouteSpline))]
public class RouteSplineEditor : Editor
{
    private static readonly Color DIRECTION_COLOR = Color.yellow;
    private RouteSpline routeSpline;
    private ReorderableList routeVerticesList;
    private RouteVerticesListGenerator routeVerticesListGenerator;

    private SerializedProperty vertexRadiusProperty;
    private SerializedProperty vertexColorProperty;
    private SerializedProperty routeVerticesProperty;
    private SerializedProperty splineColorProperty;
    private SerializedProperty resolutionProperty;
    private SerializedProperty isLoopProperty;


    private void OnEnable()
    {
        InitializeProperties();
        InitializeReorderableList();
    }

    private void InitializeProperties()
    {
        routeVerticesProperty = serializedObject.FindProperty("RouteVertices");
        vertexRadiusProperty = serializedObject.FindProperty("VertexRadius");
        vertexColorProperty = serializedObject.FindProperty("VertexColor");
        splineColorProperty = serializedObject.FindProperty("SplineColor");
        resolutionProperty = serializedObject.FindProperty("Resolution");
        isLoopProperty = serializedObject.FindProperty("IsLoop");
    }

    private void InitializeReorderableList()
    {
        routeVerticesListGenerator = new RouteVerticesListGenerator(serializedObject, routeVerticesProperty);
        routeVerticesList = routeVerticesListGenerator.GenerateList();
    }

    private void OnSceneGUI()
    {
        //Tools.current = Tool.None;
        routeSpline = (RouteSpline)target;
        DrawVertices();
    }

    private void DrawVertices()
    {
        foreach (var vertex in routeSpline.RouteVertices)
        {
            DrawVertex(vertex);
        }
    }

    private void DrawVertex(RouteVertex vertex)
    {
        var handlePosition = DrawPositionHandle(vertex.transform);
        if (handlePosition != vertex.Position)
        {
            Undo.RecordObject(vertex.transform, "Move RouteVertex");
            vertex.Position = handlePosition;
        }

        //DrawCircleHandle(vertex.Direction1);
    }

    private Vector3 DrawPositionHandle(Transform transform)
    {
        Vector3 position;

        Handles.color = Handles.xAxisColor;
        position.x = Handles.Slider(transform.position, transform.right).x;

        Handles.color = Handles.yAxisColor;
        position.y = Handles.Slider(transform.position, transform.up).y;

        Handles.color = Handles.zAxisColor;
        position.z = Handles.Slider(transform.position, transform.forward).z;

        return position;
    }

    private Vector3 DrawCircleHandle(Vector3 position)
    {
        return Handles.Slider(position, position.normalized);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawProperties();
        routeVerticesList.DoLayoutList();
        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }
    }

    private void AddVertex()
    {
        GameObject vertex = new GameObject("RouteVertex");
    }

    private void DrawProperties()
    {
        EditorGUILayout.PropertyField(vertexRadiusProperty);
        EditorGUILayout.PropertyField(vertexColorProperty);
        EditorGUILayout.PropertyField(splineColorProperty);
        EditorGUILayout.PropertyField(resolutionProperty);
        EditorGUILayout.PropertyField(isLoopProperty);
    }

    private void DrawVertexProperty(RouteVertex vertex)
    {
        SerializedObject serializedObject = new SerializedObject(vertex);
    }
}