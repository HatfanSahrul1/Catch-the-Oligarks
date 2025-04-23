using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour 
{
    GameObject player;
    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update(){
        LookAtPlayer();
    }

    void LookAtPlayer(){
        Vector3 scale = transform.localScale;
        if (transform.position.x > player.transform.position.x){
            scale.x = -Mathf.Abs(scale.x);
        }
        else{
            scale.x = Mathf.Abs(scale.x);
        }
        transform.localScale = scale;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")){
            other.GetComponent<IDamageable>().TakeDamage();
        }else if(other.CompareTag("Drone")){
            this.GetComponent<EnemyManager>().SetToDeath();
            GrappleLine.Instance.Detach();
        }
    }
}