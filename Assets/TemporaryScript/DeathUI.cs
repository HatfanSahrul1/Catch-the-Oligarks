using System.Collections;
using UnityEngine;
using TMPro;

public class DeathUI : MonoBehaviour 
{
    public static DeathUI Instance;
    [SerializeField] GameObject deathUI;
    [SerializeField] TextMeshProUGUI scoreText;

    void Awake(){
        if(Instance == null) Instance = this;
    }

    public void ShowDeathUI(bool visible){
        deathUI.SetActive(visible);
        string score = Info.Instance.score.ToString();
        scoreText.text = score;
    }
}