using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class GameRoomPlayerCounter : NetworkBehaviour
{
    //���� ����ȭ 
    [SyncVar]
    private int minPlayer;
    [SyncVar]
    private int maxPlayer;

    [SerializeField]
    private Text playerCountText;

    //���� �÷��̾� �� ������Ʈ

    public void UpdatePlayerCount()
    {
        //�÷��̾� ������Ʈ���� ã�Ƽ� �濡 ������ �÷��̾��� ���� playerCountText�� ����ϵ��� ������ִ� ��� ����
        var players = FindObjectsOfType<AmongUsRoomPlayer>();
        bool isStartable = players.Length >= minPlayer;
        playerCountText.color = isStartable ? Color.white : Color.red;
        playerCountText.text = string.Format("{0}/{1}", players.Length, maxPlayer);

        //���� ������ �ο��� ���� �ּ� �ο� �̻��� ��쿡 ���� ȣ���ϵ��� �������
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
