using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ScoreboardController : MonoBehaviour
{
    private const string METHOD_NAME = "updateScores";

    [SerializeField]
    private PhotonView photonView = null;

    private List<PlayerDataVO> playerScoresList = new List<PlayerDataVO>();

    [PunRPC]
    private void updateScores ( string playerName, int score )
    {
        PlayerDataVO playerScores = new PlayerDataVO();
        playerScores.PlayerName = playerName;
        playerScores.PlayerScore = score;
        playerScoresList.Add ( playerScores );

        onScoreUpdate ();
    }

    public void UpdatePlayerScore ( string playerName, int score )
    {
        photonView.RPC ( METHOD_NAME, RpcTarget.All, new object[] { playerName, score } );
    }

    private void onScoreUpdate ()
    {
        if ( playerScoresList.Count == PhotonNetwork.PlayerList.Length )
        {
            EventsManager.DisplayScoreboard?.Invoke (playerScoresList);
        }
    }
}
