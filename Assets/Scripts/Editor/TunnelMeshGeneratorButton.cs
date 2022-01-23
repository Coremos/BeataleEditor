using Beatale.TunnelSystem;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(TunnelMeshGenerator))]
public class TunnelMeshGeneratorButton : Editor
{
    public override void OnInspectorGUI()
    {
        var obj = (TunnelMeshGenerator)target;
        if (obj == null) return;

        DrawDefaultInspector();
        //if (GUI.changed)
        //{
        //    obj.CreateTunnelMesh();
        //}
        if (GUILayout.Button("Generate TunnelMesh"))
        {
            obj.GenerateTunnelMesh();
        }
    }
}
#endif