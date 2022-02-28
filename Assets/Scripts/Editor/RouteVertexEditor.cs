using Beatale.RouteSystem;
using UnityEditor;

[CustomEditor(typeof(RouteVertex))]
public class RouteVertexEditor : Editor
{
    private void OnEnable()
    {
        var vertex = (RouteVertex)target;
    }

    private void OnSceneGUI()
    {
        //Tools.current = Tool.None;
    }
}
