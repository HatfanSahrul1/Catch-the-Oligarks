using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] Transform shootPoint;
    [SerializeField] string projectileTag;
    [SerializeField] float minDistance = 2.0f, maxDistance = 6.0f;
    [SerializeField] float chargeTime = 3.0f;
    [SerializeField] float bulletTravelTime = 0.5f;
    [SerializeField] LayerMask targetLayer;

    private float holdTime = 0f;
    private float currentRange;
    private Vector2 targetPoint;
    private bool isCharging = false;

    ObjectPool objectPool;

    void Start()
    {
        objectPool = ObjectPool.Instance;
    }

    void Update()
    {
        GameObject target = null;

        // Hitung arah hadap berdasarkan localScale.x
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        if (Input.GetButton("Fire1"))
        {
            holdTime += Time.deltaTime;
            float t = Mathf.Clamp01(holdTime / chargeTime);
            currentRange = Mathf.Lerp(minDistance, maxDistance, t);
            isCharging = true;

            RaycastHit2D hit = Physics2D.Raycast(shootPoint.position, direction, currentRange, targetLayer);
            if (hit.collider != null)
            {
                targetPoint = hit.point;
                target = hit.transform.gameObject;
            }
            else
            {
                targetPoint = (Vector2)shootPoint.position + direction * currentRange;
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            Fire(target);
            holdTime = 0f;
            isCharging = false;
        }
    }

    void Fire(GameObject target)
    {
        if (projectileTag != null)
        {
            GameObject projectile = objectPool.SpawnFromPool(projectileTag, shootPoint.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().SetTarget(targetPoint, bulletTravelTime);
        }
    }

    void OnDrawGizmos()
    {
        if (shootPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(shootPoint.position, 0.1f);

        if (isCharging)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(shootPoint.position, targetPoint);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(targetPoint, 0.2f);
        }
    }
}
