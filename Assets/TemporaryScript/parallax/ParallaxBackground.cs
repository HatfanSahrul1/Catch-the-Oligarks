using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    float starPos, length;
    public GameObject cam;
    public float parallaxEffect;
    [SerializeField] bool isFollowY = false;

    void Start(){
        starPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate(){
        float distance = cam.transform.position.x * parallaxEffect;
        float movement = cam.transform.position.x * (1 - parallaxEffect);

        transform.position = new Vector3(starPos + distance, (isFollowY) ? cam.transform.position.y : transform.position.y, transform.position.z);

        if(movement > starPos + length){
            starPos += length;
        }
        else if(movement < starPos - length){
            starPos -= length;
        }
    }
}
