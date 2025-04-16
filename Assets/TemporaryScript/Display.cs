using System.Collections;
using UnityEngine;
using TMPro;

public class Display : MonoBehaviour 
{
    [SerializeField] TextMeshProUGUI scoreText;

    Info info;

    void Awake(){
        info = Info.Instance;
        if (info != null){
            info.OnScoreChanged += this.UpdateScore;
        }
    }

    void Start(){
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
}