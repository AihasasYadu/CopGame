using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectivityManager : MonoBehaviourPunCallbacks
{
    private const string DEFAULT_ROOM_NAME = "Game";

    private const int MIN_PLAYERS_REQUIRED = 2;

    private const float WAIT_TIME = 5.0f;

    public void Start ()
    {
        PhotonNetwork.ConnectUsingSettings ();
        
        EventsManager.CreateRoomTapped += createRoom;
        EventsManager.JoinRoomTapped += joinRoom;
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby ();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        EventsManager.JoinedLobby?.Invoke ();
    }

    private void createRoom ()
    {
        CreateRoom ();
    }

    public void CreateRoom ( string roomName = null )
    {
        string roomToCreate = null;

        if ( !string.IsNullOrEmpty ( roomName ) )
        {
            roomToCreate = roomName;
        }
        else
        {
            roomToCreate = DEFAULT_ROOM_NAME;
        }

        PhotonNetwork.CreateRoom ( roomToCreate );
    }

    private void joinRoom ()
    {
        JoinRoom ();
    }

    public void JoinRoom ( string roomName = null )
    {
        string roomToJoin = null;

        if ( !string.IsNullOrEmpty ( roomName ) )
        {
            roomToJoin = roomName;
        }
        else
        {
            roomToJoin = DEFAULT_ROOM_NAME;
        }

        PhotonNetwork.JoinRoom ( roomToJoin );
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        EventsManager.JoinedRoom?.Invoke ();
        StartCoroutine (waitForOtherPlayers ());
    }

    private IEnumerator waitForOtherPlayers ()
    {
        yield return new WaitForSeconds ( WAIT_TIME );

        if ( PhotonNetwork.PlayerList.Length >= MIN_PLAYERS_REQUIRED )
        {
            EventsManager.StartLevelSetup?.Invoke ();
        }
        else
        {
            PhotonNetwork.LeaveRoom ();
            EventsManager.PlayersNotFound?.Invoke ();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        
        if ( PhotonNetwork.PlayerList.Length < MIN_PLAYERS_REQUIRED )
        {
            EventsManager.GameOver?.Invoke ();
            PhotonNetwork.LeaveRoom ();
            OnJoinedLobby ();
        }
    }
}
