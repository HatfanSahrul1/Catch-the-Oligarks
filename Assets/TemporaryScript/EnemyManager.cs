using System.Collections;
using UnityEngine;

public class EnemyManager : Character, IDamageable
{
    void Update(){
        if(isFallOf()){
            TakeDamage();
        }

        if(isDead()){
            this.gameObject.SetActive(false);
        }
    }

    public void SetToDeath(){
        health = 0;
    }

    public void TakeDamage(){
        // Drone.Instance.CaughtThisEnemy(this.transform);
        this.GetComponent<Rigidbody2D>().gravityScale = 0;
        this.GetComponent<BoxCollider2D>().isTrigger = true;

        // Mulai tarik musuh ke drone
        GrapplePuller.Instance.StartPull(this.transform, Drone.Instance.transform);

        // Ubah visual tali ke: Drone ➜ Enemy
        GrappleLine.Instance.SetMode(GrappleLineMode.DroneToEnemy, this.transform);
    }
}