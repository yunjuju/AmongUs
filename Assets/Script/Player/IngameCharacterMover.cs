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


    //������ ������ IngameCharacterMover Ŭ������ �߰����ְ� ���� ���� ������ �� �ֵ�������
    [SyncVar]
    public EPlayerType playerType;




    //�÷��̾� �ڽ��� ������ ������ �ִ� ĳ�������� �Ǵ� ��
    //�ڱ� �ڽ��� RoomPlayer�� ã�Ƽ� �г��Ӱ� ������ ��������

    public override void Start()
    {
        base.Start();

        //characterMove�� ���� ������ �������ִ��� Ȯ��
        if (hasAuthority)
        {
            //�ӽ��ڵ�
            IsMoveable = true;

            //������ ������ �ִٸ� MyroomPlayer�� ã�Ƽ� CmdSetPlayerCharacter �Լ��� ȣ���ϰ�
            //�Ű������� �ڱ� �÷��̾��� �г��Ӱ� ������ �־���
            var myRoomPlayer = AmongUsRoomPlayer.MyRoomPlayer;

            CmdSetPlayerCharacter(myRoomPlayer.nickname, myRoomPlayer.playerColor);
        }

        //�ڽ� ���
        GameSystem.Instance.AddPalyer(this);
    }


    [Command]
    private void CmdSetPlayerCharacter(string nickname, EPlayerColor Color)
    {
        //�г��Ӱ� �÷��̾� ���� ������ ����
        this.nickname = nickname;
        playerColor = Color;

    }
}
