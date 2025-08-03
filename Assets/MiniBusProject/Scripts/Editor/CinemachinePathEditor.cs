using UnityEngine;
using UnityEditor;
using Cinemachine;

[CustomEditor(typeof(CinemachinePath))]
public class CinemachinePathEditor : Editor
{
    [MenuItem("CONTEXT/CinemachinePath/Update Line Renderer")]
    private static void UpdateLineRenderer(MenuCommand command)
    {
        CinemachinePath path = (CinemachinePath)command.context;

        LineRenderer lineRenderer = path.GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = path.gameObject.AddComponent<LineRenderer>();
        }

        int waypointCount =(int) path.m_Waypoints.Length;
        Vector3[] positions = new Vector3[waypointCount];

        for (int i = 0; i < waypointCount; i++)
        {
            positions[i] = path.m_Waypoints[i].position;
        }

        lineRenderer.positionCount = waypointCount;
        lineRenderer.SetPositions(positions);
    }
}
