using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    int AdditionScore = 10; // bukan best practice tapi ywdahsih
    private Vector2 targetPoint;
    private float travelTime;

    public void SetTarget(Vector2 target, float time)
    {
        targetPoint = target;
        travelTime = time;
        StartCoroutine(MoveToTarget());
    }

    public void AttachOnTargetAction(){
        Info.Instance.AddScore(AdditionScore);
        gameObject.SetActive(false);
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
        AttachOnTargetAction();
    }

    void OnTriggerEnter2D(Collider2D other){
        if(!other.gameObject.CompareTag("Player")){
            try
            {
                IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
                damageable.TakeDamage();
                AttachOnTargetAction();
            }
            catch (System.Exception){}
        }
    }
}
