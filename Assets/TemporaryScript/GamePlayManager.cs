using System.Collections;
using UnityEngine;

public class GamePlayManager : MonoBehaviour 
{
    public static GamePlayManager Instance { get; private set; }
    GameStateEnum gameStateEnum;

    [SerializeField] GameObject gate;

    void Awake(){
        if(Instance == null) Instance = this;
    }

    void Update(){
        gameStateEnum = GameStateManager.Instance.GetCurrentState();

        switch(gameStateEnum){
            case GameStateEnum.Home: Home(); break;
        }
    }

    void Home(){
        if(Input.anyKeyDown){
            gate.SetActive(false);
            CameraFollow.Instance.PressPlay();
        }

        //switch ke play
        if(Movement.Instance.IsGround()){
            GameStateManager.Instance.SwitchState(GameStateEnum.Play);
            Display.Instance.SetVisibility(true);
        }
    }
}