using UnityEngine;
using System.Collections;

public class GrapplePuller : MonoBehaviour
{
    public static GrapplePuller Instance;
    public float pullSpeed = 100f;

    void Awake(){
        Instance = this;
    }

    void Start(){
        Instance = this;
    }
    public void StartPull(Transform target, Transform destination)
    {
        GrappleLine.Instance.Attach(destination, target);
        StartCoroutine(PullToTarget(target, destination));
    }

    IEnumerator PullToTarget(Transform target, Transform destination)
    {
        while (target != null && Vector3.Distance(target.position, destination.position) > 0.1f)
        {
            target.position = Vector3.MoveTowards(target.position, destination.position, pullSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
