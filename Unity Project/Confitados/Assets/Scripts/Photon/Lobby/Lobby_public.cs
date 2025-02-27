﻿using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using UnityEngine;
using UnityEngine.UI;

public class Lobby_public : MonoBehaviourPunCallbacks
{
    /****************************
     *
     * Documentation: https://doc-api.photonengine.com/en-us/current/getting-started/pun-intro
     * Scripting API: https://doc-api.photonengine.com/en/pun/v2/index.html
     * 
     **********************/

    #region Variables

    public Button connectButton;
    public Button joinRandomButton;
    public Text log;

    public byte maxPlayersInRoom = 4;
    public byte minPlayersInRoom = 2;
    public int currentPlayersInRoom = 0;
    public Text playerCount;

    #endregion
    
    #region UnityCallbacks
        
    void Start()
    {
        //PhotonNetwork.ConnectUsingSettings(); //Connect to Photon servers

        //There are other ways to connect the game to server, which can be founded here:
        //     https://doc-api.photonengine.com/en/pun/v2/class_photon_1_1_pun_1_1_photon_network.html
    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.CurrentRoom != null)
            currentPlayersInRoom = PhotonNetwork.CurrentRoom.PlayerCount;

        playerCount.text = currentPlayersInRoom + "/" + maxPlayersInRoom;
    }
    
    #endregion

    #region Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("Ahora estamos conectados al servidor de la región: " + PhotonNetwork.CloudRegion);

        //Enable the join room button
        connectButton.interactable = false;
        joinRandomButton.interactable = true;
    }

    public override void OnJoinedRoom()
    {
        //base.OnJoinedRoom();

        log.text += "\nJoined to room";
        
        //Disable the join room button to prevent the user from joining multiple rooms.
        joinRandomButton.interactable = false;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //base.OnJoinRandomFailed(returnCode, message);

        log.text += "\nThere are no rooms available. Creating a new one";

        //Create a new room
        if (PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions()
            {MaxPlayers = maxPlayersInRoom}))
        {
            log.text += "\nRoom created.";
        }
        else
        {
            log.text += "\nError. Unable to create a new room";
        }
    }
    
    #endregion

    #region Methods

    public void Connect()
    {
        if (PhotonNetwork.ConnectUsingSettings())
            log.text += "\nConnection to server established";
        else
            log.text += "\nError. Couldn't connect to server...";
    }

    public void JoinRandom()
    {
        if (!PhotonNetwork.JoinRandomRoom())
            log.text += "\nError. Couldn't join any room.";
    }

    #endregion
}