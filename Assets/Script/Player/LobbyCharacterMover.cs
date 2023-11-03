using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


//�Լ� ������ ������ ��� ������ ������ �� �ڽ��� Lobby Player character�� �����ؾ��ϴµ�
//���� �ִ� ������Ʈ�� ã�Ƴ��� ����� ���ɿ� �Ҹ��� �κ��� �ֱ� ������ �ٸ� ����� ����ؾ���

public class LobbyCharacterMover : CharacterMove
{
    [SyncVar(hook = nameof(SetOnwerNetId_Hook))]
    public uint ownerNetID;



    // ownerNetId�� ����Ǹ� Ŭ���̾�Ʈ���� ȣ��
    // �޾ƿ� ownerNetId�� �̿��� �ڽ��� RoomPlayer�� ã�� ���� �ű⿡ ������ LobbyCharacterMover ����
    public void SetOnwerNetId_Hook(uint _, uint newOwnerId)
    {
        var players = FindObjectsOfType<AmongUsRoomPlayer>();

        foreach(var player in players)
        {
            if(newOwnerId == player.netId)
            {
                player.lobbyPlayerCharacter = this;
                break;
            }
        }
    }



    //���� �� ������ �� �ֵ��� ����
    public void CompleteSpawn()
    {
        if(hasAuthority)
        {
            IsMoveable = true;
        }
    }
}
