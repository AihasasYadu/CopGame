using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const int WALL_LAYER = 7;
    private const int COLLECTIBLE_LAYER = 8;

    [SerializeField]
    private float characterSpeed = 5f;

    [SerializeField]
    private Animator characterAnim = null;

    private PhotonView photonView = null;
    private bool isPlayerMoving = false;
    private bool flipVertical = false;
    private bool flipHorizontal = false;

    private Coroutine flipCoroutine = null;

    public void Start ()
    {
        photonView = GetComponent <PhotonView>();

        if ( photonView.IsMine )
        {
            EventsManager.StartPlayerMovement += movePlayer;
            EventsManager.StopPlayerMovement += stopPlayer;
            EventsManager.RotatePlayer += rotatePlayer;
        }
    }

    private void movePlayer ()
    {
        transform.position += transform.forward * characterSpeed * Time.deltaTime;

        if (characterAnim != null && !isPlayerMoving)
        {
            characterAnim.SetBool ( "IsPlayerMoving", true );
            isPlayerMoving = true;
        }
    }

    private void stopPlayer ()
    {
        if (characterAnim != null)
        {
            characterAnim.SetBool ( "IsPlayerMoving", false );
            isPlayerMoving = false;
        }
    }

    private void rotatePlayer ( float angle )
    {
        if (flipVertical)
        {
            angle = 180 - angle;
        }
        else if (flipHorizontal)
        {
            angle *= -1;
        }

        transform.eulerAngles = new Vector3(0, angle, 0);
    }

    public void OnCollisionEnter (Collision collider)
    {
        if (collider.gameObject.layer.Equals (WALL_LAYER))
        {
            WallScript wall = collider.gameObject.GetComponentInParent<WallScript>();

            if (wall != null)
            {
                if (wall.IsVericalDirectionWall)
                {
                    flipVertical = true;
                }
                else
                {
                    flipHorizontal = true;
                }
            }

            if (flipCoroutine == null)
            {
                flipCoroutine = StartCoroutine ( FlipAngle() );
            }
        }

        if ( collider.gameObject.layer.Equals ( COLLECTIBLE_LAYER ) && photonView.IsMine )
        {
            collider.gameObject.SetActive ( false );
            collider.gameObject.GetComponent<CollectibleController>().HideCollectible ();
            EventsManager.DonutCollected?.Invoke(collider.gameObject.GetComponent<CollectibleController>().CollectibleValue);
        }
    }

    private IEnumerator FlipAngle ()
    {
        rotatePlayer ( transform.eulerAngles.y );

        yield return new WaitForSeconds ( 0.5f );

        flipVertical = false;
        flipHorizontal = false;
        flipCoroutine = null;
    }

    public void OnDestroy ()
    {
        if ( photonView.IsMine )
        {
            EventsManager.StartPlayerMovement -= movePlayer;
            EventsManager.StopPlayerMovement -= stopPlayer;
            EventsManager.RotatePlayer -= rotatePlayer;
        }
    }
}