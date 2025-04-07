using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : Character, IDamageable
{
    void Start(){

    }

    void Update(){
        if(isFallOf()){
            TakeDamage();
        }

        if(isDead()){
            DeathAction();
        }
    }

    public void DeathAction(){
        SceneManager.LoadScene("SampleScene");
    }
    public void TakeDamage(){
        health = 0;
    }
}