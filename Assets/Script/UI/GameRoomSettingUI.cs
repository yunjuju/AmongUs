using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GameRoomSettingUI : SettingUI
{

    public void Open()
    {
        AmongUsRoomPlayer.MyRoomPlayer.lobbyPlayerCharacter.IsMoveable = false;
        gameObject.SetActive(true); //UI Ȱ��ȭ
    }

    public override void Close()
    {
        base.Close();   
        //�θ� Ŭ������ Close �Լ� ������
        AmongUsRoomPlayer.MyRoomPlayer.lobbyPlayerCharacter.IsMoveable = true;
        gameObject.SetActive(false); //UI ��Ȱ��ȭ
    }
    public void ExitGameRoom()
    {
        //��Ʈ��ũ �Ŵ��� ������
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
