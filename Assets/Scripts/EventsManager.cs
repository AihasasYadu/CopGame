using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public static Action StartPlayerMovement = null;

    public static Action StopPlayerMovement = null;

    public static Action<float> RotatePlayer = null;

    public static Action<int> UpdateScore = null;

    public static Action<int> DonutCollected = null;

    public static Action JoinedLobby = null;

    public static Action CreateRoomTapped = null;

    public static Action JoinRoomTapped = null;

    public static Action JoinedRoom = null;

    public static Action StartLevelSetup = null;

    public static Action LevelSetupComplete = null;

    public static Action PlayersNotFound = null;

    public static Action GameOver = null;

    public static Action<string> UpdateTimer = null;

    public static Action<List<PlayerDataVO>> DisplayScoreboard = null;
}
