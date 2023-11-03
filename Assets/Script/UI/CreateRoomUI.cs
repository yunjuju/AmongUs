using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

//���� ����� ���� �����͸� �����صΰ� �� ������ �Ϸ�� �� ���� ��������� �濡 ����
public class CreateRoomUI : MonoBehaviour
{
    [SerializeField]
    private List<Image> crewImgs;

    [SerializeField]
    private List<Button> imposterCountButtons;

    [SerializeField]
    private List<Button> maxPlayerCountButtons;

    private CreateGameRoomData roomData; //��� ������ �������

    void Start()
    {
        //crewImgs�� ���͸����� instantiate �Լ��� ������ ���͸��� �ν��Ͻ� ����
        for (int i = 0; i < crewImgs.Count; i++)
        {
            Material materialInstane = Instantiate(crewImgs[i].material);
            crewImgs[i].material = materialInstane;
        }


        // roomdata �⺻ ������ �������� �� 1, �ִ� �÷��̾� �� 10�� �־��ְ� updatecrewimages ȣ��
        roomData = new CreateGameRoomData() { imposterCount = 1, maxPlayerCount = 10 };

        UpdateCrewImages();
    }

    //�������� �� ���� ��ư
    public void UpdateImposterCount(int count)
    {
        roomData.imposterCount = count;

        for (int i = 0; i < imposterCountButtons.Count; i++)
        {
            if (i == count - 1)
            {
                imposterCountButtons[i].image.color = new Color(1f, 1f, 1f, 1f);

            }
            else
            {
                imposterCountButtons[i].image.color = new Color(1f, 1f, 1f, 0f);
            }
        }

        int limitMaxPlayer = count == 1 ? 1 : count == 2 ? 7 : 9;
        if (roomData.maxPlayerCount < limitMaxPlayer)
        {
            UpdateMaxPlayerCount(limitMaxPlayer);
        }
        else
        {
            UpdateMaxPlayerCount(roomData.maxPlayerCount);
        }

        for (int i = 0; i < maxPlayerCountButtons.Count; i++)
        {
            var text = maxPlayerCountButtons[i].GetComponentInChildren<Text>();
            if (i < limitMaxPlayer - 4)
            {
                maxPlayerCountButtons[i].interactable = false;
                text.color = Color.gray;
            }
            else
            {
                maxPlayerCountButtons[i].interactable = true;
                text.color = Color.white;
            }
        }


    }


    //�÷��̾� �� ���� ��ư
    public void UpdateMaxPlayerCount(int count)
    {
        roomData.maxPlayerCount = count;

        for (int i = 0; i < maxPlayerCountButtons.Count; i++)
        {
            if (i == count - 4)
            {
                maxPlayerCountButtons[i].image.color = new Color(1f, 1f, 1f, 1f);

            }
            else
            {
                maxPlayerCountButtons[i].image.color = new Color(1f, 1f, 1f, 0f);
            }
        }

        UpdateCrewImages();
    }


    //�� ��� ���� �ö� �ִ� ũ����� �̹����� �������� ���� �÷��̾� ���� ���� �ٲ�� �������
    private void UpdateCrewImages()
    {
        //�÷��̾� ���� ������� �ʱ�ȭ 
        for (int i = 0; i < crewImgs.Count; i++)
        {
            crewImgs[i].material.SetColor("_PlayerColor", Color.white);
        }

        int imposterCount = roomData.imposterCount; //�� �����Ϳ� ����� �ִ� �������� �� 
        int idx = 0;
        while (imposterCount != 0)
        {
            if (idx >= roomData.maxPlayerCount)
            {
                idx = 0;
            }
            // �������� ���� 0�� �� ������ ũ����� �̹����� �������� �̾� ���������� �������
            if (crewImgs[idx].material.GetColor("_PlayerColor") != Color.red && Random.Range(0, 5) == 0)
            {
                //ũ����߿� �������Ͱ� �� ���̳� �����ִ��� �̹��� ǥ��
                crewImgs[idx].material.SetColor("_PlayerColor", Color.red);
                imposterCount--;
            }
            idx++;
        }
        // ������ �÷��̾� �� ��ŭ�� ũ��� �̹����� Ȱ��ȭ��Ű�� �������� ��Ȱ��ȭ
        for (int i = 0; i < crewImgs.Count; i++)
        {
            if (i < roomData.maxPlayerCount)
                crewImgs[i].gameObject.SetActive(true);

            else
                crewImgs[i].gameObject.SetActive(false);
        }

    }

    public void CreateRoom()
    {
        var manager = AmongUsRoomManager.singleton as AmongUsRoomManager;
        //�� ���� �۾� ó��
        //roomData�� ����Ǿ��ִ� imposterCount�� maxPlayerCount�� �̿��ؼ� �� ���� ����
        //�濡 ������ �� �ִ� �� ����
        manager.minPlayerCount = roomData.imposterCount == 1 ? 1 : roomData.imposterCount == 2 ? 7 : 9;
        manager.imposterCount = roomData.imposterCount;

        //�ִ� ������ �� ����
        manager.maxConnections = roomData.maxPlayerCount;
        manager.StartHost();
    }

}

public class CreateGameRoomData //���� ����� ���� �����͸� �����صΰ� �� ������ �Ϸ� �� �� ���� ��������� �濡 ����
{
    public int imposterCount;

    public int maxPlayerCount;
}
