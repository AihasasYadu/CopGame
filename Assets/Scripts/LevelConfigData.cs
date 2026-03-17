using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "LevelData", menuName = "SO/LevelDataSO")]
public class LevelConfigData : ScriptableObject
{
    public int CollectibleValue = 1;

    public float CollectibleDisappearTimer = 3;

    public int TimerSeconds = 40;
}
