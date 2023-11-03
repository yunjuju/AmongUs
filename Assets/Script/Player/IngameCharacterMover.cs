using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;


public enum EPlayerType
{
    Crew,
    Imposter
}

public class IngameCharacterMover : CharacterMove
{


    //열겨형 변수를 IngameCharacterMover 클래스에 추가해주고 변수 값이 공유될 수 있도록해줌
    [SyncVar]
    public EPlayerType playerType;




    //플레이어 자신이 권한을 가지고 있는 캐릭터인지 판단 후
    //자기 자신의 RoomPlayer를 찾아서 닉네임과 색상을 세팅해줌

    public override void Start()
    {
        base.Start();

        //characterMove에 대한 권한을 가지고있는지 확인
        if (hasAuthority)
        {
            //임시코드
            IsMoveable = true;

            //권한을 가지고 있다면 MyroomPlayer를 찾아서 CmdSetPlayerCharacter 함수를 호출하고
            //매개변수로 자기 플레이어의 닉네임과 색상을 넣어줌
            var myRoomPlayer = AmongUsRoomPlayer.MyRoomPlayer;

            CmdSetPlayerCharacter(myRoomPlayer.nickname, myRoomPlayer.playerColor);
        }

        //자신 등록
        GameSystem.Instance.AddPalyer(this);
    }


    [Command]
    private void CmdSetPlayerCharacter(string nickname, EPlayerColor Color)
    {
        //닉네임과 플레이어 색상 변수에 저장
        this.nickname = nickname;
        playerColor = Color;

    }
}
