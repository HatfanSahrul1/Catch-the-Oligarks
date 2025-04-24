using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrappleLine : MonoBehaviour
{
    public static GrappleLine Instance;

    private LineRenderer lineRenderer;

    [SerializeField] private Transform player;
    [SerializeField] private Transform drone;
    private Transform hookOrEnemy;

    private GrappleLineMode mode;

    void Awake()
    {
        Instance = this;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (!lineRenderer.enabled) return;

        switch (mode)
        {
            case GrappleLineMode.DroneToPlayer:
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, drone.position);
                lineRenderer.SetPosition(1, player.position);
                break;

            case GrappleLineMode.DroneToPlayerToHook:
                if (hookOrEnemy != null)
                {
                    lineRenderer.positionCount = 3;
                    lineRenderer.SetPosition(0, drone.position);
                    lineRenderer.SetPosition(1, player.position);
                    lineRenderer.SetPosition(2, hookOrEnemy.position);
                }
                break;

            case GrappleLineMode.DroneToEnemy:
                if (hookOrEnemy != null)
                {
                    lineRenderer.positionCount = 2;
                    lineRenderer.SetPosition(0, drone.position);
                    lineRenderer.SetPosition(1, hookOrEnemy.position);
                }
                break;
        }
    }

    public void SetMode(GrappleLineMode newMode, Transform hookOrEnemyTarget = null)
    {
        mode = newMode;
        hookOrEnemy = hookOrEnemyTarget;
        lineRenderer.enabled = true;
    }

    public void Detach()
    {
        lineRenderer.enabled = false;
    }
}
