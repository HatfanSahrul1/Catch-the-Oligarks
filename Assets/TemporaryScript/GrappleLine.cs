using UnityEngine;

public class GrappleLine : MonoBehaviour
{
    public Transform startPoint;  // Biasanya posisi player atau grappling gun
    public Transform endPoint;    // Posisi musuh yang kena hook
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        if (endPoint != null)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, startPoint.position);
            lineRenderer.SetPosition(1, endPoint.position);
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    public void AttachLineTo(Transform enemyTransform)
    {
        endPoint = enemyTransform;
    }

    public void Detach()
    {
        endPoint = null;
    }
}
