using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CatchFish : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Detect(GetComponent<PolygonCollider2D>().points.Select(x => (Vector3)x).ToArray());
        }
    }

    public void Detect(Vector3[] positions)
    {
        Vector3[] geometry = IsGeometryClosed(positions);
        if(geometry == null)
            return;

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
            Debug.DrawLine(lastStartPoint, lastEndPoint, Color.green, 1f);

        for(int i = 0; i < positions.Length - 3; i++)
        {
            Vector3 startPoint = positions[i];
            Vector3 endPoint = positions[i + 1];

            Debug.DrawLine(startPoint, endPoint, Color.red, 1f);

            Vector3 intersection;
            bool result = GeometryExtension.GetSegmentIntersection(startPoint, endPoint, lastStartPoint, lastEndPoint, out intersection);

            if(result) {
                ArraySegment<Vector3> geomtry = new ArraySegment<Vector3>(positions, i, positions.Length - i);
                geomtry.Array[i] = intersection;
                geomtry.Array[geomtry.Count + geomtry.Offset - 1] = intersection;

                // for(int j = geomtry.Offset; j < geomtry.Offset + geomtry.Count - 1; ++j)
                //     Debug.DrawLine(geomtry.Array[j], geomtry.Array[j + 1], Color.red, 1f);
                // Debug.Break();

                return geomtry.ToArray();
            }
        }

        return null;
    }

    private Collider2D[] DetectBoundingArea(Vector3[] geometry)
    {
        Bounds bound = geometry.GetBoundingBox();
        return Physics2D.OverlapBoxAll(bound.center, bound.size, 0);        
    }

    private List<Collider2D> DetectGeometryArea(Collider2D[] detectedColliders, Vector3[] geomtry)
    {
        List<Collider2D> detected = new List<Collider2D>();

        foreach(Collider2D col in detectedColliders)
        {
            bool result = true;

            Vector3 leftTop = new Vector3(col.bounds.min.x, col.bounds.max.y);
            Vector3 leftBottom = col.bounds.min;
            Vector3 rightTop = col.bounds.max;
            Vector3 rightBottom = new Vector3(col.bounds.max.x, col.bounds.min.y);

            result &= GeometryExtension.InsideGeometry(leftTop, geomtry);
            result &= GeometryExtension.InsideGeometry(leftBottom, geomtry);
            result &= GeometryExtension.InsideGeometry(rightTop, geomtry);
            result &= GeometryExtension.InsideGeometry(rightBottom, geomtry);
               
            if(result)
                detected.Add(col);
        }

        return detected;
    }
}
