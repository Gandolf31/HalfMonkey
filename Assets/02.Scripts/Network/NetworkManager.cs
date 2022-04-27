using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public InputField RoomName;
    public GameObject onNetwork;
    public GameObject onServer;
    public GameObject pause;
    public bool onCreate;

    Vector3 Stage1 = new Vector3(0, -0.8f, -3);
    Vector3 SPlayer = new Vector3(-8, -2, 1);
    Vector3 LPlayer = new Vector3(-10, -2, 1);

    // Start is called before the first frame update
    void Awake()
    {
        Screen.SetResolution(960, 540, false);
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;

        PhotonNetwork.ConnectUsingSettings();
    }

    //public void Connet() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster()
    {
        /*onNetwork.SetActive(false);
        onServer.SetActive(true);*/
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 2 }, null);
    }

    /*public void JoinOrCreateRoom()
    {
        PhotonNetwork.JoinOrCreateRoom(RoomName.text, new RoomOptions { MaxPlayers = 2 }, null);
        onServer.SetActive(false);
    }*/
    public override void OnCreatedRoom()
    {
        onCreate = true;
    }

    public override void OnJoinedRoom()
    {
        print("방 참가");
        //PhotonNetwork.Instantiate("PLayer", LPlayer, Quaternion.identity);
        //PhotonNetwork.Instantiate("PLayerL", SPlayer, Quaternion.identity);
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
    }

    public override void OnDisconnected(DisconnectCause cause) //서버 연결 끊기
    {
        onNetwork.SetActive(false);
    }

    public void Spawn() //플레이어 불러오기
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("PlayerS", SPlayer, Quaternion.identity);
            PhotonNetwork.Instantiate("WholeMap", Stage1, Quaternion.identity);
        }
        else if (!PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("PlayerL", LPlayer, Quaternion.identity);
        }
    }

    [ContextMenu("정보")]
    void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            print("현재방 이름 : " + PhotonNetwork.CurrentRoom.Name);
            print("현재방 인원 수 : " + PhotonNetwork.CurrentRoom.PlayerCount);
            print("현재방 최대 인원 수 : " + PhotonNetwork.CurrentRoom.MaxPlayers);

            string platyerStr = "방에 있는 플레이어 목록 : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                platyerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            }
            print(platyerStr);
        }
        else
        {
            print("접속한 인원 수 : " + PhotonNetwork.CountOfPlayers);
            print("방 개수 : " + PhotonNetwork.CountOfRooms);
            print("모든 방에 있는 인원 수 : " + PhotonNetwork.CountOfPlayersInRooms);
            print("로비 연결? : " + PhotonNetwork.InLobby);
            print("연결? : " + PhotonNetwork.IsConnected);
        }
    }
}
