using Beatale.RouteSystem;
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
        routeSpline = (RouteSpline)target;
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
        routeVerticesListGenerator = new RouteVerticesListGenerator(serializedObject, routeVerticesProperty, routeSpline);
        routeVerticesList = routeVerticesListGenerator.GenerateList();
    }

    private void OnSceneGUI()
    {
        //Tools.current = Tool.None;
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

        if (vertex.VertexType != VertexType.None)
        {
            handlePosition = DrawDirectionHandle(vertex.GlobalDirection1, Quaternion.FromToRotation(Vector3.up, vertex.Direction1));
            if (handlePosition != vertex.GlobalDirection1)
            {
                Undo.RecordObject(vertex, "Move RouteVertex Direction");
                vertex.GlobalDirection1 = handlePosition;
                if (vertex.VertexType == VertexType.Connected) vertex.Direction2 = -vertex.Direction1;
            }

            handlePosition = DrawDirectionHandle(vertex.GlobalDirection2, Quaternion.FromToRotation(Vector3.up, vertex.Direction2));
            if (handlePosition != vertex.GlobalDirection2)
            {
                Undo.RecordObject(vertex, "Move RouteVertex Direction");
                vertex.GlobalDirection2 = handlePosition;
                if (vertex.VertexType == VertexType.Connected) vertex.Direction1 = -vertex.Direction2;
            }
        }
        DrawUpHandle(vertex);

        Handles.color = DIRECTION_COLOR;
        Handles.DrawLine(vertex.Position, vertex.GlobalDirection1);
        Handles.DrawLine(vertex.Position, vertex.GlobalDirection2);
    }

    private Vector3 DrawUpHandle(RouteVertex vertex)
    {
        Handles.color = Color.blue;
        return Handles.FreeMoveHandle(vertex.Position + vertex.Up * 5f, Quaternion.FromToRotation(Vector3.up, vertex.Up), .1f, Vector3.zero, Handles.CircleHandleCap);
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

    private Vector3 DrawDirectionHandle(Vector3 position, Quaternion quaternion)
    {
        Handles.color = DIRECTION_COLOR;
        //return Handles.PositionHandle(position, position.normalized);
        return Handles.FreeMoveHandle(position, quaternion, 1, Vector3.zero, Handles.CircleHandleCap);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawProperties();
        routeVerticesList.DoLayoutList();
        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
            //EditorUtility.SetDirty(target);
        }
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