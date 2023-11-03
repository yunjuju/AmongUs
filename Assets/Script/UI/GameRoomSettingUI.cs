using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GameRoomSettingUI : SettingUI
{

    public void Open()
    {
        AmongUsRoomPlayer.MyRoomPlayer.lobbyPlayerCharacter.IsMoveable = false;
        gameObject.SetActive(true); //UI 활성화
    }

    public override void Close()
    {
        base.Close();   
        //부모 클래스의 Close 함수 재정의
        AmongUsRoomPlayer.MyRoomPlayer.lobbyPlayerCharacter.IsMoveable = true;
        gameObject.SetActive(false); //UI 비활성화
    }
    public void ExitGameRoom()
    {
        //네트워크 매니저 가져옴
        var manager = AmongUsRoomManager.singleton;

        if (manager.mode == Mirror.NetworkManagerMode.Host)
        {
            manager.StopHost();
        }
        else if (manager.mode == Mirror.NetworkManagerMode.ClientOnly)
        {
            manager.StopClient();
        }
    }
}
