using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayManager : MonoBehaviour 
{
    public static GamePlayManager Instance { get; private set; }
    GameStateEnum gameStateEnum;

    [SerializeField] GameObject gate;

    void Awake(){
        if(Instance == null) Instance = this;
    }

    void Start(){
        Time.timeScale = 1;
    }

    void Update(){
        gameStateEnum = GameStateManager.Instance.GetCurrentState();

        switch(gameStateEnum){
            case GameStateEnum.Home:    Home();    break;
            case GameStateEnum.Play:    Play();    break;
            case GameStateEnum.Dead:    Dead();    break;
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
            GrappleLine.Instance.SetMode(GrappleLineMode.DroneToPlayer);
        }
    }

    void Play(){
        if(PlayerManager.Instance.isDead()){
            GameStateManager.Instance.SwitchState(GameStateEnum.Dead);
            DeathUI.Instance.ShowDeathUI(true);
            Time.timeScale = 0;
        }
    }

    void Dead(){
        if(Input.anyKeyDown){
            SceneManager.LoadScene("SampleScene");
        }
    }
}