using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour 
{
    protected const int DEAD = 0;
    protected const float FALL_LIMIT = -6.0f;
    protected const float GRAVITY_SCALE = 3.0f;
    [SerializeField] protected int health = 10;

    public bool isDead(){
        return health <= DEAD;
    }

    public bool isFallOf(){
        return this.transform.position.y <= FALL_LIMIT;
    }

    public void Reset(){
        this.health = 10;
        try{
            GetComponent<Rigidbody2D>().gravityScale = GRAVITY_SCALE;
            GetComponent<BoxCollider2D>().isTrigger = false;
        }catch{}
    }
}