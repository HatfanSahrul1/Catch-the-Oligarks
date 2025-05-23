using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    int AdditionScore = 1; // bukan best practice tapi ywdahsih
    private Vector2 targetPoint;
    private float travelTime;

    public void SetTarget(Vector2 target, float time)
    {
        targetPoint = target;
        travelTime = time;
        StartCoroutine(MoveToTarget());
    }

    public void AttachOnTargetAction(){
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
        GrappleLine.Instance.SetMode(GrappleLineMode.DroneToPlayer);
    }

    void OnTriggerEnter2D(Collider2D other){
        if(!other.gameObject.CompareTag("Player")){
            try
            {
                IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
                damageable.TakeDamage();
                Info.Instance.AddScore(AdditionScore);
                AttachOnTargetAction();
            }
            catch (System.Exception){}
        }else{
        }
    }
}
