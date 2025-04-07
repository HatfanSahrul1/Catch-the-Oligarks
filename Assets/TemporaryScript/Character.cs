using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour 
{
    protected const int DEAD = 0;
    protected const float FALL_LIMIT = -6.0f;
    [SerializeField] protected int health = 10;

    public bool isDead(){
        return health <= DEAD;
    }

    public bool isFallOf(){
        return this.transform.position.y <= FALL_LIMIT;
    }
}