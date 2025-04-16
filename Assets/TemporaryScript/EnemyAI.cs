using System.Collections;
using UnityEngine;


public class EnemyAI : MonoBehaviour 
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")){
            other.GetComponent<IDamageable>().TakeDamage();
        }
    }
}