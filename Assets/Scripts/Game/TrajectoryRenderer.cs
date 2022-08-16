using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryRenderer : MonoBehaviour
{
    private LineRenderer lineRenderer;

    private void Update()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void DrawTrajectory(Vector3 from, Vector3 speed)
    {
        var points = new Vector3[60];

        lineRenderer.positionCount = points.Length;
        
        for (var i = 0; i < points.Length; i++)
        {
            var time = i * .1f;

            points[i] = from + speed * time + Physics.gravity*time*time/2;
        }
        
        lineRenderer.SetPositions(points);
    }

    public void StopRendering()
    {
        lineRenderer.positionCount = 0;
    }
}
