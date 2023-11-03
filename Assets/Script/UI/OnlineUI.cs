using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class OnlineUI : MonoBehaviour
{
    [SerializeField]
    private InputField nicknameInputField;
    [SerializeField]
    private GameObject createRoomUI;

    public void OnclickCreateRoomButton()
    {
        //inputField�� ����ִ��� Ȯ��
        if(nicknameInputField.text != "")
        {
            PlayerSettings.nickname = nicknameInputField.text;
            //����ִٸ� �Է� ���� �ִϸ��̼� ����
            createRoomUI.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            //�г����� �ԷµǾ� �ִٸ� �� ������ �Ѿ
            nicknameInputField.GetComponent<Animator>().SetTrigger("on");
        }
    }

    public void OnClickEnterRoomButton()
    {

        //inputField�� ����ִ��� Ȯ��
        if (nicknameInputField.text != "")
        {
            //�г��� ���� 
            PlayerSettings.nickname = nicknameInputField.text;

            //��� �ִٸ� �Է� �ʵ带 �����ϴ� �ִϸ��̼� ����
            var manager = AmongUsRoomManager.singleton;

            manager.StartClient();
        }
        else
        {
            //�г����� �ԷµǾ� �ִٸ� �� ������ �Ѿ
            nicknameInputField.GetComponent<Animator>().SetTrigger("on");
        }



    }
}
