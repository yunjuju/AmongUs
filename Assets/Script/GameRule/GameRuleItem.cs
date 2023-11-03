using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//게임 규칙 설정 UI를 연 플레이어가 호스트가 아닌 클라이언트라면
//게임 규칙을 수정하지 못하도록 수정 가능한 UI들을 비활성화 시켜줌

public class GameRuleItem : MonoBehaviour
{
    [SerializeField]
    private GameObject inactiveObject;

    void Start()
    {
        //자신의 플레이어가 isServer가 아니라면
        if(!AmongUsRoomPlayer.MyRoomPlayer.isServer)
        {
            //비활성화
            inactiveObject.SetActive(false);
        }    
    }

}
