using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] GameObject trigger;
    [SerializeField] GameObject wall;

    void OnEnable()
    {
        trigger.SetActive(true);
        wall.SetActive(false);
    }
    void OnDisable()
    {
        trigger.SetActive(true);
        wall.SetActive(false);
    }
}
