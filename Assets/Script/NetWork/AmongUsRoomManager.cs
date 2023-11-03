using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AmongUsRoomManager : NetworkRoomManager
{
    public GameRuleData gameRuleData;

    public int minPlayerCount;
    public int imposterCount;


    //서버에서 새로 접속한 클라이언트를 감지했을 때 동작하는 함수
    public override void OnRoomServerConnect(NetworkConnectionToClient conn)
        //이 시점은 RoomPlayer가 생성되기 이전임
    {
        base.OnRoomServerConnect(conn);


    }

}
