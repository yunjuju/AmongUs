using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class GameRoomPlayerCounter : NetworkBehaviour
{
    //변수 동기화 
    [SyncVar]
    private int minPlayer;
    [SyncVar]
    private int maxPlayer;

    [SerializeField]
    private Text playerCountText;

    //현재 플레이어 수 업데이트

    public void UpdatePlayerCount()
    {
        //플레이어 오브젝트들을 찾아서 방에 접속한 플레이어의 수를 playerCountText에 출력하도록 만들어주는 기능 구현
        var players = FindObjectsOfType<AmongUsRoomPlayer>();
        bool isStartable = players.Length >= minPlayer;
        playerCountText.color = isStartable ? Color.white : Color.red;
        playerCountText.text = string.Format("{0}/{1}", players.Length, maxPlayer);

        //현재 접속한 인원의 수가 최소 인원 이상인 경우에 따라 호출하도록 만들어줌
        LobbyUIManager.Instance.SetInetactableStartButton(isStartable);
    }

    void Start()
    {
        if(isServer)
        {
            var manager = NetworkManager.singleton as AmongUsRoomManager;
            minPlayer = manager.minPlayerCount;
            maxPlayer = manager.maxConnections;
        }
    }
}
