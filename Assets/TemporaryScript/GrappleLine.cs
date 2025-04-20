using UnityEngine;
using System.Collections;

public class GrappleLine : MonoBehaviour
{
    private LineRenderer lineRenderer;

    public static GrappleLine Instance;

    void Awake()
    {
        Instance = this;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }

    public void Attach(Transform start, Transform end)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, start.position);
        lineRenderer.SetPosition(1, end.position);

        StartCoroutine(UpdateLine(start, end));
    }

    IEnumerator UpdateLine(Transform start, Transform end)
    {
        while (end != null && lineRenderer.enabled)
        {
            lineRenderer.SetPosition(0, start.position);
            lineRenderer.SetPosition(1, end.position);
            yield return null;
        }
    }

    public void Detach()
    {
        lineRenderer.enabled = false;
    }
}
