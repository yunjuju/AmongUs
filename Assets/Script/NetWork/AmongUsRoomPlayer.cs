using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class AmongUsRoomPlayer : NetworkRoomPlayer
{

    private static AmongUsRoomPlayer myRoomPlayer;
    public static AmongUsRoomPlayer MyRoomPlayer
    {
        get
        {
            //myRoomPlayer�� ����ִ� ��� �� ���� ã�Ƽ� ���� �� ��ȯ�ϴ� ������Ƽ ����
            if (myRoomPlayer == null)
            {
                var players = FindObjectsOfType<AmongUsRoomPlayer>();

                foreach (var player in players)
                {
                    if (player.hasAuthority)
                    {
                        myRoomPlayer = player;

                    }
                }
            }
            return myRoomPlayer;
        }
    }

    [SyncVar(hook = nameof(SetPlayerColor_Hook))] // ��Ʈ��ũ ���� ����ȭ
    public EPlayerColor playerColor;



    // RoomPlayer�� PlayerColor ����� �� ȣ��Ǵ� hook
    public void SetPlayerColor_Hook(EPlayerColor oldColor, EPlayerColor newColor)
    {
        // ���� ���� ��Ȱ��ȭ
        LobbyUIManager.Instance.CustomizeUI.UpdateUnselectColorButton(oldColor);
        //UpdateSelectColorButton �Լ� ȣ���ϵ��� ����
        LobbyUIManager.Instance.CustomizeUI.UpdateSelectColorButton(newColor);
    }

    [SyncVar]
    public string nickname;



    public CharacterMove lobbyPlayerCharacter;

    //�÷��̾ �濡 �������� �� �ѹ�, �÷��̾ �濡�� ������ �� �� ��, ������Ʈ �ǵ��� ������� (�÷��̾� �� ����)


    public void Start()
    {
        base.Start();

        if (isServer)
        {
            SpawnLobbyPlayerCharacter();

            //��ŸƮ ��ư Ȱ��ȭ
            LobbyUIManager.Instance.ActiveStartButton();
        }
        //�÷��̾ �濡 �������� �� �ѹ�, �÷��̾ �濡�� ������ �� �� ��, ������Ʈ �ǵ��� ������� (�÷��̾� �� ����)
        LobbyUIManager.Instance.GameRoomPlayerCounter.UpdatePlayerCount();

        //���� �÷��̾��� �� �г��� �ҷ����� �Լ� ȣ��
        if(isLocalPlayer)
        {
            CmdSetNickname(PlayerSettings.nickname);
        }

    }

    private void OnDestroy()
    {
        if (LobbyUIManager.Instance != null)
        {
            //�÷��̾ �濡 �������� �� �ѹ�, �÷��̾ �濡�� ������ �� �� ��, ������Ʈ �ǵ��� ������� (�÷��̾� �� ����)
            LobbyUIManager.Instance.GameRoomPlayerCounter.UpdatePlayerCount();
            LobbyUIManager.Instance.CustomizeUI.UpdateUnselectColorButton(playerColor);

        }
    }

    [Command]
    //�ڽ��� �г��Ӱ� �κ� ĳ������ �г��ӿ� ����
    public void CmdSetNickname(string nick)
    {
        nickname = nick;
        lobbyPlayerCharacter.nickname = nick;
    }


    //Ŭ���̾�Ʈ���� ������ ��ȣ ������ ���
    [Command]
    public void CmdSetPlayerColor(EPlayerColor color)
    {

        playerColor = color;
        lobbyPlayerCharacter.playerColor = color;
    }

    private void SpawnLobbyPlayerCharacter()
    {
        // ���ǿ� ���� ���� �÷��̾ ������
        var roomSlots = (NetworkManager.singleton as AmongUsRoomManager).roomSlots;
        EPlayerColor color = EPlayerColor.Red;

        // �÷��̾���� �� roomSlots�� ���鼭 ������� �ʴ� ���� ��
        for (int i = 0; i < (int)EPlayerColor.Lime + 1; i++)
        {
            bool isFindSameColor = false;
            foreach (var roomPlayer in roomSlots)
            {
                var amongUsRoomPlayer = roomPlayer as AmongUsRoomPlayer;
                if (amongUsRoomPlayer.playerColor == (EPlayerColor)i && roomPlayer.netId != netId)
                {
                    isFindSameColor = true;
                    break;
                }
            }
            if (!isFindSameColor)
            {
                color = (EPlayerColor)i;
                break;
            }
        }
        // �ڽ��� �������� ����
        playerColor = color;


        var spawnPositions = FindObjectOfType<SpawnPositions>();
        int index = spawnPositions.Index;


        Vector3 spawnPos = spawnPositions.GetSpawnPosition();



        //�Լ� ȣ�� �� lobby player character �������� spawnPrefabs�κ��� �����ͼ� �ν��Ͻ�ȭ ��Ų ��
        // nerworkserver.spawn �Լ��� Ŭ���̾�Ʈ���� �� ���ӿ�����Ʈ�� ��ȯ�Ǿ����� �˷���
        var playerCharacter = Instantiate(AmongUsRoomManager.singleton.spawnPrefabs[0], spawnPos, Quaternion.identity).GetComponent<LobbyCharacterMover>();

        playerCharacter.transform.localScale = index < 5 ? new Vector3(0.5f, 0.5f, 1f) : new Vector3(-0.5f, 0.5f, 1f);

        NetworkServer.Spawn(playerCharacter.gameObject, connectionToClient);

        playerCharacter.ownerNetID = netId;

        playerCharacter.playerColor = color;
        //�ι�° �Ű������� ��� ������ ������ �÷��̾��� ������ ����ִ� NetworkConnection�� ����
        // ��� ��ȯ�� ������Ʈ�� ���� ������ �÷��̾��� �������� �˷���



    }
}
