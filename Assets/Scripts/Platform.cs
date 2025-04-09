using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] GameObject trigger;
    [SerializeField] GameObject wall;
    [SerializeField] SpawnerEnemyTrigger spawnerEnemy;

    void OnEnable()
    {
        trigger.SetActive(true);
        wall.SetActive(false);
        spawnerEnemy.Reset();
    }
    void OnDisable()
    {
        trigger.SetActive(true);
        wall.SetActive(false);
        spawnerEnemy.Reset();
    }
}
