using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Score : MonoBehaviour
{
    public static Score Instance { get; private set; }

    public UnityAction<float> OnScoreUpdated { get; set; }
    public uint score { get { return _score; } }

    private uint _score = 0;

    private void Awake()
    {
        Score.Instance = this;
    }

    public void Add(uint amount)
    {
        _score += amount;
        OnScoreUpdated?.Invoke(score);
    }

}
