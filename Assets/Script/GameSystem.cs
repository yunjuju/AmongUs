using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Unity.VisualScripting;


//GameSystem은 게임 내내 자주 불러오게 됨 - 싱글톤 패턴 사용

//게임이 시작되면 플레이어의 객체를 찾아 GameSystem 클래스에 저장
//GmaeSystem의 Start 함수가 호출될 시점에 각플레어들의 객체인 ingameCharacterMover 들이 모두 제대로 생성되었는지 보장 X
//ingameCharacterMover 객체를 직접 찾는 것이 아니라
//ingameCharacterMover객체가 스스로 GameSystem 클래스에 자신을 등록하도록 만들어야함

public class GameSystem : NetworkBehaviour
{

    public static GameSystem Instance;


    //플레이어 객체 저장
    private List<IngameCharacterMover> players = new List<IngameCharacterMover>();

    //플레이어 등록

    public void AddPalyer(IngameCharacterMover player)
        {
        if (!players.Contains(player)) 
        {
            players.Add(player);

        }
    }

    private IEnumerator GameReady()
    {
        //AmongUsRoomManager의 roomSlots에 들어있는 AmongUsRoomPlayer의 수와
        //GameSystem의 players에 둘어있는 IngameCharacterMover의 수를 비교 후
        //모든 플레이어가 players에 등록되었는지 검사하면서 기다림

        var manager = NetworkManager.singleton as AmongUsRoomManager;

        while(manager.roomSlots.Count != players.Count)
        {
            yield return null;
        }

        //플레이어 리스트에 등록된 플레이어 중 임포스터 선정
        for(int i = 0; i < manager.imposterCount; i++)
        {
            var player = players[Random.Range(0, players.Count)];

            if(player.playerType != EPlayerType.Imposter)
            {
                player.playerType = EPlayerType.Imposter;
            }
            else
            {
                i--;
            }
        }

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(IngameUIManager.Instance.IngameIntroUI.ShowIntroSequence());

    }

    //플레이 리스트 가져와서 사용할 수 있도록 함수 구현
    public List<IngameCharacterMover> GetPlayerList()
    {
        return players;
    }


    public void Awake()
    {
        Instance = this; 
    }

    void Start()
    {
        StartCoroutine(GameReady());
    }
    

}
