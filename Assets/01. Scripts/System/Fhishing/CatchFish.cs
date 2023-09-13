using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CatchFish : MonoBehaviour
{
    [SerializeField] PolygonCollider2D col2d;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
            Detect(col2d.points.Select(x => (Vector3)x).ToArray());
    }

    public void Detect(Vector3[] positions)
    {
        Vector3[] geometry = IsGeometryClosed(positions);
        if(geometry == null)
            return;

        Debug.Log("Geometry Closed");
        Collider2D[] boundedCols = DetectBoundingArea(geometry);
        List<Collider2D> detectedCols = DetectGeometryArea(boundedCols, geometry);

        foreach(Collider2D col in detectedCols)
            // if(col.TryGetComponent<FishBehaviour>(out FishBehaviour fish))
                Debug.Log(col.name);
    }

    private Vector3[] IsGeometryClosed(Vector3[] positions)
    {
        if(positions.Length < 4)
            return null;

        Vector3 lastStartPoint = positions[positions.Length - 2];
        Vector3 lastEndPoint = positions[positions.Length - 1];

        for(int i = 0; i < positions.Length - 3; i++)
        {
            Vector3 startPoint = positions[i];
            Vector3 endPoint = positions[i + 1];

            Vector3 intersection;
            bool result = GeometryExtension.GetSegmentIntersection(startPoint, endPoint, lastStartPoint, lastEndPoint, out intersection);

            if(result) {
                ArraySegment<Vector3> geomtry = new ArraySegment<Vector3>(positions, i, positions.Length - i);
                geomtry.Array[i] = intersection;
                geomtry.Array[geomtry.Array.Length - 1] = intersection;

                Debug.Log($"{startPoint}, {endPoint} : {lastStartPoint}, {lastEndPoint}");

                return geomtry.Array;
            }
        }

        return null;
    }

    private Collider2D[] DetectBoundingArea(Vector3[] positions)
    {
        Vector2[] planePositions = positions.Select(x => (Vector2)x).ToArray();
        col2d.points = planePositions;

        Bounds bound = col2d.bounds;
        return Physics2D.OverlapBoxAll(bound.center, bound.size, 0);        
    }

    private List<Collider2D> DetectGeometryArea(Collider2D[] detectedColliders, Vector3[] positions)
    {
        List<Collider2D> detected = new List<Collider2D>();

        foreach(Collider2D col in detectedColliders)
        {
            bool result = true;

            Vector3 leftTop = new Vector3(col.bounds.min.x, col.bounds.max.y);
            Vector3 rightBottom = new Vector3(col.bounds.max.x, col.bounds.min.y);
            result &= GeometryExtension.InsideGeometry(col.bounds.min, positions);
            result &= GeometryExtension.InsideGeometry(col.bounds.max, positions);
            result &= GeometryExtension.InsideGeometry(leftTop, positions);
            result &= GeometryExtension.InsideGeometry(rightBottom, positions);
               
            if(result)
                detected.Add(col);
        }

        return detected;
    }
}
