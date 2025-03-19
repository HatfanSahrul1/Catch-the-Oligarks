using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] GameObject wall;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            wall.SetActive(true);
            gameObject.SetActive(false);
            Debug.Log("Wall activated");
        }
    }
}
