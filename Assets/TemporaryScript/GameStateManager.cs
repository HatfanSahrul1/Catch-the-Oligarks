using System.Collections;
using UnityEngine;

public class GameStateManager : MonoBehaviour 
{
    GameStateEnum gameStateEnum;

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