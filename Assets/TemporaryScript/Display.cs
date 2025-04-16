using System.Collections;
using UnityEngine;
using TMPro;

public class Display : MonoBehaviour 
{
    [SerializeField] TextMeshProUGUI scoreText;

    Info info;

    void Awake(){
        info = Info.Instance;
    }
    void Start(){
        if (info == null){
            info.OnScoreChanged += this.UpdateScore;
        }
    }

    public void UpdateScore(int score){
        scoreText.text = score.ToString();
    }
}