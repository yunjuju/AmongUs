using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Unity.VisualScripting;


//GameSystem�� ���� ���� ���� �ҷ����� �� - �̱��� ���� ���

//������ ���۵Ǹ� �÷��̾��� ��ü�� ã�� GameSystem Ŭ������ ����
//GmaeSystem�� Start �Լ��� ȣ��� ������ ���÷������ ��ü�� ingameCharacterMover ���� ��� ����� �����Ǿ����� ���� X
//ingameCharacterMover ��ü�� ���� ã�� ���� �ƴ϶�
//ingameCharacterMover��ü�� ������ GameSystem Ŭ������ �ڽ��� ����ϵ��� ��������

public class GameSystem : NetworkBehaviour
{

    public static GameSystem Instance;


    //�÷��̾� ��ü ����
    private List<IngameCharacterMover> players = new List<IngameCharacterMover>();

    //�÷��̾� ���

    public void AddPalyer(IngameCharacterMover player)
        {
        if (!players.Contains(player)) 
        {
            players.Add(player);

        }
    }

    private IEnumerator GameReady()
    {
        //AmongUsRoomManager�� roomSlots�� ����ִ� AmongUsRoomPlayer�� ����
        //GameSystem�� players�� �Ѿ��ִ� IngameCharacterMover�� ���� �� ��
        //��� �÷��̾ players�� ��ϵǾ����� �˻��ϸ鼭 ��ٸ�

        var manager = NetworkManager.singleton as AmongUsRoomManager;

        while(manager.roomSlots.Count != players.Count)
        {
            yield return null;
        }

        //�÷��̾� ����Ʈ�� ��ϵ� �÷��̾� �� �������� ����
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

    //�÷��� ����Ʈ �����ͼ� ����� �� �ֵ��� �Լ� ����
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
