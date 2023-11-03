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
            //myRoomPlayer가 비어있는 경우 한 번만 찾아서 저장 후 반환하는 프로퍼티 생성
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

    [SyncVar(hook = nameof(SetPlayerColor_Hook))] // 네트워크 통해 동기화
    public EPlayerColor playerColor;



    // RoomPlayer의 PlayerColor 변경될 때 호출되는 hook
    public void SetPlayerColor_Hook(EPlayerColor oldColor, EPlayerColor newColor)
    {
        // 이전 색상 비활성화
        LobbyUIManager.Instance.CustomizeUI.UpdateUnselectColorButton(oldColor);
        //UpdateSelectColorButton 함수 호출하도록 변경
        LobbyUIManager.Instance.CustomizeUI.UpdateSelectColorButton(newColor);
    }

    [SyncVar]
    public string nickname;



    public CharacterMove lobbyPlayerCharacter;

    //플레이어가 방에 접속했을 때 한번, 플레이어가 방에서 나갔을 때 한 번, 업데이트 되도록 해줘야함 (플레이어 수 갱신)


    public void Start()
    {
        base.Start();

        if (isServer)
        {
            SpawnLobbyPlayerCharacter();

            //스타트 버튼 활성화
            LobbyUIManager.Instance.ActiveStartButton();
        }
        //플레이어가 방에 접속했을 때 한번, 플레이어가 방에서 나갔을 때 한 번, 업데이트 되도록 해줘야함 (플레이어 수 갱신)
        LobbyUIManager.Instance.GameRoomPlayerCounter.UpdatePlayerCount();

        //로컬 플레이어일 때 닉네임 불러오는 함수 호출
        if(isLocalPlayer)
        {
            CmdSetNickname(PlayerSettings.nickname);
        }

    }

    private void OnDestroy()
    {
        if (LobbyUIManager.Instance != null)
        {
            //플레이어가 방에 접속했을 때 한번, 플레이어가 방에서 나갔을 때 한 번, 업데이트 되도록 해줘야함 (플레이어 수 갱신)
            LobbyUIManager.Instance.GameRoomPlayerCounter.UpdatePlayerCount();
            LobbyUIManager.Instance.CustomizeUI.UpdateUnselectColorButton(playerColor);

        }
    }

    [Command]
    //자신의 닉네임과 로비 캐릭터의 닉네임에 세팅
    public void CmdSetNickname(string nick)
    {
        nickname = nick;
        lobbyPlayerCharacter.nickname = nick;
    }


    //클라이언트에서 서버로 신호 보내는 기능
    [Command]
    public void CmdSetPlayerColor(EPlayerColor color)
    {

        playerColor = color;
        lobbyPlayerCharacter.playerColor = color;
    }

    private void SpawnLobbyPlayerCharacter()
    {
        // 대기실에 접속 중인 플레이어를 가져옴
        var roomSlots = (NetworkManager.singleton as AmongUsRoomManager).roomSlots;
        EPlayerColor color = EPlayerColor.Red;

        // 플레이어들이 든 roomSlots를 돌면서 사용하지 않는 색상 고름
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
        // 자신의 색상으로 지정
        playerColor = color;


        var spawnPositions = FindObjectOfType<SpawnPositions>();
        int index = spawnPositions.Index;


        Vector3 spawnPos = spawnPositions.GetSpawnPosition();



        //함수 호출 후 lobby player character 프리팹을 spawnPrefabs로부터 가져와서 인스턴스화 시킨 후
        // nerworkserver.spawn 함수로 클라이언트에게 이 게임오브젝트가 소환되었음을 알려줌
        var playerCharacter = Instantiate(AmongUsRoomManager.singleton.spawnPrefabs[0], spawnPos, Quaternion.identity).GetComponent<LobbyCharacterMover>();

        playerCharacter.transform.localScale = index < 5 ? new Vector3(0.5f, 0.5f, 1f) : new Vector3(-0.5f, 0.5f, 1f);

        NetworkServer.Spawn(playerCharacter.gameObject, connectionToClient);

        playerCharacter.ownerNetID = netId;

        playerCharacter.playerColor = color;
        //두번째 매개변수에 방금 서버에 접속한 플레이어의 정보를 담고있는 NetworkConnection을 전달
        // 방금 소환된 오브젝트가 새로 접속한 플레이어의 소유임을 알려줌



    }
}
