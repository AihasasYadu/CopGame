using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    private const int PLAYER_LAYER = 6;

    private const string METHOD_NAME = "updateCollectibleState";

    [SerializeField]
    private PhotonView collectiblePhotonView = null;

    private int collectibleValue = 1;
    public int CollectibleValue
    {
        get
        {
            return collectibleValue;
        }

        private set
        {
            int temp = value;
            if ( temp > 0 )
            {
                collectibleValue = temp;
            }
        }
    }

    public void SetupCollectible ( int value )
    {
        CollectibleValue = value;
    }

    [PunRPC]
    private void updateCollectibleState ( bool isEnable )
    {
        gameObject.SetActive ( isEnable );
    }

    public void ShowCollectible ()
    {
        collectiblePhotonView.RPC ( METHOD_NAME, RpcTarget.All, true);
    }

    public void HideCollectible ()
    {
        collectiblePhotonView.RPC ( METHOD_NAME, RpcTarget.All, false);
    }
}
