using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const float CAMERA_SIZE_PERCENTAGE = 0.4f;

    [SerializeField]
    private LevelConfigData levelData = null;

    [SerializeField]
    private GridManager gridManager = null;

    [SerializeField]
    private GameObject collectiblePrefab = null;
    private CollectibleController collectibleInstance = null;

    [SerializeField]
    private GameObject playerPrefab = null;
    private PlayerController playerInstance = null;

    [SerializeField]
    private ScoreboardController scoreboard = null;

    private bool gameOver = false;

    private int score = 0;
    private int timerSeconds = 0;

    public void Start ()
    {
        EventsManager.StartLevelSetup += setupGame;
        EventsManager.DonutCollected += onDonutCollected;
        EventsManager.GameOver += removeCollectible;
        EventsManager.GameOver += removePlayer;
    }

    private void setupGame ()
    {
        gridManager.GenerateGrid ();
        spawnPlayer ();
        initiateCollectible ();
        setCamera ();
        timerSeconds = levelData.TimerSeconds;
        StartCoroutine (timer ());
        EventsManager.LevelSetupComplete?.Invoke ();
    }

    private void onDonutCollected (int value)
    {
        score += value;
        EventsManager.UpdateScore?.Invoke(score);
    }

    private void spawnPlayer ()
    {
        if ( playerPrefab != null )
        {
            int rowIndex = UnityEngine.Random.Range ( 0, gridManager.RowMax );
            int colIndex = UnityEngine.Random.Range ( 0, gridManager.ColumnMax );
            Transform cellPos = gridManager.GetCellAtIndex ( rowIndex, colIndex );
            GameObject player = PhotonNetwork.Instantiate ( playerPrefab.name, cellPos.position, Quaternion.identity );
            playerInstance = player.GetComponent<PlayerController>();
        }
    }

    private void setCamera ()
    {
        Camera.main.orthographicSize = CAMERA_SIZE_PERCENTAGE * 
                            ( gridManager.RowMax > gridManager.ColumnMax ? gridManager.RowMax : gridManager.ColumnMax );
        
        int midRow = gridManager.RowMax / 2;
        int midColumn = gridManager.ColumnMax / 2;
        Transform lookAtTransform = gridManager.GetCellAtIndex ( midRow, midColumn );
        Camera.main.transform.LookAt ( lookAtTransform, Vector3.up );
    }

    private void initiateCollectible ()
    {
        if ( PhotonNetwork.IsMasterClient )
        {
            GameObject collectible = PhotonNetwork.Instantiate ( collectiblePrefab.name, Vector3.zero, Quaternion.identity );
            collectibleInstance = collectible.GetComponent<CollectibleController>();
            collectibleInstance.SetupCollectible (levelData.CollectibleValue);
            collectibleInstance.HideCollectible ();
    
            StartCoroutine (beginCollectibleCycle());
        }
    }

    private IEnumerator beginCollectibleCycle ()
    {
        yield return new WaitForSeconds ( 1.5f );

        while ( !gameOver )
        {
            int rowIndex = UnityEngine.Random.Range ( 0, gridManager.RowMax );
            int colIndex = UnityEngine.Random.Range ( 0, gridManager.ColumnMax );
            gridManager.SpanwObjectAtGridIndex ( collectibleInstance.gameObject, rowIndex, colIndex );
            collectibleInstance.ShowCollectible ();

            yield return new WaitForSeconds ( levelData.CollectibleDisappearTimer );
        }
    }

    private void removePlayer ()
    {
        if ( playerInstance != null )
        {
            Destroy (playerInstance.gameObject);
            playerInstance = null;
        }
    }

    private void removeCollectible ()
    {
        if ( collectibleInstance != null )
        {
            Destroy (collectibleInstance.gameObject);
            collectibleInstance = null;
        }
    }

    private IEnumerator timer ()
    {
        while ( !gameOver )
        {
            yield return new WaitForSeconds ( 1.0f );
            
            timerSeconds--;
            int minutes = TimeSpan.FromSeconds ( timerSeconds ).Minutes;
            int secs = TimeSpan.FromSeconds ( timerSeconds ).Seconds;
            string timeRemaining = minutes.ToString("00") + ":" + secs.ToString("00");
            EventsManager.UpdateTimer?.Invoke ( timeRemaining );

            if ( timerSeconds <= 0 )
            {
                gameOver = true;
                scoreboard.UpdatePlayerScore ( PhotonNetwork.LocalPlayer.ActorNumber.ToString(), score );
                EventsManager.GameOver?.Invoke ();
            }
        }
    }
}