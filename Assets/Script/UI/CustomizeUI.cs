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


    //������ ĳ���� ���� �̸� ������ image
    [SerializeField]
    private Image characterPreview;

    //���� ��ư ���� ����
    [SerializeField]
    private List<ColorSelectButton> colorSelectButtons;

    //Ŭ���� ��ư�� ���� Ȱ��ȭ�� ��ư�� ������ �ٲٰ� �г��� ��ȯ��Ű���� ����
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
        //characterPreview�� material�� �ǵ帱 �� ���� material�� ������ ��ġ�� �ʵ��� �̹��� material�� �ν��Ͻ�
        var inst = Instantiate(characterPreview.material);
        characterPreview.material = inst;
    }

    public void OnEnable()
    {
        UpdateColorButton();
        ActiveColorPanel();

        var roomSlots = (NetworkManager.singleton as AmongUsRoomManager).roomSlots;

        //�����信 �ڽ��� ĳ���� ������ ������ �ϹǷ�, RoomSlots���� �ڱ� ĳ���� ã�Ƽ� ȣ��
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

    //�÷��̾���� ���� ���¿� ���� ��ư ���� ������Ʈ
    public void UpdateColorButton()
    {
        var roolSlots = (NetworkManager.singleton as AmongUsRoomManager).roomSlots;

        //�÷���ư ��� interactable true�� �� ����
        for(int i = 0; i < colorSelectButtons.Count; i++)
        {
            colorSelectButtons[i].SetInteractable(true);
        }

        //�÷��̾��� ������ interactable false�� ����
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


    //�޾ƿ� EPlayerColor�� ���� ĳ���� �̹��� ���� ����
    public void UpdatePreviewColor(EPlayerColor color)
    {
        characterPreview.material.SetColor("_PlayerColor",PlayerColor.GetColor(color));
    }


    //�÷��̾ ������ �������� �� ��ư ������ ����
    public void UpdateUnselectColorButton(EPlayerColor color)
    {
        //�Ű������� ���� cololr ���� �ش��ϴ� ��ư�� Ȱ��ȭ ��Ű���� �ڵ带 �ۼ�
        colorSelectButtons[(int)color].SetInteractable(true);

    }


    //���� ��ư ���� �� ���
    public void OnClickColorButton(int index)
    {
        //index ������ ��ư�� ���¸� �˻��ϰ� ��ư�� ���� ������ �����̶��
        //�ڽ��� RoomPlayer�� ������ ������ �����ϰڴٴ� ��ȣ ����
        //RoomPlayer �������� �۾��� AmongUsRoomPlayer class���� �۾�
        if (colorSelectButtons[index].isInteractable)
        {
            AmongUsRoomPlayer.MyRoomPlayer.CmdSetPlayerColor((EPlayerColor)index);
            UpdatePreviewColor((EPlayerColor)index);
        }
    }
    //UI Ŭ��
    public void Open()
    {
        //�������� ���ϰ�
        AmongUsRoomPlayer.MyRoomPlayer.lobbyPlayerCharacter.IsMoveable = false;
        //UI ����
        gameObject.SetActive(true);
    }

    public void Close()
    {
        //�����̵���
        AmongUsRoomPlayer.MyRoomPlayer.lobbyPlayerCharacter.IsMoveable = true;
        //UI ��
        gameObject.SetActive(false);
    }
}
