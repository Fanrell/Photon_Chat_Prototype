using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PhotonChat.Menus
{
    public class MainMenu : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject findOponentPanel = null;
        [SerializeField] private GameObject waitingStatusPanel = null;
        [SerializeField] private TextMeshProUGUI waitingStatusText = null;

        private bool isConnecting = false;

        private const string GameVersion = "0.1";
        private const int MaxPalyerPerRoom = 2;

        private void Awake() => PhotonNetwork.AutomaticallySyncScene = true;

        public void FindOpponnent()
        {
            isConnecting = true;

            findOponentPanel.SetActive(false);
            waitingStatusPanel.SetActive(true);

            waitingStatusText.text = "Searching...";

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.GameVersion = GameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connceted to Master");
            if(isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            waitingStatusPanel.SetActive(false);
            findOponentPanel.SetActive(true);

            Debug.Log($"Disconnected due to: {cause}");
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("No clients are waiting for an opponent, create new room");
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPalyerPerRoom });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Client succesfully join a room");
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

            if (playerCount != MaxPalyerPerRoom)
            {
                waitingStatusText.text = "Waiting for opponent";
                Debug.Log("Client is waiting for the opponent");
            }
            else
            {
                waitingStatusText.text = "Opponent found";
                Debug.Log("Maching is ready to begin");
            }
        }
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if(PhotonNetwork.CurrentRoom.PlayerCount == MaxPalyerPerRoom)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                Debug.Log("Match is ready to begin");
                waitingStatusText.text = "Opponent found";

                PhotonNetwork.LoadLevel("Scene_main");
            }
        }

    }
}
