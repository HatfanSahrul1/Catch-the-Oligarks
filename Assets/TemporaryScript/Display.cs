using System.Collections;
using UnityEngine;
using TMPro;

public class Display : MonoBehaviour 
{
    public static Display Instance;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject displayUI;

    Info info;

    void Awake(){
        if (Instance == null) Instance = this;
        info = Info.Instance;
        if (info != null){
            info.OnScoreChanged += this.UpdateScore;
        }
    }

    void Start(){
        if(GameStateManager.Instance.GetCurrentState() == GameStateEnum.Play){
            SetVisibility(true);
        }else{
            SetVisibility(false);
        }
        if (info == null){
            info = Info.Instance;
            info.OnScoreChanged += this.UpdateScore;
        }
        else{
            info.OnScoreChanged += this.UpdateScore;
        }
    }

    public void UpdateScore(int score){
        scoreText.text = score.ToString();
    }

    public void SetVisibility(bool visible){
        displayUI.SetActive(visible);
    }
}