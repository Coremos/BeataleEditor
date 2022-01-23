using Beatale.Route;
using UnityEditor;

[CustomEditor(typeof(RouteVertex))]
public class RouteVertexEditor : Editor
{
    private void OnSceneGUI()
    {
        Tools.current = Tool.None;
        var vertex = (RouteVertex)target;
    }
}
