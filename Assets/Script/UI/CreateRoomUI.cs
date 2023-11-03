using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

//새로 만드는 방의 데이터를 저장해두고 방 생성이 완료될 때 새로 만들어지는 방에 전달
public class CreateRoomUI : MonoBehaviour
{
    [SerializeField]
    private List<Image> crewImgs;

    [SerializeField]
    private List<Button> imposterCountButtons;

    [SerializeField]
    private List<Button> maxPlayerCountButtons;

    private CreateGameRoomData roomData; //멤버 변수로 만들어줌

    void Start()
    {
        //crewImgs에 머터리얼을 instantiate 함수로 복제된 머터리얼 인스턴스 만듦
        for (int i = 0; i < crewImgs.Count; i++)
        {
            Material materialInstane = Instantiate(crewImgs[i].material);
            crewImgs[i].material = materialInstane;
        }


        // roomdata 기본 값으로 임포스터 수 1, 최대 플레이어 수 10을 넣어주고 updatecrewimages 호출
        roomData = new CreateGameRoomData() { imposterCount = 1, maxPlayerCount = 10 };

        UpdateCrewImages();
    }

    //임포스터 수 설정 버튼
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


    //플레이어 수 설정 버튼
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


    //맵 배너 위에 올라가 있는 크루원의 이미지가 임포스터 수와 플레이어 수에 따라서 바뀌도록 만들어줌
    private void UpdateCrewImages()
    {
        //플레이어 색상 흰색으로 초기화 
        for (int i = 0; i < crewImgs.Count; i++)
        {
            crewImgs[i].material.SetColor("_PlayerColor", Color.white);
        }

        int imposterCount = roomData.imposterCount; //룸 데이터에 저장돼 있는 임포스터 수 
        int idx = 0;
        while (imposterCount != 0)
        {
            if (idx >= roomData.maxPlayerCount)
            {
                idx = 0;
            }
            // 임포스터 수가 0이 될 때까지 크루원의 이미지를 랜덤으로 뽑아 빨간색으로 만들어줌
            if (crewImgs[idx].material.GetColor("_PlayerColor") != Color.red && Random.Range(0, 5) == 0)
            {
                //크루원중에 임포스터가 몇 명이나 숨어있는지 이미지 표현
                crewImgs[idx].material.SetColor("_PlayerColor", Color.red);
                imposterCount--;
            }
            idx++;
        }
        // 설정한 플레이어 수 만큼만 크루원 이미지를 활성화시키고 나머지는 비활성화
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
        //방 설정 작업 처리
        //roomData에 저장되어있는 imposterCount와 maxPlayerCount를 이용해서 각 값들 세팅
        //방에 접속할 수 있는 수 구현
        manager.minPlayerCount = roomData.imposterCount == 1 ? 1 : roomData.imposterCount == 2 ? 7 : 9;
        manager.imposterCount = roomData.imposterCount;

        //최대 접속자 수 제한
        manager.maxConnections = roomData.maxPlayerCount;
        manager.StartHost();
    }

}

public class CreateGameRoomData //새로 만드는 방의 데이터를 저장해두고 방 생성이 완료 될 때 새로 만들어지는 방에 전달
{
    public int imposterCount;

    public int maxPlayerCount;
}
