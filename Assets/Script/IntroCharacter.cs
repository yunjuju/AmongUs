using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class IntroCharacter : MonoBehaviour
{

    //ĳ���� �̹���
    [SerializeField]
    private Image character;

    //ĳ���� �г���
    [SerializeField]
    private Text nickname;

    //ĳ���� �̹����� ���͸����� �ν��Ͻ����ִ� �۾� ó��
    //ĳ������ �г��Ӱ� ������ ǥ���ϵ��� ����
    public void SetIntrocharacter(string nick, EPlayerColor playerColor)
    {
        var matInst = Instantiate(character.material);
        character.material = matInst;

        nickname.text = nick;
        character.material.SetColor("_PlayerColor", PlayerColor.GetColor(playerColor));
    }
}
