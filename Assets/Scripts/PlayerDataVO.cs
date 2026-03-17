using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataVO
{
    private string playerName = string.Empty;
    public string PlayerName
    {
        get
        {
            return playerName;
        }

        set
        {
            playerName = value;
        }
    }

    private int playerScore = 0;
    public int PlayerScore
    {
        get
        {
            return playerScore;
        }

        set
        {
            playerScore = value;
        }
    }
}
