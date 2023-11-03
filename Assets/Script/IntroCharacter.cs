using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class IntroCharacter : MonoBehaviour
{

    //캐릭터 이미지
    [SerializeField]
    private Image character;

    //캐릭터 닉네임
    [SerializeField]
    private Text nickname;

    //캐릭터 이미지의 머터리얼을 인스턴싱해주는 작업 처리
    //캐릭터의 닉네임과 색상을 표현하도록 설정
    public void SetIntrocharacter(string nick, EPlayerColor playerColor)
    {
        var matInst = Instantiate(character.material);
        character.material = matInst;

        nickname.text = nick;
        character.material.SetColor("_PlayerColor", PlayerColor.GetColor(playerColor));
    }
}
