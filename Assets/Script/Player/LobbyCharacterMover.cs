using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


//함수 내에서 색상을 멤버 변수에 저장한 뒤 자신의 Lobby Player character에 전달해야하는데
//씬에 있는 오브젝트를 찾아내는 기능은 성능에 불리한 부분이 있기 때문에 다른 방법을 사용해야함

public class LobbyCharacterMover : CharacterMove
{
    [SyncVar(hook = nameof(SetOnwerNetId_Hook))]
    public uint ownerNetID;



    // ownerNetId가 변경되면 클라이언트에서 호출
    // 받아온 ownerNetId를 이용해 자신의 RoomPlayer를 찾은 다음 거기에 생성된 LobbyCharacterMover 저장
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



    //스폰 후 움직일 수 있도록 만듦
    public void CompleteSpawn()
    {
        if(hasAuthority)
        {
            IsMoveable = true;
        }
    }
}
