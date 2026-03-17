using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText = null;

    public void Start ()
    {
        scoreText.text = "0";
        EventsManager.UpdateScore += updateScore;
    }

    private void updateScore (int score)
    {
        scoreText.text = score.ToString();
    }
}
