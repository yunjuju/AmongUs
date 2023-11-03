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
        //inputField가 비어있는지 확인
        if(nicknameInputField.text != "")
        {
            PlayerSettings.nickname = nicknameInputField.text;
            //비어있다면 입력 강조 애니메이션 실행
            createRoomUI.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            //닉네임이 입력되어 있다면 방 만들기로 넘어감
            nicknameInputField.GetComponent<Animator>().SetTrigger("on");
        }
    }

    public void OnClickEnterRoomButton()
    {

        //inputField가 비어있는지 확인
        if (nicknameInputField.text != "")
        {
            //닉네임 저장 
            PlayerSettings.nickname = nicknameInputField.text;

            //비어 있다면 입력 필드를 강조하는 애니메이션 실행
            var manager = AmongUsRoomManager.singleton;

            manager.StartClient();
        }
        else
        {
            //닉네임이 입력되어 있다면 방 만들기로 넘어감
            nicknameInputField.GetComponent<Animator>().SetTrigger("on");
        }



    }
}
