using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System;

public class CustomizeUI : MonoBehaviour
{
    [SerializeField]
    private Button colorButton;
    [SerializeField]
    private GameObject colorPanel;
    [SerializeField]
    private Button gameRuleButton;
    [SerializeField]
    private GameObject gameRulePanel;


    //선택한 캐릭터 색상 미리 보여줄 image
    [SerializeField]
    private Image characterPreview;

    //색상 버튼 상태 저장
    [SerializeField]
    private List<ColorSelectButton> colorSelectButtons;

    //클릭된 버튼에 따라서 활성화된 버튼의 색상을 바꾸고 패널을 전환시키도록 해줌
    public void ActiveColorPanel()
    {
        colorButton.image.color = new Color(0,0,0,0.75f);
        gameRuleButton.image.color = new Color(0,0,0,0.25f);

        colorPanel.SetActive(true);
        gameRulePanel.SetActive(false);
    }

    public void ActiveGameRulePanel()
    {
        colorButton.image.color = new Color(0, 0, 0, 0.25f);
        gameRuleButton.image.color = new Color(0, 0, 0, 0.75f);

        colorPanel.SetActive(false);
        gameRulePanel.SetActive(true);
    }


    void Start()
    {
        //characterPreview의 material을 건드릴 때 원본 material에 영향을 미치지 않도록 이미지 material을 인스턴싱
        var inst = Instantiate(characterPreview.material);
        characterPreview.material = inst;
    }

    public void OnEnable()
    {
        UpdateColorButton();
        ActiveColorPanel();

        var roomSlots = (NetworkManager.singleton as AmongUsRoomManager).roomSlots;

        //프리뷰에 자신의 캐릭터 색상이 보여야 하므로, RoomSlots에서 자기 캐릭터 찾아서 호출
        foreach(var player in roomSlots)
        {
            var aPlayer = player as AmongUsRoomPlayer;
            if (aPlayer.isLocalPlayer)
            {
                UpdatePreviewColor(aPlayer.playerColor);
                    break;
            }
        }
    }

    //플레이어들의 색상 상태에 따라 버튼 상태 업데이트
    public void UpdateColorButton()
    {
        var roolSlots = (NetworkManager.singleton as AmongUsRoomManager).roomSlots;

        //컬러버튼 모두 interactable true로 한 다음
        for(int i = 0; i < colorSelectButtons.Count; i++)
        {
            colorSelectButtons[i].SetInteractable(true);
        }

        //플레이어의 색상은 interactable false로 변경
        foreach(var player in roolSlots)
        {
            var aPlayer = player as AmongUsRoomPlayer;
            colorSelectButtons[(int)aPlayer.playerColor].SetInteractable(false);
        }
    }

    public void UpdateSelectColorButton(EPlayerColor color)
    {
        colorSelectButtons[(int)color].SetInteractable(false);
    }


    //받아온 EPlayerColor에 따라 캐릭터 이미지 색상 변경
    public void UpdatePreviewColor(EPlayerColor color)
    {
        characterPreview.material.SetColor("_PlayerColor",PlayerColor.GetColor(color));
    }


    //플레이어가 접속을 종료했을 때 버튼 선택을 해제
    public void UpdateUnselectColorButton(EPlayerColor color)
    {
        //매개변수로 받은 cololr 값에 해당하는 버튼을 활성화 시키도록 코드를 작성
        colorSelectButtons[(int)color].SetInteractable(true);

    }


    //색삭 버튼 누를 때 기능
    public void OnClickColorButton(int index)
    {
        //index 값으로 버튼의 상태를 검사하고 버튼이 선택 가능한 색상이라면
        //자신의 RoomPlayer를 가져와 색상을 변경하겠다는 신호 보냄
        //RoomPlayer 가져오는 작업은 AmongUsRoomPlayer class에서 작업
        if (colorSelectButtons[index].isInteractable)
        {
            AmongUsRoomPlayer.MyRoomPlayer.CmdSetPlayerColor((EPlayerColor)index);
            UpdatePreviewColor((EPlayerColor)index);
        }
    }
    //UI 클릭
    public void Open()
    {
        //움직이지 못하게
        AmongUsRoomPlayer.MyRoomPlayer.lobbyPlayerCharacter.IsMoveable = false;
        //UI 켜줌
        gameObject.SetActive(true);
    }

    public void Close()
    {
        //움직이도록
        AmongUsRoomPlayer.MyRoomPlayer.lobbyPlayerCharacter.IsMoveable = true;
        //UI 끔
        gameObject.SetActive(false);
    }
}
