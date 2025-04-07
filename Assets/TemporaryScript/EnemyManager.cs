using System.Collections;
using UnityEngine;

public class EnemyManager : Character, IDamageable
{
    void Update(){
        if(isDead()){
            this.gameObject.SetActive(false);
        }
    }

    public void TakeDamage(){
        health = 0;
    }
}