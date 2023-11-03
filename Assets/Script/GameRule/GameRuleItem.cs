using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ��Ģ ���� UI�� �� �÷��̾ ȣ��Ʈ�� �ƴ� Ŭ���̾�Ʈ���
//���� ��Ģ�� �������� ���ϵ��� ���� ������ UI���� ��Ȱ��ȭ ������

public class GameRuleItem : MonoBehaviour
{
    [SerializeField]
    private GameObject inactiveObject;

    void Start()
    {
        //�ڽ��� �÷��̾ isServer�� �ƴ϶��
        if(!AmongUsRoomPlayer.MyRoomPlayer.isServer)
        {
            //��Ȱ��ȭ
            inactiveObject.SetActive(false);
        }    
    }

}
