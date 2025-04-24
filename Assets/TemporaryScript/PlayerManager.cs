using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : Character, IDamageable
{
    public static PlayerManager Instance;
    void Start(){
        Instance = this;
    }

    void Update(){
        if(isFallOf()){
            TakeDamage();
        }
    }

    public void DeathAction(){
        SceneManager.LoadScene("SampleScene");
    }
    public void TakeDamage(){
        health = 0;
    }
}