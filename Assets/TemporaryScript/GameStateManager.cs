using System.Collections;
using UnityEngine;

public class GameStateManager : MonoBehaviour 
{
    public static GameStateManager Instance { get; private set;}
    GameStateEnum gameStateEnum;

    void Awake(){
        if(Instance == null) Instance = this;
    }

    void Start(){
        gameStateEnum = GameStateEnum.Home;
    }

    public void SwicthState(GameStateEnum newState){
        gameStateEnum = newState;
    }

    public GameStateEnum GetCurrentState(){
        return gameStateEnum;
    }
}