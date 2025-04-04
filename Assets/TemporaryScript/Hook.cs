using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 targetPoint;
    private float travelTime;

    public void SetTarget(Vector2 target, float time)
    {
        targetPoint = target;
        travelTime = time;
        StartCoroutine(MoveToTarget());
    }

    private IEnumerator MoveToTarget()
    {
        Vector2 startPos = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < travelTime)
        {
            transform.position = Vector2.Lerp(startPos, targetPoint, elapsedTime / travelTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPoint;
        // Destroy(gameObject, 0.5f); // Hancurkan proyektil setelah mencapai target
    }
}
