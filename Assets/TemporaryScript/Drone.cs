using UnityEngine;

public class Drone : MonoBehaviour 
{
    public static Drone Instance;

    void Awake(){
        Instance = this;
    }

    void Start(){
        Instance = this;
    }
    public void CaughtThisEnemy(Transform enemy){
        GrapplePuller.Instance.StartPull(enemy, this.transform);
    }
}