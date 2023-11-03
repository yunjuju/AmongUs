using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
    public static LobbyUIManager Instance; //�̱���

    [SerializeField]
    private CustomizeUI customizeUI;

    public CustomizeUI CustomizeUI { get { return customizeUI; } }

    [SerializeField]
    private GameRoomPlayerCounter gameRoomPlayerCounter;
    public GameRoomPlayerCounter GameRoomPlayerCounter { get { return gameRoomPlayerCounter; } }


    [SerializeField]
    private Button useButton; // Use ��ư 
    [SerializeField]
    private Sprite originalUseButtonSprite; //Use ��ư ���� ��������Ʈ


    [SerializeField]
    private Button startButton; // Start ��ư


    private void Awake()
    {
        Instance = this;
    }

    public void SetUseButton(Sprite sprite, UnityAction action)
    {
        useButton.image.sprite = sprite; //��ư ��������Ʈ �̹���

        // ��ư ������ �� ���� ��� ����� �Լ��� ��ư�� onclick �̺�Ʈ�� ����
        useButton.onClick.AddListener(action);
        useButton.interactable = true; //��ư Ȱ��ȭ
    }

    //��ȣ�ۿ� ������Ʈ���� �־����� �� Use ��ư ���󺹱�
    public void UnsetUseButton()
    {
        useButton.image.sprite = originalUseButtonSprite;
        useButton.onClick.RemoveAllListeners();
        useButton.interactable = false; //��ư ��Ȱ��ȭ
    }

    public void ActiveStartButton()
    {
        //startButton Ȱ��ȭ
        startButton.gameObject.SetActive(true);
    }

    public void SetInetactableStartButton(bool isInteractable)
    {
        //��ư ��ȣ�ۿ� ��� ���ų� �ѵ���
        startButton.interactable = isInteractable;
    }

    public void OnClickStartButton()
    {
        var manager = NetworkManager.singleton as AmongUsRoomManager;



        //���� �÷��� ������ �̵� �� AMongUSRoomManager�� GameRuleData ����
        manager.gameRuleData = FindObjectOfType<GameRuleStore>().GetGameRuleData();

        //��� �÷��̾��� readyToBegin�� true�� �����ؼ� �غ�����ְ�
        var players = FindObjectsOfType<AmongUsRoomPlayer>();


        for (int i = 0; i < players.Length; i++)
        {
            //players[i].readyToBegin = true;

            FindObjectOfType<NetworkRoomPlayer>().SetreadyToBegin(true);
        }

        //�� �Ŵ����� GamePlay ������ ��ȯ�ϵ��� �������
        manager.ServerChangeScene(manager.GameplayScene);
    }

}
