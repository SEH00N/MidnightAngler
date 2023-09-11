using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CatchFish : MonoBehaviour
{
    [SerializeField] PolygonCollider2D col2d;

    private void Awake()
    {
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
            Detect(col2d.points.Select(x => (Vector3)x).ToArray());
    }

    public void Detect(Vector3[] positions)
    {
        Collider2D[] boundedCols = DetectBoundingArea(positions);
        List<Collider2D> detectedCols = DetectGeometryArea(boundedCols, positions);

        foreach(Collider2D col in detectedCols)
            // if(col.TryGetComponent<FishBehaviour>(out FishBehaviour fish))
                Debug.Log(col.name);
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
