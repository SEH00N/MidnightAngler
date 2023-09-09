using System.Runtime.InteropServices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingHook : MonoBehaviour
{
    [SerializeField] float vertexDistance = 0.05f;
    [SerializeField] float updateDelay = 0.05f;
    [SerializeField] int vertexCount = 10;

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

            if((now - last).magnitude > vertexDistance)
            {
                vertices.Enqueue(now);
                if(vertices.Count > vertexCount)
                    vertices.Dequeue();

                lineRenderer.positionCount = vertices.Count;
                lineRenderer.SetPositions(vertices.ToArray());
            }

            yield return delay;
        }
    }
}
