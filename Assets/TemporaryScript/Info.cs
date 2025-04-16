using System.Collections;
using UnityEngine;

public class Info : MonoBehaviour
{
    public event System.Action<int> OnScoreChanged;
    int _score;
    public int score{
        get => _score;
        set
        {
            if (_score != value)
            {
                _score = value;
                OnScoreChanged?.Invoke(_score);
            }
        }
    }
    public static Info Instance;

    void Awake(){
        Instance = this;
    }

    void Start(){
        score = 0;
    }

    public void AddScore(int score){
        this.score += score;
    }
}