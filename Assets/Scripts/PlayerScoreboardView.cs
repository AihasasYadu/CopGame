using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScoreboardView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerNameText = null;

    [SerializeField]
    private TextMeshProUGUI playerScoreText = null;

    public void SetPlayerData ( string name, int score )
    {
        playerNameText.text = name;
        playerScoreText.text = score.ToString();
    }
}
