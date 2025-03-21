using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] Transform shootPoint;
    [SerializeField] float minDistance = 2.0f, maxDistance = 6.0f;
    [SerializeField] float chargeTime = 3.0f;
    [SerializeField] LayerMask targetLayer;

    private float holdTime = 0f;
    private float currentRange;
    private Vector2 targetPoint;
    private bool isCharging = false; // Menandakan apakah sedang mengisi daya tembakan

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            holdTime += Time.deltaTime;
            float t = Mathf.Clamp01(holdTime / chargeTime);
            currentRange = Mathf.Lerp(minDistance, maxDistance, t);
            isCharging = true;

            RaycastHit2D hit = Physics2D.Raycast(shootPoint.position, transform.right, currentRange, targetLayer);
            if (hit.collider != null)
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = (Vector2)shootPoint.position + (Vector2)transform.right * currentRange;
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            Fire();
            holdTime = 0f;
            isCharging = false;
        }
    }

    void Fire()
    {
        Debug.Log("Fired at: " + targetPoint);
        Instantiate(this.gameObject, targetPoint, Quaternion.identity);
    }

    // Menampilkan Gizmos untuk mengecek arah dan jarak tembakan
    void OnDrawGizmos()
    {
        if (shootPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(shootPoint.position, 0.1f); // Menandai titik tembak

        if (isCharging) // Jika sedang menahan tombol Fire1
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(shootPoint.position, targetPoint); // Menunjukkan lintasan raycast
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(targetPoint, 0.2f); // Menandai titik akhir raycast
        }
    }
}
