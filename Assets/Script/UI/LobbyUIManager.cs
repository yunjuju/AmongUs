using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
    public static LobbyUIManager Instance; //싱글톤

    [SerializeField]
    private CustomizeUI customizeUI;

    public CustomizeUI CustomizeUI { get { return customizeUI; } }

    [SerializeField]
    private GameRoomPlayerCounter gameRoomPlayerCounter;
    public GameRoomPlayerCounter GameRoomPlayerCounter { get { return gameRoomPlayerCounter; } }


    [SerializeField]
    private Button useButton; // Use 버튼 
    [SerializeField]
    private Sprite originalUseButtonSprite; //Use 버튼 원본 스프라이트


    [SerializeField]
    private Button startButton; // Start 버튼


    private void Awake()
    {
        Instance = this;
    }

    public void SetUseButton(Sprite sprite, UnityAction action)
    {
        useButton.image.sprite = sprite; //버튼 스프라이트 이미지

        // 버튼 눌렀을 때 동작 기능 담당할 함수를 버튼의 onclick 이벤트에 세팅
        useButton.onClick.AddListener(action);
        useButton.interactable = true; //버튼 활성화
    }

    //상호작용 오브젝트에서 멀어졌을 때 Use 버튼 원상복구
    public void UnsetUseButton()
    {
        useButton.image.sprite = originalUseButtonSprite;
        useButton.onClick.RemoveAllListeners();
        useButton.interactable = false; //버튼 비활성화
    }

    public void ActiveStartButton()
    {
        //startButton 활성화
        startButton.gameObject.SetActive(true);
    }

    public void SetInetactableStartButton(bool isInteractable)
    {
        //버튼 상호작용 기능 끄거나 켜도록
        startButton.interactable = isInteractable;
    }

    public void OnClickStartButton()
    {
        var manager = NetworkManager.singleton as AmongUsRoomManager;



        //게임 플레이 씬으로 이동 전 AMongUSRoomManager에 GameRuleData 저장
        manager.gameRuleData = FindObjectOfType<GameRuleStore>().GetGameRuleData();

        //모든 플레이어의 readyToBegin을 true로 변경해서 준비시켜주고
        var players = FindObjectsOfType<AmongUsRoomPlayer>();


        for (int i = 0; i < players.Length; i++)
        {
            //players[i].readyToBegin = true;

            FindObjectOfType<NetworkRoomPlayer>().SetreadyToBegin(true);
        }

        //룸 매니저가 GamePlay 씬으로 전환하도록 만들어줌
        manager.ServerChangeScene(manager.GameplayScene);
    }

}
