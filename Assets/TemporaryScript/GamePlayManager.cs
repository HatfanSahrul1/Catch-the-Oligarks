using System.Collections;
using UnityEngine;

public class GamePlayManager : MonoBehaviour 
{
    public static GamePlayManager Instance { get; private set; }

    void Awake(){
        if(Instance == null) Instance = this;
    }
}