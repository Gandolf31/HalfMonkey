using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class NetworkBeta : MonoBehaviourPunCallbacks
{
    public InputField RoomName;
    public GameObject onNetwork;
    public bool onCreate = false;
    string randomRoom;
    Pause thePause;
    // Start is called before the first frame update
    void Awake()
    {
        Screen.SetResolution(960, 540, false);
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;

        PhotonNetwork.ConnectUsingSettings();
    }

    private void Start()
    {
        thePause = FindObjectOfType<Pause>();
    }

    public override void OnConnectedToMaster()
    {
 
    }

    public override void OnCreatedRoom()
    {
        RoomName.text = randomRoom;
    }

    public override void OnJoinedRoom()
    {
        print("�� ����");
        onCreate = true;
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            thePause.pause();
        }

        if(onCreate)
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            onNetwork.SetActive(false);
        }
    }

    public override void OnDisconnected(DisconnectCause cause) //���� ���� ����
    {
        onNetwork.SetActive(false);
    }

    public void Spawn() //�÷��̾� �ҷ�����
    {

    }

    public void CreateRoom()
    {
        int random = Random.Range(1, 1000);
        randomRoom = random.ToString();
        PhotonNetwork.CreateRoom(randomRoom, new RoomOptions { MaxPlayers = 2 }, null);
    }

    public void JoinRoom()
    {

        PhotonNetwork.JoinRoom(RoomName.text);
    }

    [ContextMenu("����")]
    void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            print("����� �̸� : " + PhotonNetwork.CurrentRoom.Name);
            print("����� �ο� �� : " + PhotonNetwork.CurrentRoom.PlayerCount);
            print("����� �ִ� �ο� �� : " + PhotonNetwork.CurrentRoom.MaxPlayers);

            string platyerStr = "�濡 �ִ� �÷��̾� ��� : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                platyerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            }
            print(platyerStr);
        }
        else
        {
            print("������ �ο� �� : " + PhotonNetwork.CountOfPlayers);
            print("�� ���� : " + PhotonNetwork.CountOfRooms);
            print("��� �濡 �ִ� �ο� �� : " + PhotonNetwork.CountOfPlayersInRooms);
            print("�κ� ����? : " + PhotonNetwork.InLobby);
            print("����? : " + PhotonNetwork.IsConnected);
        }
    }
}
