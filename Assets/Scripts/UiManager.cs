using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField]
    private GameObject loadingScreenPrefab = null;
    private GameObject loadingScreenInstance = null;

    [SerializeField]
    private GameObject waitingScreenPrefab = null;
    private GameObject waitingScreenInstance = null;

    [SerializeField]
    private GameObject noPlayersFoundScreenPrefab = null;
    private GameObject noPlayersFoundScreenInstance = null;

    [SerializeField]
    private GameObject lobbyScreenInstance = null;

    [SerializeField]
    private Button createRoomButton = null;

    [SerializeField]
    private Button joinRoomButton = null;

    [SerializeField]
    private GameObject gameUi = null;

    [SerializeField]
    private TextMeshProUGUI timerText = null;

    [SerializeField]
    private TextMeshProUGUI nameText = null;

    [SerializeField]
    private GameObject scoreboardPrefab = null;

    [SerializeField]
    private PlayerScoreboardView playerScoresPrefab = null;

    public void Start ()
    {
        hideGameUi ();
        hideLobbyScreen ();
        showLoadingScreen ();

        EventsManager.JoinedLobby += hideLoadingScreen;
        EventsManager.JoinedLobby += showLobbyScreen;
        EventsManager.LevelSetupComplete += setNameText;
        EventsManager.JoinedRoom += hideLobbyScreen;
        EventsManager.JoinedRoom += showWaitingScreen;
        EventsManager.StartLevelSetup += hideWaitingScreen;
        EventsManager.StartLevelSetup += showGameUi;

        EventsManager.PlayersNotFound += hideWaitingScreen;
        EventsManager.PlayersNotFound += showNoPlayersFoundScreen;

        EventsManager.UpdateTimer += updateTimer;
        EventsManager.DisplayScoreboard += displayScoreboard;

        createRoomButton.onClick.AddListener ( onCreateRoomButtonTapped );
        joinRoomButton.onClick.AddListener ( onJoinRoomButtonTapped );
    }

    private void showLoadingScreen ()
    {
        loadingScreenInstance = Instantiate ( loadingScreenPrefab, transform );
    }

    private void hideLoadingScreen ()
    {
        loadingScreenInstance.SetActive ( false );
    }

    private void showLobbyScreen ()
    {
        lobbyScreenInstance.SetActive ( true );
    }

    private void hideLobbyScreen ()
    {
        lobbyScreenInstance.SetActive ( false );
    }

    private void showWaitingScreen ()
    {
        waitingScreenInstance = Instantiate ( waitingScreenPrefab, transform );
        waitingScreenInstance.SetActive ( true );
    }

    private void hideWaitingScreen ()
    {
        waitingScreenInstance.SetActive ( false );
    }

    private void showNoPlayersFoundScreen ()
    {
        noPlayersFoundScreenInstance = Instantiate ( noPlayersFoundScreenPrefab, transform );
        noPlayersFoundScreenInstance.SetActive ( true );

        StartCoroutine (hideNoPlayersFoundScreen ());
    }

    private IEnumerator hideNoPlayersFoundScreen ()
    {
        yield return new WaitForSeconds ( 3.0f );

        noPlayersFoundScreenInstance.SetActive ( false );
        showLobbyScreen ();
    }

    private void onCreateRoomButtonTapped ()
    {
        EventsManager.CreateRoomTapped?.Invoke ();
    }

    private void onJoinRoomButtonTapped ()
    {
        EventsManager.JoinRoomTapped?.Invoke ();
    }

    private void showGameUi ()
    {
        gameUi.SetActive ( true );
    }

    private void hideGameUi ()
    {
        gameUi.SetActive ( false );
    }

    private void updateTimer ( string remainingTime )
    {
        timerText.text = remainingTime;
    }

    private void setNameText ()
    {
        nameText.text = PhotonNetwork.LocalPlayer.ActorNumber.ToString();
    }

    private void displayScoreboard ( List<PlayerDataVO> playerScores )
    {
        GameObject scoreboard = Instantiate ( scoreboardPrefab, transform );
        scoreboard.transform.SetParent ( transform );
        Transform scoreboardContainer = scoreboard.transform.GetChild(0);

        for ( int i = 0 ; i < playerScores.Count ; i++ )
        {
            PlayerScoreboardView scores = Instantiate (playerScoresPrefab, scoreboardContainer);
            scores.SetPlayerData ( playerScores [i].PlayerName, playerScores [i].PlayerScore );
            scores.transform.SetAsLastSibling ();
        }
    }
}
