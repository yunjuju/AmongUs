using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AmongUsRoomManager : NetworkRoomManager
{
    public GameRuleData gameRuleData;

    public int minPlayerCount;
    public int imposterCount;


    //�������� ���� ������ Ŭ���̾�Ʈ�� �������� �� �����ϴ� �Լ�
    public override void OnRoomServerConnect(NetworkConnectionToClient conn)
        //�� ������ RoomPlayer�� �����Ǳ� ������
    {
        base.OnRoomServerConnect(conn);


    }

}
