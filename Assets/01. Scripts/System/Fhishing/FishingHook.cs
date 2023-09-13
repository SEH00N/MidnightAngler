using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FishingHook : MonoBehaviour
{
    [SerializeField] float vertexDistance = 0.05f;
    [SerializeField] float updateDelay = 0.05f;
    [SerializeField] int vertexCount = 10;

    [Space(20f)]
    [SerializeField] UnityEvent<Vector3[]> OnVertexUpdated;

    private Queue<Vector3> vertices = new Queue<Vector3>();
    private LineRenderer lineRenderer = null;

    private void Awake()
    {
        lineRenderer = transform.Find("FishingArea")?.GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
            StopTracking();
        if(Input.GetKeyDown(KeyCode.D))
            StartTracking();
    }
    
    public void StopTracking()
    {
        lineRenderer.enabled = false;

        StopAllCoroutines();
    }

    public void StartTracking()
    {
        vertices.Clear();
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, lineRenderer.transform.position);
        lineRenderer.enabled = true;

        StartCoroutine(LineUpdateRoutine());
    }

    private IEnumerator LineUpdateRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(updateDelay);

        while(true) {
            Vector3 last = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
            Vector3 now = lineRenderer.transform.position;
            now.z = 1;

            if((now - last).magnitude > vertexDistance)
            {
                vertices.Enqueue(now);
                if(vertices.Count > vertexCount)
                    vertices.Dequeue();

                Vector3[] positions = vertices.ToArray();

                lineRenderer.positionCount = vertices.Count;
                lineRenderer.SetPositions(positions);
                OnVertexUpdated?.Invoke(positions);
            }

            yield return delay;
        }
    }
}
