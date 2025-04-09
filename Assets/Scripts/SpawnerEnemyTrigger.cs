using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemyTrigger : MonoBehaviour
{
    const string EnemyTag = "Enemy";
    [SerializeField] Transform spawnPoint;
    bool isSpawned = false;

    ObjectPool objectPool;

    void Awake(){
        objectPool = ObjectPool.Instance;
    }

    public void Reset(){
        isSpawned = false;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") && !isSpawned){
            isSpawned = true;
            GameObject enemy = objectPool.SpawnFromPool(EnemyTag, spawnPoint.position, Quaternion.identity);
        }
    }
}
