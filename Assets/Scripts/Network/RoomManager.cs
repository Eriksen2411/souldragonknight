using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static readonly string ROOM_PROPERTIES_MISSING_TYPE_KEY = "MissingType";

    [SerializeField] private string gameSceneName;
    [SerializeField] private string mainMenuSceneName;
    [SerializeField] private string roomSceneName;

    public static void UpdateRoomProperty(string key, object value)
    {
        Hashtable roomProperty = new Hashtable();
        roomProperty[key] = value;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperty);
    }

    public void RestartLevel()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.DestroyAll();
            photonView.RPC("RPC_LoadGameLevel", RpcTarget.All);
        }
    }

    [PunRPC]
    public void RPC_LoadGameLevel()
    {
        PhotonNetwork.LoadLevel(gameSceneName);
    }

    [PunRPC]
    public void RPC_LoadRoomLevel()
    {
        PhotonNetwork.LoadLevel(roomSceneName);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"Player {otherPlayer.ActorNumber} has left the game");

        UpdateRoomProperty(ROOM_PROPERTIES_MISSING_TYPE_KEY, 
            otherPlayer.CustomProperties[PlayerSpawner.PLAYER_PROPERTIES_TYPE_KEY]);

        photonView.RPC("RPC_LoadRoomLevel", RpcTarget.All);
        base.OnPlayerLeftRoom(otherPlayer);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.LoadLevel(mainMenuSceneName);

        Hashtable playerProperties = new Hashtable();
        playerProperties[PlayerSpawner.PLAYER_PROPERTIES_TYPE_KEY] = null;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
    }
}