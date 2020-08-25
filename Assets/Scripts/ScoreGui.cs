using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreGui : MonoBehaviour
{
    [SerializeField] Text scoreText = null;

    private void Start()
    {
        Score.Instance.OnScoreUpdated += ScoreUpdated;
        ScoreUpdated(Score.Instance.score);
    }

    private void ScoreUpdated(float score)
    {
        scoreText.text = score.ToString();
    }

}
