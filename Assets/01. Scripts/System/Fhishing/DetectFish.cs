using UnityEngine;

public class DetectFish : MonoBehaviour
{
    public const float EPSILON = 0.01f;
    [SerializeField] PolygonCollider2D col2d;

    private void Awake()
    {
        ContactFilter2D filter2D = new ContactFilter2D() { 
            layerMask = 1 << 5, 
            useLayerMask = true,
            useDepth = false,
            useNormalAngle = false
        };

        Collider2D[] others = new Collider2D[100];
        col2d.OverlapCollider(filter2D, others);

        foreach(Collider2D other in others)
            Debug.Log(other);
    }

    public void Detect(Vector3[] positions)
    {
        for(int i = 0 ; i < positions.Length; i++)
        {

        }
    }

    private bool IsIntersect(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        return true;
    }
}
